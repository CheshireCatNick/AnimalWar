using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove: MonoBehaviour {

    Rigidbody2D playerRigidbody2D;

    //傳進Destination


    [Header("水平速度")]
    public float speedX;
    [Header("垂直速度")]
    public float speedY;
    [Header("終點座標")]
    public Vector2 Destinaiton;
    [Header("目前水平方向")]
    public float horizontalDir;//-1~1之間

    const string HORIZONTAL = "Horizontal" ;

    [Header("水平推力")]
    [Range(0,500)]
    public float xForce;

    [Header("最大水平速度")]
    public float maxSpeedX;

    public void ControlSpeed(){
        speedX =  playerRigidbody2D.velocity.x;
        speedY =  playerRigidbody2D.velocity.y;
        float newSpeedX = Mathf.Clamp(speedX, -maxSpeedX, maxSpeedX);
        playerRigidbody2D.velocity = new Vector2(newSpeedX, speedY);
    }

    


    /// <summary>水平移動</summary>
    void MovementX () {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        horizontalDir = Destinaiton.x - playerRigidbody2D.position.x;
        while ( playerRigidbody2D.position.x != Destinaiton.x ) {
            playerRigidbody2D.AddForce(new Vector2(xForce * horizontalDir, 0));
        }
    }

	// Update is called once per frame
	void Update () {
        MovementX();
        //目前速度
        speedX = player1Regidbody2D.velocity.x;
        ControlSpeed();

	}
}
