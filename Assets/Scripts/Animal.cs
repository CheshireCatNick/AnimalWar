using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal{

    public int characterID;
    public Vector2 position;
    public bool isSet;
    public string type;
    public GameObject animal;

    public Animal(int characterID, Vector2 position, bool isSet, string type, GameObject animal)
    {
        this.characterID = characterID;
        this.position = position;
        this.isSet = isSet;
        this.type = type;
        this.animal = animal;
    }
}
