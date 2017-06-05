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
		phase1.gameObject.SetActive (true);
		phase2.gameObject.SetActive (false);
		phase3.gameObject.SetActive (false);
	}

	public void StartPhase2(){
		phase1.gameObject.SetActive (false);
		phase2.gameObject.SetActive (true);
		phase3.gameObject.SetActive (false);
	}

	public void StartPhase3(){
		phase1.gameObject.SetActive (false);
		phase2.gameObject.SetActive (false);
		phase3.gameObject.SetActive (true);

	}


		

}
