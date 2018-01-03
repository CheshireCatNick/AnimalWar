using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_ray : MonoBehaviour
{
    // Use this for initialization
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other != null && 
            (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player"))
        {
            Destroy(this.gameObject);
        }
    }
}