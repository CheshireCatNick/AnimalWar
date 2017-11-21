using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove: MonoBehaviour {

    Rigidbody2D playerRigidbody2D;

    public Vector2 Destination;

    Vector2 INITDESTINATION = new Vector2(1000,1000);
    const string HORIZONTAL = "Horizontal";

    public void Move()  {
        //獲得當前位置
        Vector2 curenPosition = playerRigidbody2D.transform.position;
        
        if (Destination != INITDESTINATION) {
            float speed = 150;

            if (Vector2.Distance(curenPosition, Destination) < 0.01f)
            {
                transform.position = Destination;
            }
            else
            {
                //插值移動
                //距離就等於 間隔時間乘以速度
                float maxDistanceDelta = Time.deltaTime * speed;
                transform.position = Vector2.MoveTowards(curenPosition, Destination, maxDistanceDelta);
            }
        }
    }


	// Update is called once per frame
	void Update () {
        Move();
        
	}
}
