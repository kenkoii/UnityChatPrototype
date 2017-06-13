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
			InvokeRepeating("StartTimer2",0,1);

			for (int i = 0; i < battleUI.Length; i++) {
				battleUI[i].SetActive (false);
			}

	}

	IEnumerator StartTimer (int timeReceive)
	{
		int timer = timeReceive;

		while (timer > 0) {
			timer--;
			yield return new WaitForSeconds (1);
		}
//		FirebaseDatabaseFacade.Instance.CheckAttackPhase ();

	}
	private void StartTimer2(){
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;

			} else {
				GameTimer.Instance.ToggleTimer (false);
				stoptimer = false;
			}
		}
	}
		
}
