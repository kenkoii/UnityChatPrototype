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

	public void OnEnable ()
	{
		//stoptimer = true;

		battleController = FindObjectOfType<BattleController> ();
		Attack ();
	}

	private void Attack ()
	{
		battleController.SendAttackToDatabase ();
	
			stoptimer = true;
			timeLeft = 3;
			InvokeRepeating("StartTimer",0,1);

			for (int i = 0; i < battleUI.Length; i++) {
				battleUI[i].SetActive (false);
			}

	}


	private void StartTimer(){
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 

			GameTimer.Instance.ToggleTimer (false);
			stoptimer = false;
		}
	}
		
}
