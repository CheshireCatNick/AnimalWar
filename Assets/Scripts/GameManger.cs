using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour {
    private ConnectionManager connectionManager;

    // Use this for initialization
    void Start()
    {
        connectionManager = new ConnectionManager();
        connectionManager.Send("encrypt");
        Debug.Log(connectionManager.Receive());
        connectionManager.Close();

    }

	// Update is called once per frame
	void Update () {

    }
}
