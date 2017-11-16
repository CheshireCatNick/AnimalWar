using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPControl : MonoBehaviour
{

    private float m_hp;
    private Text m_text;
    public static HPControl Instance;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        m_text = GetComponent<Text>();
        m_hp = Health.Instance.GetHP();
    }

    // Update is called once per frame
    void Update()
    {
        m_text.text ="HP : "+ m_hp.ToString();
    }

    public void SubHP(int point)
    {
        m_hp -= point;
    }
}