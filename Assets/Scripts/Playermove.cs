using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove: MonoBehaviour {

    Rigidbody2D playerRigidbody2D;

    public Vector2 Destination;

    public bool isArrive;

    const string HORIZONTAL = "Horizontal";

    void Start()
    {
        StartCoroutine(Wait());
        
        isArrive = false;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        Destination = playerRigidbody2D.transform.position;
    }

    public void Move()  {
        //獲得當前位置
        if (playerRigidbody2D != null) {
            Vector2 currentPosition = playerRigidbody2D.transform.position;

            if (Destination != currentPosition) {
                float speed = 5;

                if (Vector2.Distance(currentPosition, Destination) < 0.01f)
                {
                    transform.position = Destination;
                }
                else
                {
                    //插值移動
                    //距離就等於 間隔時間乘以速度
                    float maxDistanceDelta = Time.deltaTime * speed;
                    transform.position = Vector2.MoveTowards(currentPosition, Destination, maxDistanceDelta);
                }
            }
        }
    }
    
	// Update is called once per frame
	void Update () {
        Move();
	}
}
