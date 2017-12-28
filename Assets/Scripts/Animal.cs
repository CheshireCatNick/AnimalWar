using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal{

    public int characterID;
    public bool isSet;
    public string type;
    public GameObject player;
    public ActionObject action;

    public Animal(int ID )
    {
        this.characterID = ID;
        this.isSet = false;
        this.type = "animal";
        //this.animal = new GameObject();
        this.isFinish = false;
        this.action = new ActionObject(this.characterID);
    }

    public void SetisSet(bool set)
    {
        this.isSet = set;
    }

    public void SetType(string type)
    {
        this.type = type;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void SetAction(ActionObject action)
    {
        this.action = action;
    }

	public void SetFinish(bool b)
	{
		this.player.GetComponent<Playermove> ().isFinish = b;
	}

	public bool IsFinish()
	{
		return this.player.GetComponent<Playermove> ().isFinish;
	}

	public void Move(Vector2 moveTarget, Vector2 attackTarget)
	{
		this.player.GetComponent<Playermove> ().Destination = moveTarget;
		this.player.GetComponent<Playermove> ().target = attackTarget;
		this.player.GetComponent<Playermove> ().isStart = true;
	}
}
