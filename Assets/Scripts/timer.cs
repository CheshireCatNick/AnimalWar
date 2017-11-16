using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //使用Unity UI程式庫。

public class timer : MonoBehaviour
{

    int time_int = 45;
    public Text time_UI;

    void Start()
    {
        time_UI.text = "Time : 45";
        InvokeRepeating("timecount", 1, 1);
    }

    void timecount()
    {

        time_int -= 1;

        time_UI.text = "Time : " + time_int + "";

        if (time_int == 0)
        {

            time_UI.text = "Time : 0";

            CancelInvoke("timecount");

        }

    }

}