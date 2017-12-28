using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //使用Unity UI程式庫。

public class Client : MonoBehaviour
{

    private const int maxCharacterNum = 2, maxWeaponNum = 2, periodTime = 45;

    public int scale;

    public int playerID;

    public GameObject player;

    public GameObject[] players = new GameObject[2*maxCharacterNum];

	public Animal[] animals = new Animal[2*maxCharacterNum];

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
        Replay
    };

    private stage nowStage = stage.Character;

    private ActionObject[] actionObjects = new ActionObject[maxCharacterNum];
    private ActionObject[] replayActionObjects = new ActionObject[maxCharacterNum * 2];

    private GameObject[] shadows = new GameObject[maxCharacterNum];

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
            animals[i] = new Animal(i);
            players[i] = (GameObject)GameObject.Instantiate(player, new Vector3(7.5f-i*5,-0.5f,0.0f),player.transform.rotation);
            //Do Flip
            if (i == 0 || i == 1)
            {
                actionObjects[i].moveTarget = new Vector2(7.5f - i * 5, -0.5f);
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                players[i].transform.localScale = theScale;
            }
            players[i].name = "player" + i.ToString();
            players[i].gameObject.layer = 10 + i;
            //add player to Animal class
            animals[i].SetAnimal(players[i]);
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
                if (Input.GetKeyDown(i))
                {
                    print(i);
                    //use select target Character
                    if (nowStage == stage.Character)
                    {

						if ((i - KeyCode.Alpha0 < maxCharacterNum) && (players[i - KeyCode.Alpha0] != null))
                        {
                            command_UI.text = CommandTextFormat((i - KeyCode.Alpha0).ToString(), "0 : first Character\n1 : second Character");
                            nowCharacterID = i - KeyCode.Alpha0;
                            if (shadows[(i - KeyCode.Alpha0)] == null)
                            {
                                int playerIndex = (playerID == 1) ? i - KeyCode.Alpha0 : maxCharacterNum * 2 - 1 - (i - KeyCode.Alpha0);
                                shadows[(i - KeyCode.Alpha0)] = GameObject.Instantiate(players[playerIndex]);
                            }
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
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                print("UPArrow");
                command_UI.text = CommandTextFormat("Up", "");
                //Character move
                if (nowStage == stage.Move)
                {
                    moveDelta += Vector2.up * scale;
                    shadows[nowCharacterID].GetComponent<Playermove>().Destination = shadows[nowCharacterID].transform.localPosition + (Vector3.up * scale);
                }

                //weapon target
                if (nowStage == stage.Attack)
                {
                    attackDelta += Vector2.up * scale;
                }
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                print("DownArrow");
                command_UI.text = CommandTextFormat("Down", "");
                //Character move
                if (nowStage == stage.Move)
                {
                    moveDelta += Vector2.down * scale;
                    shadows[nowCharacterID].GetComponent<Playermove>().Destination = shadows[nowCharacterID].transform.localPosition + (Vector3.down * scale);
                }

                //weapon target
                if (nowStage == stage.Attack)
                {
                    attackDelta += Vector2.down * scale;
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                print("LeftArrow");
                command_UI.text = CommandTextFormat("Left", "");
                //Character move
                if (nowStage == stage.Move)
                {
                    moveDelta += Vector2.left * scale;
                    shadows[nowCharacterID].GetComponent<Playermove>().Destination = shadows[nowCharacterID].transform.localPosition + (Vector3.left * scale);
                }

                //weapon target
                if (nowStage == stage.Attack)
                {
                    attackDelta += Vector2.left * scale;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                print("RightArrow");
                command_UI.text = CommandTextFormat("Right", "");
                //Character move
                if (nowStage == stage.Move)
                {
                    moveDelta += Vector2.right * scale;
                    shadows[nowCharacterID].GetComponent<Playermove>().Destination = shadows[nowCharacterID].transform.localPosition + (Vector3.right * scale);
                }

                //weapon target
                if (nowStage == stage.Attack)
                {
                    attackDelta += Vector2.right * scale;
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
                    print("in " + nowCharacterID);
                    actionObjects[nowCharacterID].characterID = nowCharacterID;
                    nowStage = stage.Move;
                    print("after " + nowStage);
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
                }

                else if (nowStage == stage.Attack)
                {
                    actionObjects[nowCharacterID].attackTarget = actionObjects[nowCharacterID].moveTarget + attackDelta;
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
                    replayActionObjects[0] = new ActionObject(opponentActionStr[0]);
                    replayActionObjects[1] = new ActionObject(opponentActionStr[1]);
                    replayActionObjects[2] = actionObjects[1];
                    replayActionObjects[3] = actionObjects[0];
                }
                else if (playerID == 1)
                {
                    replayActionObjects[0] = actionObjects[0];
                    replayActionObjects[1] = actionObjects[1];
                    replayActionObjects[2] = new ActionObject(opponentActionStr[1]);
                    replayActionObjects[3] = new ActionObject(opponentActionStr[0]);
                }
                Replay(replayActionObjects);
                nowStage = stage.Replay;
            }
        }
        if (nowStage == stage.Replay)
        {
            time_UI.text = TimeTextFormat();

            //Destroy shadows
            for (int i = 0; i < maxCharacterNum; i++)
            {
                Destroy(shadows[i]);
            }

            nowStage = stage.Character;
            foreach (GameObject player in players)
            {
                if (!player.GetComponent<Playermove>().isFinish)
                {
                    nowStage = stage.Replay;
                    break;
                }
            }
            if (nowStage == stage.Character)
            {
                for (int i = 0; i < maxCharacterNum; i++)
                    actionObjects[i].isSet = false;
                for (int i = 0; i < maxCharacterNum * 2; i++)
                    players[i].GetComponent<Playermove>().isFinish = false;

                nowCharacterID = 0;
                nowWeapon = weapons[0];
                moveDelta = Vector2.zero;
                attackDelta = Vector2.zero;

                //set timer
                time_int = periodTime;
                InvokeRepeating("Timecount", 1, 1);
                time_UI.text = TimeTextFormat();
            }
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
            players[i].GetComponent<Playermove>().Destination = actionArray[i].moveTarget;
            players[i].GetComponent<Playermove>().target = actionArray[i].attackTarget;
            players[i].GetComponent<Playermove>().isStart = true;
        }
     /*   
        for (int i = 0; i < maxCharacterNum*2; i++)
        {
            if(actionArray[i].weapon.name == "gun")
                players[i].GetComponentInChildren<Weapon>().Shoot(new Vector2(0.0f, 0.0f));
        }
       */ 
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
            actionObjects[nowCharacterID].moveTarget = shadows[nowCharacterID].transform.localPosition;
            actionObjects[nowCharacterID].attackTarget = actionObjects[nowCharacterID].moveTarget + attackDelta;
        }
        nowStage = stage.Complete;
    }
}
