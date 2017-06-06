using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase3Controller : MonoBehaviour, IPhase
{
	BattleController battleController;

	void Start(){
		battleController = FindObjectOfType<BattleController> ();
	}

	public void StartPhase ()
	{
		
		Attack ();
	}

	private void Attack ()
	{
		battleController.SendAttackToDatabase ();
		DoOnMainThread.ExecuteOnMainThread.Enqueue(() => { StartCoroutine (StartTimer (3)); } );

	}

	IEnumerator StartTimer (int timeReceive)
	{
		int timer = timeReceive;

		while (timer > 0) {
			timer--;
			yield return new WaitForSeconds (1);
		}
		FirebaseDatabaseFacade.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0);
		PhaseManager.Instance.StartPhase1 ();

	}
		
}
