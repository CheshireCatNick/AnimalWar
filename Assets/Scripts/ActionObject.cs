using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObject {

    public int characterID;
    public Vector2 moveTarget, attackTarget;
    public Weapons weapon;
    public bool isSet;

    public ActionObject(int charaterID)
    {
        this.characterID = charaterID;
        this.isSet = false;
        this.weapon = new Weapons("skip");
        this.moveTarget = Vector2.zero;
        this.attackTarget = Vector2.zero;
    }

    public ActionObject(string data)
    {
        string[] properties = data.Split(',');
        this.characterID = int.Parse(properties[0]);
        this.moveTarget = new Vector2(-int.Parse(properties[1]), int.Parse(properties[2]));
        this.weapon = new Weapons(properties[3]);
        this.attackTarget = new Vector2(-int.Parse(properties[4]), int.Parse(properties[5]));
    }

    public override string ToString()
    {
        return string.Format("{0},{1},{2},{3},{4},{5}", 
            characterID, 
            moveTarget.x, 
            moveTarget.y, 
            weapon.name, 
            attackTarget.x, 
            attackTarget.y);
    }
}