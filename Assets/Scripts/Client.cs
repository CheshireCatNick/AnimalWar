using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //使用Unity UI程式庫。

public class Client : MonoBehaviour
{

    private const int maxCharacterNum = 3, maxWeaponNum = 2, periodTime = 45;

    public int scale;

    public int playerID;

	public Animal[] animals = new Animal[2*maxCharacterNum];

	public GameObject fox, eagle, frog;

	public GameObject targetPrefab;
	private GameObject [] targets = new GameObject[maxCharacterNum];

    private Vector2 moveDelta, attackDelta;

    private int nowCharacterID;
    private Weapons[] weapons = new Weapons[maxWeaponNum];
    private Weapons nowWeapon;

    private ConnectionManager connectionManager;

    int time_int = periodTime;
    public Text time_UI;
    public Text command_UI;

    //Animal Object need a flag to show if it is completed  
    private enum stage
    {
        Character,
        Move,
        Weapon,
        Attack,
        Complete,
        WaitOpponent,
        ReplayMove,
		ReplayAttack,
		GameOver
    };

    private stage nowStage = stage.Character;

    private ActionObject[] actionObjects = new ActionObject[maxCharacterNum];
    private ActionObject[] replayActionObjects = new ActionObject[maxCharacterNum * 2];

    private GameObject[] shadows = new GameObject[maxCharacterNum];

	private float attackTimer;

    private void Start()
    {
        connectionManager = new ConnectionManager();
        playerID = int.Parse(connectionManager.ReceiveID());
        Debug.Log("PlayerID: " + playerID);

        for (int i = 0; i < maxCharacterNum; i++)
        {
            actionObjects[i] = new ActionObject(i);
        }

        for (int i = 0; i < 2 * maxCharacterNum; i++)
        { 
			Vector3 scale = transform.localScale;
			if (i >= 0 && i < maxCharacterNum) {
				actionObjects [i].moveTarget = new Vector2 (12f - i * 4.8f, -0.5f);
				scale.x *= -1;
			}
			if (i == 0 || i == 2*maxCharacterNum-1)
				animals [i] = new Animal (i, scale, fox, "fox");
			else if (i == 1 || i == 2*maxCharacterNum-2)
				animals [i] = new Animal (i, scale, frog, "frog");
			else
				animals [i] = new Animal (i, scale, eagle, "eagle");
		}

        weapons[0] = new Weapons("skip");
        weapons[1] = new Weapons("gun");
        nowWeapon = weapons[0];
        nowCharacterID = 0;
        moveDelta = Vector2.zero;
        attackDelta = Vector2.zero;

        //set timer
        time_int = periodTime;
        InvokeRepeating("Timecount", 1, 1);
        time_UI.text = TimeTextFormat();
        command_UI.text = CommandTextFormat("", "0 : first Character\n1 : second Character");
    }

    private void Update()
    {
        //check Timeout, if true, set nowStages to Complete and Replay
        print(nowStage);
        if (nowStage == stage.Character || nowStage == stage.Weapon)
        {
            for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha0 + maxCharacterNum; i++)
            {
                if (Input.GetKey(i))
                {
                    print(i);
                    //use select target Character
                    if (nowStage == stage.Character)
                    {
						if ((i - KeyCode.Alpha0 < maxCharacterNum) && (animals[i - KeyCode.Alpha0].player != null))
                        {
                            command_UI.text = CommandTextFormat((i - KeyCode.Alpha0).ToString(), "0 : first Character\n1 : second Character");
                            nowCharacterID = i - KeyCode.Alpha0;
                        }
                    }

                    //user select target weapon
                    else if (nowStage == stage.Weapon)
                    {

                        if (i - KeyCode.Alpha0 < maxWeaponNum)
                        {
                            command_UI.text = CommandTextFormat((i - KeyCode.Alpha0).ToString(), "0 : skip attack\n1 : gun");
                            nowWeapon = weapons[i - KeyCode.Alpha0];
                        }
                    }

                }
            }
        }
        if (nowStage == stage.Move || nowStage == stage.Attack)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                print("UPArrow");
                command_UI.text = CommandTextFormat("Up", "");
                //Character move
                if (nowStage == stage.Move)
                {
                    moveDelta += Vector2.up * scale;
                    Vector2 dst = shadows[nowCharacterID].transform.localPosition + Vector3.up * scale;
                    int playerIndex = (playerID == 1) ? nowCharacterID : maxCharacterNum * 2 - 1 - nowCharacterID;
                    if (animals[playerIndex].CanMove(dst))
                        shadows[nowCharacterID].GetComponent<Playermove>().Destination = dst;
                }

                //weapon target
                if (nowStage == stage.Attack)
                {
                    attackDelta += Vector2.up * scale;
					targets[nowCharacterID].GetComponent<Playermove>().Destination = targets[nowCharacterID].transform.localPosition + (Vector3.up * scale);
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                print("DownArrow");
                command_UI.text = CommandTextFormat("Down", "");
                //Character move
                if (nowStage == stage.Move)
                {
                    moveDelta += Vector2.down * scale;
                    Vector2 dst = shadows[nowCharacterID].transform.localPosition + Vector3.down * scale;
                    int playerIndex = (playerID == 1) ? nowCharacterID : maxCharacterNum * 2 - 1 - nowCharacterID;
                    if (animals[playerIndex].CanMove(dst))
                        shadows[nowCharacterID].GetComponent<Playermove>().Destination = dst;
                }

                //weapon target
                if (nowStage == stage.Attack)
                {
                    attackDelta += Vector2.down * scale;
					targets[nowCharacterID].GetComponent<Playermove>().Destination = targets[nowCharacterID].transform.localPosition + (Vector3.down * scale);
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                print("LeftArrow");
                command_UI.text = CommandTextFormat("Left", "");
                //Character move
                if (nowStage == stage.Move)
                {
                    moveDelta += Vector2.left * scale;
                    Vector2 dst = shadows[nowCharacterID].transform.localPosition + Vector3.left * scale;
                    int playerIndex = (playerID == 1) ? nowCharacterID : maxCharacterNum * 2 - 1 - nowCharacterID;
                    if (animals[playerIndex].CanMove(dst))
                        shadows[nowCharacterID].GetComponent<Playermove>().Destination = dst;
                }

                //weapon target
                if (nowStage == stage.Attack)
                {
                    attackDelta += Vector2.left * scale;
					targets[nowCharacterID].GetComponent<Playermove>().Destination = targets[nowCharacterID].transform.localPosition + (Vector3.left * scale);
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                print("RightArrow");
                command_UI.text = CommandTextFormat("Right", "");
                //Character move
                if (nowStage == stage.Move)
                {
                    moveDelta += Vector2.right * scale;
                    Vector2 dst = shadows[nowCharacterID].transform.localPosition + Vector3.right * scale;
                    int playerIndex = (playerID == 1) ? nowCharacterID : maxCharacterNum * 2 - 1 - nowCharacterID;
					print (animals [playerIndex].CanMove (dst));
                    if (animals[playerIndex].CanMove(dst))
                        shadows[nowCharacterID].GetComponent<Playermove>().Destination = dst;
                }

                //weapon target
                if (nowStage == stage.Attack)
                {
                    attackDelta += Vector2.right * scale;
					targets[nowCharacterID].GetComponent<Playermove>().Destination = targets[nowCharacterID].transform.localPosition + (Vector3.right * scale);
                }
            }

            /*
            //get mouse left click
            if (Input.GetMouseButtonDown(0))
            {
                print(Input.mousePosition);
                if (nowStage == stage.Attack)
                {
                    moveDelta = Input.mousePosition;
                }
            }*/
        }
        if (nowStage >= stage.Character && nowStage <= stage.Attack)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                print(KeyCode.Return);
                command_UI.text = CommandTextFormat("Enter", "");
                if (nowStage == stage.Character)
                {
                    actionObjects[nowCharacterID].characterID = nowCharacterID;
					if (shadows[nowCharacterID] == null)
					{
						int playerIndex = (playerID == 1) ? nowCharacterID : maxCharacterNum * 2 - 1 - nowCharacterID;
						shadows[nowCharacterID] = GameObject.Instantiate(animals[playerIndex].player);
                        shadows[nowCharacterID].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                    }
                    nowStage = stage.Move;
                    command_UI.text = CommandTextFormat("", "Please use the arrow key to move your animal.");
                }

                else if (nowStage == stage.Move)
                {
                    actionObjects[nowCharacterID].moveTarget = shadows[nowCharacterID].transform.localPosition;
                    nowStage = stage.Weapon;
                    command_UI.text = CommandTextFormat("", "0 : skip attack\n1 : gun");
                }

                else if (nowStage == stage.Weapon)
                {
                    actionObjects[nowCharacterID].weapon = nowWeapon;
                    nowStage = stage.Attack;
					command_UI.text = CommandTextFormat("", "Please use the arrow key to choose the target you want to attack.");
					//if nowWeapon is skip, then skip attack stage
					if (nowWeapon == weapons [0]) {
						actionObjects [nowCharacterID].attackTarget = actionObjects [nowCharacterID].moveTarget;
						actionObjects [nowCharacterID].isSet = true;
						nowStage = stage.Complete;
						foreach (ActionObject actionObject in actionObjects) {
							if (!actionObject.isSet) {
								nowCharacterID = actionObject.characterID;
								nowStage = stage.Character;
								moveDelta = Vector2.zero;
								attackDelta = Vector2.zero;
								nowWeapon = weapons [0];
								command_UI.text = CommandTextFormat ("", "0 : first Character\n1 : second Character");
								break;
							}
						}
					}
					else {
						if (targets[nowCharacterID] == null)
						{
							print ("new targets");
							targets[nowCharacterID] = GameObject.Instantiate(targetPrefab, shadows[nowCharacterID].transform.localPosition, shadows[nowCharacterID].transform.localRotation);
						}
						//show how big is my gun!!!
						if (nowWeapon == weapons [1]) {
							shadows [nowCharacterID].transform.GetChild (3).gameObject.SetActive (true);
							shadows[nowCharacterID].transform.GetChild(3).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
						}
					}
                }

                else if (nowStage == stage.Attack)
                {
					actionObjects[nowCharacterID].attackTarget = targets[nowCharacterID].transform.localPosition;
                    actionObjects[nowCharacterID].isSet = true;
                    //if it is the final animal then go to Complete, else go to Character
                    nowStage = stage.Complete;
                    foreach (ActionObject actionObject in actionObjects)
                    {
                        if (!actionObject.isSet)
                        {
                            nowCharacterID = actionObject.characterID;
                            nowStage = stage.Character;
                            moveDelta = Vector2.zero;
                            attackDelta = Vector2.zero;
                            nowWeapon = weapons[0];
                            command_UI.text = CommandTextFormat("", "0 : first Character\n1 : second Character");
                            break;
                        }
                    }
                }
            }
        }
        if (nowStage == stage.Complete)
        {
			time_UI.text = TimeTextFormat();
            CancelInvoke("Timecount");
            SendActions();
            nowStage = stage.WaitOpponent;
        }
        if (nowStage == stage.WaitOpponent)
        {
            time_UI.text = TimeTextFormat();

            string opponentActionStr = connectionManager.ReceiveActionStr();
            if (opponentActionStr != "")
            {
                string[] opponentActions = opponentActionStr.Split('/');
                if (playerID == 0)
                {
                    replayActionObjects[0] = new ActionObject(opponentActions[0]);
                    replayActionObjects[1] = new ActionObject(opponentActions[1]);
					replayActionObjects[2] = new ActionObject(opponentActions[2]);
					replayActionObjects[3] = actionObjects[2];
                    replayActionObjects[4] = actionObjects[1];
                    replayActionObjects[5] = actionObjects[0];
                }
                else if (playerID == 1)
                {
                    replayActionObjects[0] = actionObjects[0];
                    replayActionObjects[1] = actionObjects[1];
					replayActionObjects[2] = actionObjects[2];
					replayActionObjects[3] = new ActionObject(opponentActions[2]);
                    replayActionObjects[4] = new ActionObject(opponentActions[1]);
                    replayActionObjects[5] = new ActionObject(opponentActions[0]);
                }
                
                Replay(replayActionObjects);
                nowStage = stage.ReplayMove;
            }
        }
		if (nowStage == stage.ReplayMove) {
			time_UI.text = TimeTextFormat ();

			//Destroy shadows
			for (int i = 0; i < maxCharacterNum; i++) {
				Destroy (shadows [i]);
				Destroy (targets [i]);
			}

			nowStage = stage.ReplayAttack;
			foreach (Animal animal in animals) {
				if (animal.player != null && !animal.IsFinish ()) {
					nowStage = stage.ReplayMove;
					break;
				}
			}
			if (nowStage == stage.ReplayAttack) {
				for (int i = 0; i < 2 * maxCharacterNum; i++) {
					if (animals [i].player != null) {
						if (replayActionObjects [i].weapon.name == "gun") {
							animals [i].player.GetComponentInChildren<Gun> ().Shoot (replayActionObjects [i].attackTarget);
						}
					}
				}
				attackTimer = Time.time;
			}
		}
            
		if (nowStage == stage.ReplayAttack) {
			nowStage = stage.Character;
			if (Time.time - attackTimer < 4.0f)
				nowStage = stage.ReplayAttack;
			
			if (nowStage == stage.Character) {
				//hide your big gun!!!
				for (int i = 0; i < 2 * maxCharacterNum; i++) {
					if (animals [i].player != null) {
						animals [i].player.transform.GetChild (3).gameObject.SetActive (false);
					}
				}
				//check game over
				bool[] flags = { true, true };

				print ("init player 0 : " + flags [0] + " player 1: " + flags [1]);
				for (int i = 0; i < maxCharacterNum; i++) {
					if (animals [i].player != null) {
						flags [1] = false;
						if (playerID == 1)
							actionObjects [i].isSet = false;
					}
					if (animals [maxCharacterNum + i].player != null) {
						flags [0] = false;
						if (playerID == 0)
							actionObjects [maxCharacterNum - 1 - i].isSet = false;
					}
				}
				print ("player 0 : " + flags [0] + " player 1: " + flags [1]);
				for (int i = 0; i < 2; i++) {
					if (flags [i]) {
						command_UI.text = CommandTextFormat ("", "Player " + i.ToString () + " win!!\nPlease press enter to restart game,\nOr press esc to quit.");
						nowStage = stage.GameOver;
					}
				}
				if (flags [0] && flags [1]) {
					command_UI.text = CommandTextFormat ("", "Draw!!\nPlease press enter to restart game,\nOr press esc to quit.");
					nowStage = stage.GameOver;
				}

				if (nowStage != stage.GameOver) {
					//for (int i = 0; i < maxCharacterNum; i++)
					//    actionObjects[i].isSet = false;
					for (int i = 0; i < maxCharacterNum * 2; i++)
						animals [i].SetFinish (false);

					nowCharacterID = 0;
					nowWeapon = weapons [0];
					moveDelta = Vector2.zero;
					attackDelta = Vector2.zero;

					//set timer
					time_int = periodTime;
					InvokeRepeating ("Timecount", 1, 1);
					time_UI.text = TimeTextFormat ();
				}
			}
		}
		if (nowStage == stage.GameOver) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				connectionManager.Close ();
				Start ();
				nowStage = stage.Character;
			}
			if (Input.GetKeyDown (KeyCode.Escape))
				Application.Quit ();
		}
	}

    //receive actionArray from server
    private void SendActions()
    {
        string msg = playerID + "|";
        for (int i = 0; i < maxCharacterNum; i++)
            msg += actionObjects[i].ToString() + "/";
        connectionManager.Send(msg);
        return;
    }
    //show the result
    public void Replay(ActionObject[] actionArray)
    { 
        foreach (ActionObject a in actionArray)
        {
            print(a.ToString());
        }

        for (int i = 0; i < maxCharacterNum*2; i++)
        {
			if (animals [i].player != null) {
				//show how big is my gun!!!
				if (actionArray [i].weapon.name == "gun") {
					animals [i].player.transform.GetChild (3).gameObject.SetActive (true);
				}
				animals [i].Move (actionArray [i].moveTarget, actionArray [i].attackTarget);
			}
        }
    }

    private void OnDestroy()
    {
        connectionManager.Close();
    }

    private string CommandTextFormat (string command, string msg)
    {
        return "Your input : " + command + "\n" + msg;
    }

    private string TimeTextFormat ()
    {
        return "Time : " + time_int + "\nNow Stage : " + nowStage.ToString("g");
    }

    void Timecount()
    {
        time_int -= 1;

        time_UI.text = TimeTextFormat();

        if (time_int == 0)
        {   
            CancelInvoke("Timecount");

            Timeout();
        }

    }

    void Timeout()
    {
        print("Timeout");
        for (int i = 0; i < maxCharacterNum; i++)
        {
            if (!actionObjects[i].isSet)
            {
                actionObjects[i].weapon = weapons[0];
            }
        }
        if (!actionObjects[nowCharacterID].isSet)
        {
            if (nowStage - stage.Weapon >= 0)
                actionObjects[nowCharacterID].weapon = nowWeapon;
            else
                actionObjects[nowCharacterID].weapon = weapons[0];
			if (shadows[nowCharacterID] == null)
			{
				int playerIndex = (playerID == 1) ? nowCharacterID : maxCharacterNum * 2 - 1 - nowCharacterID;
				shadows[nowCharacterID] = GameObject.Instantiate(animals[playerIndex].player);
			}
            actionObjects[nowCharacterID].moveTarget = shadows[nowCharacterID].transform.localPosition;
			if (actionObjects[nowCharacterID].weapon != weapons[0])
				actionObjects[nowCharacterID].attackTarget = targets[nowCharacterID].transform.localPosition;
        }
        nowStage = stage.Complete;
    }
}
