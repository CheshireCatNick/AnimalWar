using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_ray : MonoBehaviour
{
    public GameObject ray;
    // Use this for initialization
    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(ray);
    }
}