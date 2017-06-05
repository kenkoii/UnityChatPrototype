using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase3Controller : MonoBehaviour
{
	BattleController battleController;

	void OnEnable ()
	{
		battleController = FindObjectOfType<BattleController> ();
		Attack ();
	}

	private void Attack ()
	{
		battleController.SendAttackToDatabase ();
		StartCoroutine (StartTimer (3));
	}

	IEnumerator StartTimer (int timeReceive)
	{
		int timer = timeReceive;

		while (timer > 0) {
			timer--;
			yield return new WaitForSeconds (1);
		}

		FirebaseDatabaseFacade.Instance.CheckAttackPhase ();
	}

}
