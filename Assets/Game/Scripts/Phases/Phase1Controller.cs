using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase1Controller : MonoBehaviour, IPhase
{

	public GameObject questionSelect;
	private bool hasAnswered = false;



	public void StartPhase ()
	{
		
		DoOnMainThread.ExecuteOnMainThread.Enqueue(() => { StartCoroutine (StartTimer (5)); } );

		questionSelect.SetActive (true);
	
	}

	void OnDisable(){
		questionSelect.SetActive (false);
	}

	public void QuestionSelect ()
	{
		//call question callback here
		RPCWrapper.Instance.RPCWrapAnswer();

	}

	IEnumerator StartTimer (int timeReceive)
	{
		hasAnswered = false;
		int timer = timeReceive;
		GameTimer.Instance.ToggleTimer (true);

		while (timer > 0 && hasAnswered == false) {
			GameTimer.Instance.gameTimerText.text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}

		hasAnswered = true;
		questionSelect.SetActive (false);
		GameTimer.Instance.ToggleTimer (false);
		FirebaseDatabaseFacade.Instance.CheckAnswerPhase ();

	}

}
