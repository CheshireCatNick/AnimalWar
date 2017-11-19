using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	//風力大小
	public float WindForce = 2f;

	void Start () {
		//5秒後自動刪除
		Destroy(gameObject, 5f);
		//每隔1秒被風施力
		InvokeRepeating("AddWindForce", 0f, 1f);
	}

	void AddWindForce () {
		//隨機選方向施力
		if(Random.Range(0f, 1f) > 0.5f)
			GetComponent<Rigidbody2D>().AddForce(Vector2.right * WindForce);
		else
			GetComponent<Rigidbody2D>().AddForce(-Vector2.right * WindForce);
	}

	//觸碰到其他物體就刪除
	void OnCollisionEnter2D(Collision2D hit){
        if (hit.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
        {
            Destroy(gameObject);
        }
	}
}
