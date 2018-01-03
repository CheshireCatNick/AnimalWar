using UnityEngine;
using System.Collections;

public class FireGun : MonoBehaviour {

	public GameObject Cannonball;
    public Transform firePoint;
    public GameObject firegun;//砲管
    //float barrelSpeed = 30;
    float angle;
    float speed = 10;


    // Update is called once per frame
    public void Shoot(Vector2 target)
    {
		float x = (target.x - transform.position.x) * this.transform.parent.localScale.x;
        float y = target.y - transform.position.y;

        angle = Mathf.Atan2(y , x);//得到砲管角度		
        angle *= (180/Mathf.PI);
        //angle = Mathf.Clamp (angle, minAngle, maxAngle);
        Vector3 temp = firegun.transform.localEulerAngles;
        temp.z = angle;
        firegun.transform.localEulerAngles = temp;//上下旋轉砲管
        GameObject shoot = (GameObject)Instantiate(Cannonball, firePoint.position, firePoint.rotation);
		shoot.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(speed * (target.x * target.x + target.y * target.y) / 5, 0));
        Physics2D.IgnoreCollision (transform.root.GetComponent<Collider2D> (), shoot.GetComponent<Collider2D> ());
    }
}
