using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replaymove : MonoBehaviour
{

    public Vector2 now_position;
    public Vector2 end_position;
    private Vector2 step;
    private int count;

    // Use this for initialization
    void Start()
    {
        now_position.x = this.transform.position.x;
        now_position.y = this.transform.position.y;
        end_position = this.GetComponent<Client>().nowCoordinate;
        step = (end_position-now_position) / 100;
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (count <= 100)
        {
            now_position += step;
            count += 1;
        }
    }
}
