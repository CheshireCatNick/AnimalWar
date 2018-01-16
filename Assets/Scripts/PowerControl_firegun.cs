using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerControl_firegun : MonoBehaviour {


    private int r = 4;
    private int m_power;
    public Vector3 start_point;
    public Vector3 end_point;
    private float power_x;
    private float power_y;
    public TextMesh m_text;

    void Update()
    {
        power_x = end_point.x - start_point.x;
        power_y = end_point.y - start_point.y;
        m_power = (int)(100 * (Mathf.Sqrt((power_x * power_x) + (power_y * power_y)) / Mathf.Sqrt(r * r)));
        if (m_power >= 100)
            m_power = 100;
        m_text.text = m_power.ToString();
    }
}
