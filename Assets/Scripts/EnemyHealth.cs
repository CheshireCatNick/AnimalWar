using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
	public int numberOfLives;	//initial life

	public Image lifee;
	public float maxhp;
	private float hp;

    int currentLives;				
	bool alive = true;				//enemy state

	void Start () {	
		currentLives = numberOfLives;
		hp = maxhp;
	}	
	void Update () {
		lifee.transform.localPosition= new Vector3 (( -2 + 2*(hp/maxhp)), 0.0f, 0.0f);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag != "Sphere" || !alive)
			return;
		//the collision occur
		Destroy(other.gameObject);
        currentLives -= 1;
		hp -= 1;

		//the enemy run out of life
		if(currentLives <= 0)
		{
			Destroy(this.gameObject);
            //HPControl.Instance.SubHP(10);
        }
	}
}

