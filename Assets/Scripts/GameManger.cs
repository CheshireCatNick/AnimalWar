using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour {
    private Client client;
    // Use this for initialization
    void Start()
    {
        client = gameObject.AddComponent<Client>();
    }

	// Update is called once per frame
	void Update () {

    }
}
