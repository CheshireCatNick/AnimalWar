using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal{
    // animal general config
    public enum AnimalType
    {
        Fox,
        Frog,
        Eagle
    };
    public struct Ability
    {
        public Vector2 moveLimit;

        public Ability(Vector2 ml)
        {
            moveLimit = ml;
        }
    };
    public Ability[] animalAbilities = {
        new Ability(new Vector2(0.5f, 0.5f)),
        new Ability(new Vector2(0, 0)),
        new Ability(new Vector2(0, 0))
    };

    public int characterID;
    public bool isSet;
    public AnimalType type;
    public GameObject player;
    public ActionObject action;
    public Ability ability;
    

    public Animal(int ID, Vector3 scale, GameObject prefab)
    {
        this.characterID = ID;
        this.isSet = false;
        this.type = AnimalType.Fox;
        this.ability = animalAbilities[(int)this.type];
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

    public void SetType(AnimalType type)
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

    // return limited move range
    public bool CanMove(Vector2 moveDelta)
    {
        /*
        // check ability move limit
        if (moveDelta.x > this.ability.moveLimit.x ||
            moveDelta.y > this.ability.moveLimit.y)
            return false;*/
        // check boundary
        Vector3 pos = this.player.transform.localPosition + new Vector3(moveDelta[0], moveDelta[1], 0);
        Vector2 leftBoundary = new Vector2(-0.4f, 4.2f);
        Vector2 rightBoundary = new Vector2(42.0f, 4.2f);
        if (pos.x < leftBoundary.x || pos.x > rightBoundary.x ||
            pos.y > leftBoundary.y || pos.y > rightBoundary.y)
            return false;
        return true;
    }

	public void Move(Vector2 moveTarget, Vector2 attackTarget)
	{
		this.player.GetComponent<Playermove> ().Destination = moveTarget;
		this.player.GetComponent<Playermove> ().target = attackTarget;
		this.player.GetComponent<Playermove> ().isStart = true;
	}
}
