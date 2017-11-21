using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replaymove : MonoBehaviour
{


    public void Move(Vector2 end_position)
    {
        Vector2 now_position;
        Vector2 step;
        int count = 100;
        now_position.x = this.transform.position.x;
        now_position.y = this.transform.position.y;
        //end_position = this.GetComponent<Client>().nowCoordinate;
        step = (end_position-now_position) / 100;

        
        {
            now_position += step;
            count += 1;
        }
    }
}
