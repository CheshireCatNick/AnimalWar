using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	public int numberOfLives;	//設定血有多少
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
        if (currentLives <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public float GetHP()
    {
        return currentLives;
    }
	void OnCollisionEnter2D (Collision2D other)
	{
		//當敵人碰到塔就受傷
		print ("victim : " + this.gameObject.name + "name : " + other.gameObject.name + "tag : " + other.gameObject.tag);
		if ((other.gameObject.tag != "Bomb" && other.gameObject.tag != "Cannonball") || !alive) {
			return;
		}
        if (other.gameObject.tag == "Bomb") 
		    currentLives -= 40;
        else if(other.gameObject.tag == "Cannonball")
            currentLives -= 30;

        print (currentLives);
        //如果沒血了
        if (currentLives <= 0)
		{
			Destroy(this.gameObject);
		}
	}

	public void DecreaseHealth (int damage) {
		currentLives -= damage;
		//如果沒血了
		if (currentLives <= 0)
		{
			Destroy(this.gameObject);
		}
	}
}
