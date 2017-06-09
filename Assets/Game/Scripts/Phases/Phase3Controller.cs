using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase3Controller : MonoBehaviour
{
	BattleController battleController;
	public GameObject[] battleUI;


	public void OnEnable ()
	{
		battleController = FindObjectOfType<BattleController> ();
		Attack ();
	}

	private void Attack ()
	{
		battleController.SendAttackToDatabase ();
		DoOnMainThread.ExecuteOnMainThread.Enqueue(() => { StartCoroutine (StartTimer (3)); } );
		DoOnMainThread.ExecuteOnMainThread.Enqueue(() => { 
			for (int i = 0; i < battleUI.Length; i++) {
				battleUI[i].SetActive (false);
			};} );

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
		
}
