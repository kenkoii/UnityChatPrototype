using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase3Controller : MonoBehaviour
{
	BattleController battleController;
	public GameObject[] battleUI;
	private static bool stoptimer = false;
	private static int timeLeft;

	void OnEnable ()
	{

		stoptimer = true;
		timeLeft = 10;
		InvokeRepeating ("StartTimer", 0, 1);
		battleController = FindObjectOfType<BattleController> ();
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (false);
		}
		battleController.SendAttackToDatabase ();
	}



	void OnDisable(){
		stoptimer = false;
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 

			FirebaseDatabaseFacade.Instance.CheckAttackPhase();

			GameTimer.Instance.ToggleTimer (false);
			stoptimer = false;
		}
	}
		
}
