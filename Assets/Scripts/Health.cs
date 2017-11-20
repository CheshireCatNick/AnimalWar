using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	public int numberOfLives;	//設定塔的血有多少
    public static Health Instance;
	public Image life;
	public float maxhp;

	int currentLives;				//目前血量
	bool alive = true;              //生或死
    void Awake()
    {
        Instance = this;
    }
    void Start () {	
		currentLives = numberOfLives;
	}	
	void Update () {
		life.transform.localPosition= new Vector3 (( -2 + 2*(currentLives/maxhp)), 0.0f, 0.0f);
	}
    public float GetHP()
    {
        return currentLives;
    }
	void OnCollisionEnter (Collision other)
	{
		//當敵人碰到塔就受傷
		if ((other.gameObject.tag != "Sphere" && other.gameObject.tag != "enemy") || !alive)
			return;
		
		currentLives -= 1;
        //HPControl.Instance.SubHP(1);

        //如果沒血了
        if (currentLives <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
