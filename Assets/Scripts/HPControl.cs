using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPControl : MonoBehaviour
{

    private float m_hp;
    public Text m_text;

    // Use this for initialization
    void Start()
    {
        m_hp = this.GetComponent<Health>().GetHP();
    }

    // Update is called once per frame
    void Update()
    {
        m_hp = this.GetComponent<Health>().GetHP();
        m_text.text ="HP : "+ m_hp.ToString();
    }
}