using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase3Controller : MonoBehaviour
{
	private BattleController battleController;
	public GameObject[] battleUI;
	private bool stoptimer = false;
	private int timeLeft;

	void OnEnable ()
	{
		Debug.Log ("phase3 started");
		battleController = FindObjectOfType<BattleController> ();
		stoptimer = true;
		timeLeft = 10;
		InvokeRepeating ("StartTimer", 0, 1);

		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (false);
		}
		battleController.SendAttackToDatabase ();
		Debug.Log ("phase3 started...");
	}



	void OnDisable(){
		CancelInvoke ("StartTimer");

	}

	private void StartTimer ()
	{
		if (stoptimer) {
			if (timeLeft > 0) {
				timeLeft--;
				return;
			} 

			FirebaseDatabaseFacade.Instance.CheckAttackPhase();

			stoptimer = false;

		}
	}
		
}
