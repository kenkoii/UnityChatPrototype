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

	public void QuestionSelect ()
	{
		

		//call question callback here
		RPCWrapper.Instance.RPCWrapAnswer();
		StopTimer ();
	}

	IEnumerator StartTimer (int timeReceive)
	{
		hasAnswered = false;
		int timer = timeReceive;

		while (timer > 0 && hasAnswered == false) {
			questionSelect.transform.Find ("SelectTime").GetComponent<Text> ().text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}

		//choose a question if timeout
		if (hasAnswered == false) {
			QuestionSelect ();
		}


	}

	private void StopTimer ()
	{
		hasAnswered = true;
		questionSelect.SetActive (false);

		FirebaseDatabaseFacade.Instance.CheckAnswerPhase ();
		//CHECK FIREBASE FOR STATUS FOR NEXT PHASE

	}
}
