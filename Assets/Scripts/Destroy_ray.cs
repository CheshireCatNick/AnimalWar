using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_ray : MonoBehaviour
{
    public GameObject ray;
    // Use this for initialization
    void Update()
    {
        Destroy(ray);
    }
}