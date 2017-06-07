using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class QuestionController : MonoBehaviour
{
	private GameObject selectLetterIcon;

	private Action<bool> onFinishQuestion;
	public static int getround;
	private static int correctAnswers;
	public static Boolean stoptimer = true;
	private static int timeLeft;
	private static int timeDuration;
	private GameObject timerObj;
	private static GameObject[] inputButton;
	private float timeLimit = 3;
	private int totalGP;
	private static int questionsTime;
	public static Action<int> onResult;
	// Use this for initialization
	public Action<int> OnResult {
		get { 
			return onResult;
		}
		set { 
			onResult = value;
		}

	}

	public int TimeLeft {
		get { 
			return timeLeft;
		}
		set { 
			timeLeft = value;

		}

	}

	public Boolean Stoptimer {
		get { 
			return stoptimer;
		}
		set { 
			stoptimer = value;
		}
	}

	public void SetQuestion (IQuestion questiontype, int qTime, Action<int> Result)
	{
		questiontype.Activate (selectLetterIcon, qTime, Result);
		questionsTime = qTime;
	}

	void Start ()
	{
		ResetTime ();
		timerObj = GameObject.Find ("Timer");
		StartCoroutine (StartTimer(true));
	}
		

	public void ResetTime ()
	{
		timeDuration = questionsTime;

	}
	public IEnumerator StartTimer(Boolean stoptimer){
		if (stoptimer) {

			while (timeLeft > 0) {
				timerObj.GetComponent<Text> ().text = "" + timeLeft;
				yield return new WaitForSeconds (1);
				timeLeft--;

			}
			stoptimer = false;
			ComputeScore ();
			for (int i = 0; i < 12; i++) {
				Destroy (GameObject.Find ("input" + i));
			}
			GameObject.Find ("QuestionModal").SetActive (false);
		} else {
			ComputeScore ();
			GameObject.Find ("QuestionModal").SetActive (false);
			for (int i = 0; i < 12; i++) {
				Destroy (GameObject.Find ("input" + i));
			}
		}
	}

	public void ComputeScore ()
	{
		stoptimer = false;
		SelectLetterIcon sli = new SelectLetterIcon ();
		sli.QuestionsDone.Clear ();
		onResult.Invoke (correctAnswers);
		correctAnswers = 0;
	}

	public void Returner (Action<bool> action, int round, int answerScore)
	{
		//callbackreturn
		onFinishQuestion = action;
		onFinishQuestion (true);
		getround = round;
		correctAnswers = answerScore;
		if (round > timeLimit) {
			stoptimer = false;
			ComputeScore ();

			GameObject.Find ("QuestionModal").SetActive (false);
		} 

	}
}
