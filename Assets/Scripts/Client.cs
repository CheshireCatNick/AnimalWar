using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour {

    private const int maxCharacterNum = 2;
    public int scale = 10;

    public int playerID;

    private Vector2 moveDelta, attackDelta;
    private int nowCharacterID = 0, nowWeapon;

    private ConnectionManager connectionManager;
    
    //Animal Object need a flag to show if it is completed  
    private enum stage
    {
        Character,
        Move,
        Weapon,
        Attack,
        Complete
    };

<<<<<<< HEAD
    private stage nowStage = stage.Character;

    private ActionObject [] actionObjects;
=======
    private stage nowStage = stage.Complete;

    private ActionObject [] actionObjects = new ActionObject[maxCharacterNum];
>>>>>>> 8c59259d690dba4c5ebf257c4815cd744a32a661

    private void Start()
    {
        connectionManager = new ConnectionManager();
        playerID = int.Parse(connectionManager.Receive());
<<<<<<< HEAD
        Debug.Log(playerID);
        for (int i = 0; i < 2*maxCharacterNum; i++)
=======
        Debug.Log("PlayerID: " + playerID);
        for (int i = 0; i < maxCharacterNum; i++)
>>>>>>> 8c59259d690dba4c5ebf257c4815cd744a32a661
        {
            actionObjects[i] = new ActionObject(i);
        }
    }

    private void Update () {
        //check Timeout, if true, set nowStaget to Complete and Replay
        for (KeyCode i = KeyCode.Alpha0; i < KeyCode.Alpha0 + maxCharacterNum; i++) {
            if (Input.GetKeyDown(i))
            {
                //use select target Character
                if (nowStage == stage.Character)
                {
                    nowCharacterID = i - KeyCode.Alpha0;
                    moveDelta = Vector2.zero;
                }
                
                //user select target weapon
                else if (nowStage == stage.Weapon)
                {
                    nowWeapon = i - KeyCode.Alpha0;
                    attackDelta = Vector2.zero;
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
        
        //get mouse left click
        if (Input.GetMouseButtonDown(0))
        {
            print(Input.mousePosition);
            if (nowStage == stage.Attack)
            {
                moveDelta = Input.mousePosition;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (nowStage == stage.Character)
            {
                actionObjects[nowCharacterID].characterID = nowCharacterID;
                nowStage = stage.Move;
            }

            if (nowStage == stage.Move)
            {
                actionObjects[nowCharacterID].moveTarget += moveDelta;
                nowStage = stage.Weapon;
            }

            if (nowStage == stage.Weapon)
            {
                actionObjects[nowCharacterID].weapon = "gun";
                nowStage = stage.Attack;
            }

            if (nowStage == stage.Attack)
            {
                actionObjects[nowCharacterID].attackTarget += attackDelta;
                actionObjects[nowCharacterID].isSet = true;
                //if it is the final animal then go to Complete, else go to Character
                bool flag = false;
                foreach (ActionObject actionObject in actionObjects)
                {
                    if (actionObject.isSet == false)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                    nowStage = stage.Character;
                else
                    nowStage = stage.Complete;
            }
        }
        if (nowStage == stage.Complete)
        {
            Send();
        }
    }

    //receive actionArray from server
    public void Send()
    {
        string msg = "";
        foreach (ActionObject action in actionObjects)
            msg += action.ToString() + "/";
        //connectionManager.Send(msg);
        //string opponentActionStr = connectionManager.Receive();
        string[] opponentActions = msg.Split('/');
        ActionObject[] replayActionObjects = new ActionObject[maxCharacterNum * 2];
        int i = 0;
        for (; i < maxCharacterNum * 2; i++)
        {
            if (i < maxCharacterNum)
                replayActionObjects[i] = actionObjects[i];
            else
                replayActionObjects[i] = new ActionObject(opponentActions[i - maxCharacterNum]);
        }
        Replay(replayActionObjects);
    }

    //show the result
    public void Replay (ActionObject [] actionArray)
    {
        nowStage = stage.Character;
    }

    private void OnDestroy()
    {
        connectionManager.Close();
    }

<<<<<<< HEAD
        // receive
        //Replay();
    }

    //show the result
    public void Replay (ActionObject [] actionArray)
    {



        nowStage = stage.Character;
    }


=======
>>>>>>> 8c59259d690dba4c5ebf257c4815cd744a32a661
}
