using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //使用Unity UI程式庫。

public class Client : MonoBehaviour
{

    private const int maxCharacterNum = 2, maxWeaponNum = 2, periodTime = 45;

    public int scale = 10;

    public int playerID;

    public GameObject player;

    public GameObject[] players = new GameObject[4];

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
        Complete
    };

    private stage nowStage = stage.Character;

    private ActionObject[] actionObjects = new ActionObject[maxCharacterNum];

    private void Start()
    {
        connectionManager = new ConnectionManager();
        playerID = int.Parse(connectionManager.Receive());
        Debug.Log("PlayerID: " + playerID);

        for (int i = 0; i < maxCharacterNum; i++)
        {
            actionObjects[i] = new ActionObject(i);
        }

        for (int i = 0; i < 2*maxCharacterNum; i++)
        {
            players[i] = Instantiate<GameObject>(player,new Vector3(-7.5f+(i*5),0.0f, 0.0f), player.transform.rotation);
            players[i].name = "player" + i.ToString();
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
                    nowCharacterID = i - KeyCode.Alpha0;
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
                actionObjects[nowCharacterID].attackTarget += attackDelta;
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

            Send();
        }
    }

    //receive actionArray from server
    public void Send()
    {
        string msg = playerID + "|";
        foreach (ActionObject action in actionObjects)
            msg += action.ToString() + "/";
        //return;
        connectionManager.Send(msg);
        string opponentActionStr = connectionManager.Receive();
        string[] opponentActions = opponentActionStr.Split('/');
        ActionObject[] replayActionObjects = new ActionObject[maxCharacterNum * 2];
        
        for (int i = 0; i < maxCharacterNum * 2; i++)
        {
            if (i < maxCharacterNum)
                replayActionObjects[i] = actionObjects[i];
            else
                replayActionObjects[i] = new ActionObject(opponentActions[i - maxCharacterNum]);
        }
        Replay(replayActionObjects);
    }
    //show the result
    public void Replay(ActionObject[] actionArray)
    { 
        foreach (ActionObject a in actionArray)
        {
            print(a.ToString());
        }
        
        for (int i = 0; i < maxCharacterNum; i++)
        {
            actionObjects[i].isSet = false;
        }

        nowStage = stage.Character;
        nowCharacterID = 0;
        nowWeapon = weapons[0];
        moveDelta = Vector2.zero;
        attackDelta = Vector2.zero;

        //set timer
        time_int = periodTime;
        InvokeRepeating("Timecount", 1, 1);
        time_UI.text = TextFormat();
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
            actionObjects[nowCharacterID].attackTarget += attackDelta;
        }
        Send();
    }
}
