using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons{

    public float Damage;
    public float Range;
    public bool isSingle;
    public string name;
	public Vector2 attackRange;

    public Weapons(float Damage, float Range, bool isSingle, string name, Vector2 attackRange)
    {
        this.Damage = Damage;
        this.Range = Range;
        this.isSingle = isSingle;
        this.name = name;
		this.attackRange = attackRange;
    }

    public Weapons(string name)
    {
        this.name = name;
    }
}
