using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
	//public GameObject effect;//特效

	void OnCollisionEnter2D (Collision2D collision) {//碰撞發生時呼叫
		//碰撞後產生爆炸
			
		/*if(collision.gameObject.tag == "enemy" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "ground")
        {//當撞到的collider具有enemy tag時
			Instantiate (effect, transform.position, transform.rotation);
			Destroy(gameObject);//刪除砲彈
		}*/
        Destroy(gameObject); 
    }
}
