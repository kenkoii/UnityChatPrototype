using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class QuestionController : MonoBehaviour
{
	private GameObject selectLetterIcon;

	private bool Result;
	private Action<bool> OnFinishQuestion;
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
		resetTime ();
		timerObj = GameObject.Find ("Timer");
		StartCoroutine (StartTimer(true));
	}
		

	public void resetTime ()
	{
		timeDuration = questionsTime;

	}

//	public void Timer (Boolean timebool)
//	{
//		if (timebool) {
//			if (timeLeft < 1) {
//				//Destroy (GameObject.Find ("QuestionModal"));
//				//clear();
//				computeScore ();
//				stoptimer = false;
//				for (int i = 0; i < 12; i++) {
//					Destroy (GameObject.Find ("input" + i));
//				}
//				GameObject.Find ("QuestionModal").SetActive (false);
//
//
//			} else {
//				timeLeft -= Time.deltaTime;
//	
//				GameObject.Find ("Timer").GetComponent<Text> ().text = Math.Truncate (timeLeft).ToString ();
//
//			}
//		}
//	}

	public IEnumerator StartTimer(Boolean stoptimer){
		if (stoptimer) {

			while (timeLeft > 0) {
				timerObj.GetComponent<Text> ().text = "" + timeLeft;
				yield return new WaitForSeconds (1);
				timeLeft--;

			}
			stoptimer = false;
			computeScore ();
			for (int i = 0; i < 12; i++) {
				Destroy (GameObject.Find ("input" + i));
			}
			GameObject.Find ("QuestionModal").SetActive (false);
		} else {
			computeScore ();
			GameObject.Find ("QuestionModal").SetActive (false);
			for (int i = 0; i < 12; i++) {
				Destroy (GameObject.Find ("input" + i));
			}
		}
	}

	public void computeScore ()
	{
		stoptimer = false;
		SelectLetterIcon sli = new SelectLetterIcon ();
		sli.QuestionsDone.Clear ();
		onResult.Invoke (correctAnswers);
		GameObject TextArea = Resources.Load ("Prefabs/TextArea") as GameObject;
		GameObject TotalGP = Instantiate (TextArea) as GameObject;
		GameObject canvas = GameObject.Find ("Canvas");
		TotalGP.transform.position = new Vector2 (0, 180);
		TotalGP.transform.SetParent (canvas.transform, false);
		TotalGP.transform.localScale = new Vector3 (1, 1, 1);
		TotalGP.name = "TextArea";

		TotalGP.transform.GetChild (0).GetComponent<Text> ().text = 
			"Earned " + "" + correctAnswers.ToString () + "GP!";
		if (timeLeft > (timeDuration * 0.3)) {
			TotalGP.transform.GetChild (1).GetComponent<Text> ().text = timeLeft + "s - Time Bonus!";
		} else {
			TotalGP.transform.GetChild (1).GetComponent<Text> ().text = "No Time Bonus!";
		}
		/*
		GameObject debugger = Resources.Load ("Prefabs/DebugMode") as GameObject;
		GameObject spawnDebugger = Instantiate (debugger) as GameObject;
		spawnDebugger.transform.position = new Vector2 (0, -180f);
		spawnDebugger.transform.SetParent (canvas.transform, false);
		spawnDebugger.transform.localScale = new Vector3 (1, 1, 1);
		spawnDebugger.name = "DebugMode";
		spawnDebugger.transform.GetChild (0).GetComponent<Button> ().onClick.AddListener (() => {
			GameObject.Find ("SelectLetterIcon").GetComponent<SelectLetterEvent> ().DebugOnClick ();
		});*/
		correctAnswers = 0;
	}

	public void Returner (Action<bool> action, int round, int answerScore)
	{
		//callbackreturn
		OnFinishQuestion = action;
		OnFinishQuestion (true);
		getround = round;
		correctAnswers = answerScore;
		if (round > timeLimit) {
			stoptimer = false;
			computeScore ();

			GameObject.Find ("QuestionModal").SetActive (false);
		} 

	}
}
