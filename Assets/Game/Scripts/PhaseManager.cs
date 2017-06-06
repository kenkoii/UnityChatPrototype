using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PhaseManager : SingletonMonoBehaviour<PhaseManager>
{
	public Phase1Controller phase1;
	public Phase2Controller phase2;
	public Phase3Controller phase3;

	public void StartPhase1(){
		Debug.Log ("Starting phase 1");
		phase1.StartPhase ();
	}

	public void StartPhase2(){
		Debug.Log ("Starting phase 2");
		phase2.StartPhase ();
	}

	public void StartPhase3(){
		Debug.Log ("Starting phase 3");
		phase3.StartPhase ();

	}

	public void StopAll(){
		Debug.Log ("Stopped phases");
		phase1.gameObject.SetActive (false);
		phase2.gameObject.SetActive (false);
		phase3.gameObject.SetActive (false);
	}






		

}
