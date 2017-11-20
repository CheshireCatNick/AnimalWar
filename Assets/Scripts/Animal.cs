using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal{

    public int characterID;
    public Vector2 position;
    public bool isSet;
    public string type;

    public Animal(int characterID, Vector2 position, bool isSet, string type)
    {
        this.characterID = characterID;
        this.position = position;
        this.isSet = isSet;
        this.type = type;
    }
}
