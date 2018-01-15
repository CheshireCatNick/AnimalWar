using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerControl_firegun : MonoBehaviour {


    private int r = 4;
    private int m_power;
	private Vector3 start_point;
    public TextMesh m_text;

    // Use this for initialization
    void Start()
    {
		start_point = this.transform.position;
        m_power = 0;
        m_text.text = m_power.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float power_x = this.transform.position.x - start_point.x;
        float power_y = this.transform.position.y - start_point.y;
        m_power = (int)(100 * (Mathf.Sqrt((power_x*power_x)+(power_y*power_y))/Mathf.Sqrt(r*r)));
        if(m_power >= 100)
            m_power = 100;
        m_text.text =m_power.ToString();
    }
}
