using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons{

    public float Damage;
    public float Range;
    public bool isSingle;
    public string name;
	public int attackRadius;

    public Weapons(string name)
    {
        this.name = name;
        if (name == "gun")
        {
            this.Damage = 100;
            this.Range = 4;
            this.isSingle = true;
            this.attackRadius = 6;
        }
        else if (name == "firegun")
        {
            this.Damage = 100;
            this.Range = 3;
            this.isSingle = false;
            this.attackRadius = 4;
        }
        else if (name == "bomb")
        {
            this.Damage = 100;
            this.Range = 1;
            this.isSingle = false;
            this.attackRadius = 3;
        }
    }
}
