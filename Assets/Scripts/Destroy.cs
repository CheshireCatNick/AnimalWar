using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {
    public GameObject me;
	// Use this for initialization
	void Update () {
        Destroy(me,2.0f);
    }
}
