using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal{

    public int characterID;
    public bool isSet;
    public string type;
    public GameObject player;
    public ActionObject action;

	public Animal(int ID, Vector3 scale, GameObject prefab)
    {
        this.characterID = ID;
        this.isSet = false;
        this.type = "animal";
        //this.animal = new GameObject();
        this.action = new ActionObject(this.characterID);

		this.player = (GameObject)GameObject.Instantiate(prefab, new Vector3(7.5f-ID*5,-0.5f,0.0f), prefab.transform.rotation);
		this.player.transform.localScale = scale;
		this.player.name = "player" + ID.ToString();
		this.player.gameObject.layer = 10 + ID;
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
