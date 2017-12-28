using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPControl : MonoBehaviour
{

    private float m_hp;
    private Text m_text;

    // Use this for initialization
    void Start()
    {
        m_text = GetComponent<Text>();
        m_hp = Health.Instance.GetHP();
    }

    // Update is called once per frame
    void Update()
    {
        m_text.text ="HP : "+ m_hp.ToString();
        m_hp = Health.Instance.GetHP();
    }
}