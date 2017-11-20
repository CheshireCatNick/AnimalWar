using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObject{

    public int characterId;
    public Vector2 moveTarget, attackTarget;
    public string weapon;
    public bool isSet;

    public ActionObject(int charaterId)
    {
        this.characterId = charaterId;
        this.isSet = false;
        this.weapon = "skip";
        this.moveTarget = Vector2.zero;
        this.attackTarget = Vector2.zero;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
