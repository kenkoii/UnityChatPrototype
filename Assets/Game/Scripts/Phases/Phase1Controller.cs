using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase1Controller : MonoBehaviour
{

	public GameObject questionSelect;
	private bool hasAnswered = false;
	private int questionCounter = 0;
	private int answerCounter = 0;
	public int chooseQuestionFirstTime = 4;
	public int chooseQuestionSecondTime = 3;
	public int chooseAnswerTime = 15;


	void OnEnable ()
	{
		StartCoroutine (StartTimer (chooseQuestionFirstTime));
		questionSelect.SetActive (true);
		
	}

	public void QuestionSelect ()
	{
		questionCounter = 1;
		StartCoroutine (StartTimer (chooseAnswerTime));
	}

	IEnumerator StartTimer (int timeReceive)
	{
		int timer = timeReceive;

		while (timer > 0) {
			questionSelect.transform.Find ("SelectTime").GetComponent<Text> ().text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}

		if (questionCounter < 1) {
			questionCounter++;
			StartCoroutine (StartTimer (chooseQuestionSecondTime));
		} else {
			
			if (answerCounter < 1) {
				answerCounter++;
				StartCoroutine (StartTimer (chooseAnswerTime));
				//CALL QUESTION CLASS WITH CALLBACK HERE if answercounter >1 stoptimer

				//StopTimer();
			} else {
				StopTimer ();

			}
		}

	}

	private void StopTimer ()
	{
		questionCounter = 0;
		answerCounter = 0;
		questionSelect.SetActive (false);

		FirebaseDatabaseFacade.Instance.CheckAnswerPhase ();
		//CHECK FIREBASE FOR STATUS FOR NEXT PHASE

	}
}
