using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookatController : MonoBehaviour {

	public Transform lookObj;

	void Update(){
		this.transform.LookAt (lookObj);
	}
}
