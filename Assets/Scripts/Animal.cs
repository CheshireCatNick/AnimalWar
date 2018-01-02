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
        // Fox
        new Ability(new Vector2(5f, 0.005f)),
        // Frog
        new Ability(new Vector2(5f, 0.005f)),
        // Eagle
        new Ability(new Vector2(5f, 10f)),
    };

    public int characterID;
    public bool isSet;
    public AnimalType type;
    public GameObject player;
    public ActionObject action;
    public Ability ability;

	public Animal(int ID, Vector3 scale, GameObject prefab, string type)
    {
        this.characterID = ID;
        this.isSet = false;
		if (type == "fox")
			this.type = AnimalType.Fox;
		else if (type == "eagle")
			this.type = AnimalType.Eagle;
		else if (type == "frog")
			this.type = AnimalType.Frog;
		this.ability = animalAbilities[(int)this.type];
        this.action = new ActionObject(this.characterID);

		this.player = (GameObject)GameObject.Instantiate(prefab, new Vector3(12f-ID*4.8f,-0.5f,0.0f), prefab.transform.rotation);
		this.player.transform.GetChild (3).gameObject.SetActive (false);
		this.player.transform.GetChild (4).gameObject.SetActive (false);
		this.player.transform.GetChild (5).gameObject.SetActive (false);
        bool flip = false;
        if (this.player.transform.localScale != scale)
            flip = true;

        this.player.transform.localScale = scale;
		this.player.name = "player" + ID.ToString();
		this.player.gameObject.layer = 10 + ID;
        if (flip)
        {
            Vector3 childscale = this.player.transform.GetChild(2).transform.localScale;
            childscale.x *= -1;
            this.player.transform.GetChild(2).transform.localScale = childscale;
        }
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
    public bool CanMove(Vector2 shadowPos)
    {
        // check ability move limit
        Vector2 pos = this.player.transform.localPosition;
        Vector2 moveDelta = shadowPos - pos;
		if (Mathf.Abs (moveDelta.x) > this.ability.moveLimit.x ||
		          Mathf.Abs (moveDelta.y) > this.ability.moveLimit.y)
			return false;
		// check boundary
        Vector2 leftBoundary = new Vector2(-12f, 4.2f);
        Vector2 rightBoundary = new Vector2(12f, 4.2f);
        if (shadowPos.x < leftBoundary.x || shadowPos.x > rightBoundary.x ||
            shadowPos.y > leftBoundary.y || shadowPos.y > rightBoundary.y)
            return false;
        return true;
    }

    public bool CanMoveTarget(Vector2 shadowPos, Vector2 targetPos, Weapons weapon)
    {
        // check ability move target limit
        Vector2 moveDelta = targetPos - shadowPos;
        Debug.Log("move" + moveDelta.x * moveDelta.x + moveDelta.y * moveDelta.y);
        Debug.Log("ww" + weapon.attackRadius * weapon.attackRadius);
        if (moveDelta.x * moveDelta.x + moveDelta.y * moveDelta.y
            > weapon.attackRadius * weapon.attackRadius)
            return false;
        // check boundary
        Vector2 leftBoundary = new Vector2(-12f, 4.2f);
        Vector2 rightBoundary = new Vector2(12f, 4.2f);
        if (targetPos.x < leftBoundary.x || targetPos.x > rightBoundary.x ||
            targetPos.y > leftBoundary.y || targetPos.y > rightBoundary.y)
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
