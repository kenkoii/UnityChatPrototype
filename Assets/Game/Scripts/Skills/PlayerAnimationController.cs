using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationController : MonoBehaviour {

	private Animator anim;

	void Start(){
		anim = this.GetComponent<Animator> ();
	}

	public void Skill1Animate(){
		anim.SetBool ("Rest", true);
	}

	public void Skill2Animate(){

	}

	public void Skill3Animate(){

	}




}
