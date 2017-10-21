using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour {

    private const int maxCharacterNum = 2;
    
    private int nowCharacterID = 0, nowWeapon;

    private ConnectionManager connectionManager = new ConnectionManager();
    
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

    private Vector2 nowCoordinate = new Vector2(0, 0);

    private ActionObject [] actionObject;

    private void Start()
    {
        for (int i = 0; i < 2*maxCharacterNum; i++)
        {
            actionObject[i] = new ActionObject();
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
                }
                
                //user select target weapon
                else if (nowStage == stage.Weapon)
                {
                    nowWeapon = i - KeyCode.Alpha0;
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
                nowCoordinate += Vector2.up;
            }

            //weapon target
            if (nowStage == stage.Attack)
            {
                nowCoordinate += Vector2.up;
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            print("DownArrow");
            //Character move
            if (nowStage == stage.Move)
            {
                nowCoordinate += Vector2.down;
            }

            //weapon target
            if (nowStage == stage.Attack)
            {
                nowCoordinate += Vector2.down;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            print("LeftArrow");
            //Character move
            if (nowStage == stage.Move)
            {
                nowCoordinate += Vector2.left;
            }

            //weapon target
            if (nowStage == stage.Attack)
            {
                nowCoordinate += Vector2.left;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            print("RightArrow");
            //Character move
            if (nowStage == stage.Move)
            {
                nowCoordinate += Vector2.right;
            }

            //weapon target
            if (nowStage == stage.Attack)
            {
                nowCoordinate += Vector2.right;
            }
        }
        
        //get mouse left click
        if (Input.GetMouseButtonDown(0))
        {
            print(Input.mousePosition);
            if (nowStage == stage.Attack)
            {
                nowCoordinate = Input.mousePosition;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (nowStage == stage.Character)
            {
                actionObject[nowCharacterID].characterId = nowCharacterID;
                nowStage = stage.Move;
            }

            if (nowStage == stage.Move)
            {
                actionObject[nowCharacterID].moveTarget = nowCoordinate;
                nowStage = stage.Weapon;
            }

            if (nowStage == stage.Weapon)
            {
                actionObject[nowCharacterID].weapon = "gun";
                nowStage = stage.Attack;
            }

            if (nowStage == stage.Attack)
            {
                actionObject[nowCharacterID].attackTarget = nowCoordinate;
                //if it is the final naimal then go to Complete, else go to Character
                nowStage = stage.Complete;
            }
        }
    }

    //set attack target
    public void SelectCell (Vector2 coordinate) { }

    //receive actionArray from server
    public void Send () { }

    //show the result
    public void Replay (ActionObject [] actionArray)
    {
        nowStage = stage.Character;
    }


}
