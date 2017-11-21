using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal{

    public int characterID;
    public bool isSet;
    public string type;
    public GameObject animal;
    public bool isFinish;
    public ActionObject action;

    public Animal(int ID )
    {
        this.characterID = ID;
        this.isSet = false;
        this.type = "animal";
        this.animal = new GameObject();
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

    public void SetAnimal(GameObject animal)
    {
        this.animal = animal;
    }

    public void SetFinish(bool finish)
    {
        this.isFinish = finish;
    }

    public void SetAction(ActionObject action)
    {
        this.action = action;
    }
}
