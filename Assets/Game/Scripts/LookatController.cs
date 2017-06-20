using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatController : MonoBehaviour {

	public Transform lookObj;
	private float speed = 20;

	void FixedUpdate(){
		

//		Vector3 dir = lookObj.position - transform.position;
//		Quaternion rot = Quaternion.LookRotation(dir);
//		// slerp to the desired rotation over time
//		transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
	}
}
