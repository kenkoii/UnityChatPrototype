using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase1Controller : MonoBehaviour
{

	public GameObject questionSelect;
	private bool hasAnswered = false;
	private int questionCounter = 0;
	private Coroutine timerCoroutine;
	public int chooseQuestionFirstTime = 4;
	public int chooseQuestionSecondTime = 3;


	void OnEnable ()
	{
		timerCoroutine = StartCoroutine (StartTimer (chooseQuestionFirstTime));
		questionSelect.SetActive (true);
		
	}

	public void QuestionSelect ()
	{
		hasAnswered = true;
		StopTimer ();
	}

	IEnumerator StartTimer (int timeReceive)
	{
		int timer = timeReceive;

		while (timer > 0) {
			questionSelect.transform.Find ("SelectTime").GetComponent<Text> ().text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}

		if (hasAnswered == false && questionCounter <1) {
			questionCounter++;
			timerCoroutine = StartCoroutine (StartTimer (chooseQuestionSecondTime));
		} else {
			StopTimer ();
		}

	}

	private void StopTimer ()
	{
		questionCounter = 0;
		StopCoroutine (timerCoroutine);
		questionSelect.SetActive (false);

		//CHECK FIREBASE FOR STATUS FOR NEXT PHASE

	}
}
