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

    public GameObject[] players = new GameObject[4];

    public Animal[] animals = new Animal[4];

    private Vector2 moveDelta, attackDelta;

    private int nowCharacterID;
    private Weapons[] weapons = new Weapons[maxWeaponNum];
    private Weapons nowWeapon;

    private ConnectionManager connectionManager;

    int time_int = periodTime;
    public Text time_UI;


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
        time_UI.text = TextFormat();
    }

    private void Update()
    {
        //check Timeout, if true, set nowStaget to Complete and Replay
        print(nowStage);
        for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha0 + maxCharacterNum; i++)
        {
            if (Input.GetKeyDown(i))
            {
                print(i);
                //use select target Character
                if (nowStage == stage.Character)
                {
                    if (i - KeyCode.Alpha0 < maxCharacterNum)
                    {
                        nowCharacterID = i - KeyCode.Alpha0;
                    }
                }

                //user select target weapon
                else if (nowStage == stage.Weapon)
                {
                    if (i - KeyCode.Alpha0 < maxWeaponNum)
                    {
                        nowWeapon = weapons[i - KeyCode.Alpha0];
                    }
                }

                else { }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            print("UPArrow");
            //Character move
            if (nowStage == stage.Move)
            {
                moveDelta += Vector2.up * scale;
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
            //Character move
            if (nowStage == stage.Move)
            {
                moveDelta += Vector2.down * scale;
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
            //Character move
            if (nowStage == stage.Move)
            {
                moveDelta += Vector2.left * scale;
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
            //Character move
            if (nowStage == stage.Move)
            {
                moveDelta += Vector2.right * scale;
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

        if (Input.GetKeyDown(KeyCode.Return))
        {
            print(KeyCode.Return);
            if (nowStage == stage.Character)
            {
                print("in " + nowCharacterID);
                actionObjects[nowCharacterID].characterID = nowCharacterID;
                nowStage = stage.Move;
                print("after " + nowStage);
            }

            else if (nowStage == stage.Move)
            {
                actionObjects[nowCharacterID].moveTarget += moveDelta;
                nowStage = stage.Weapon;
            }

            else if (nowStage == stage.Weapon)
            {
                actionObjects[nowCharacterID].weapon = nowWeapon;
                nowStage = stage.Attack;
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
                        break;
                    }
                }
            }
        }
        if (nowStage == stage.Complete)
        {
            time_UI.text = TextFormat();
            CancelInvoke("Timecount");
            SendActions();
            nowStage = stage.WaitOpponent;
        }
        if (nowStage == stage.WaitOpponent)
        {
            time_UI.text = TextFormat();
            string opponentActionStr = connectionManager.ReceiveActionStr();
            if (opponentActionStr != "")
            {
                string[] opponentActions = opponentActionStr.Split('/');
                for (int i = 0; i < maxCharacterNum * 2; i++)
                {
                    if (i < maxCharacterNum)
                        replayActionObjects[i] = actionObjects[i];
                    else
                        replayActionObjects[i] = new ActionObject(opponentActions[i - maxCharacterNum]);
                }
                Replay(replayActionObjects);
                nowStage = stage.Replay;
            }
        }
        if (nowStage == stage.Replay)
        {
            time_UI.text = TextFormat();
            nowStage = stage.Character;
            foreach (Animal animal in animals)
            {
                if (!animal.isFinish)
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
                    animals[i].isFinish = false;

                nowCharacterID = 0;
                nowWeapon = weapons[0];
                moveDelta = Vector2.zero;
                attackDelta = Vector2.zero;

                //set timer
                time_int = periodTime;
                InvokeRepeating("Timecount", 1, 1);
                time_UI.text = TextFormat();
            }
        }

    }

    //receive actionArray from server
    private void SendActions()
    {
        string msg = playerID + "|";
        foreach (ActionObject action in actionObjects)
            msg += action.ToString() + "/";
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

    private string TextFormat ()
    {
        return "Time : " + time_int + "\nNow Stage : " + nowStage.ToString("g");
    }

    void Timecount()
    {
        time_int -= 1;

        time_UI.text = TextFormat();

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
            actionObjects[nowCharacterID].moveTarget += moveDelta;
            actionObjects[nowCharacterID].attackTarget = actionObjects[nowCharacterID].moveTarget + attackDelta;
        }
        nowStage = stage.Complete;
    }
}
