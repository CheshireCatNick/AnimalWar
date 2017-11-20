using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons{

    public float Damage;
    public float Range;
    public bool isSingle;
    public string name;

    public Weapons(float Damage, float Range, bool isSingle, string name)
    {
        this.Damage = Damage;
        this.Range = Range;
        this.isSingle = isSingle;
        this.name = name;
    }

    public Weapons(string name)
    {
        this.name = name;
    }
}
