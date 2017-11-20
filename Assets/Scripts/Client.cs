﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //使用Unity UI程式庫。

public class Client : MonoBehaviour
{

    private const int maxCharacterNum = 2, maxWeaponNum = 2;

    public int scale = 10;

    public int playerID;

    private Vector2 moveDelta, attackDelta;

    private int nowCharacterID = 0;
    private Weapons[] weapons = new Weapons[maxWeaponNum];
    private Weapons nowWeapon;

    private ConnectionManager connectionManager;

    int time_int = 45;
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

        weapons[0] = new Weapons("skip");
        weapons[1] = new Weapons("gun");

        time_UI.text = "Time : " + time_int + "";
        InvokeRepeating("Timecount", 1, 1);
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
                    moveDelta = Vector2.zero;
                }

                //user select target weapon
                else if (nowStage == stage.Weapon)
                {
                    if (i - KeyCode.Alpha0 < maxWeaponNum)
                    {
                        nowWeapon = weapons[i - KeyCode.Alpha0];
                        attackDelta = Vector2.zero;
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
            //Send();
        }
    }

    //receive actionArray from server
    public void Send()
    {
        string msg = playerID + "|";
        foreach (ActionObject action in actionObjects)
            msg += action.ToString() + "/";
        connectionManager.Send(msg);
        string opponentActionStr = connectionManager.Receive();
        string[] opponentActions = opponentActionStr.Split('/');
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
    public void Replay(ActionObject[] actionArray)
    {
        nowStage = stage.Character;
        time_UI.text = "Time : " + time_int + "";
        InvokeRepeating("Timecount", 1, 1);
        for (int i = 0; i < maxCharacterNum; i++)
        {
            actionObjects[i].isSet = false;
        }
    }

    private void OnDestroy()
    {
        connectionManager.Close();
    }

    void Timecount()
    {
        time_int -= 1;

        time_UI.text = "Time : " + time_int + "";

        if (time_int == 0)
        {

            time_UI.text = "Time : 0";

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