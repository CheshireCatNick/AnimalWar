using UnityEngine;
using System.Collections;

public class FireGun : MonoBehaviour {

	public GameObject projcetile;
    public Transform firePoint;
    float speed = 10;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject shoot = (GameObject)Instantiate(projcetile, firePoint.position, firePoint.rotation);
            shoot.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(speed, 0));
            //Physics.IgnoreCollision (transform.root.GetComponent<Collider> (), shoot.GetComponent<Collider> ());
        }
    }
}
