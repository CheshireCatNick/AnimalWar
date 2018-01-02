using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackMove : MonoBehaviour {

	public GameObject barrel;//砲管
	float barrelSpeed = 30;
	//float maxAngle = 80;
	//float minAngle = -5;
	float angle;
	
	// Update is called once per frame
	void Update () {

		angle += Input.GetAxis ("Mouse ScrollWheel")*barrelSpeed;//得到砲管角度			
		//angle = Mathf.Clamp (angle, minAngle, maxAngle);
		Vector3 temp = barrel.transform.localEulerAngles;
		temp.z = angle;
		barrel.transform.localEulerAngles = temp;//上下旋轉砲管

	
	}
}
