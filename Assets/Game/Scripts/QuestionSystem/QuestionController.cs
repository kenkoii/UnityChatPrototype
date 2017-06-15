﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class QuestionController : MonoBehaviour
{
	private GameObject selectLetterIcon;
	private GameObject typingIcon;
	private GameObject changeOrderIcon;
	private GameObject wordChoiceIcon;
	public static int getround;
	private static int correctAnswers;
	private static bool stoptimer = false;
	private static int timeLeft;
	private static int timeDuration;
	private static GameObject[] inputButton;
	private float roundlimit = 3;
	private int totalGP;
	private static string questionType = "";
	private static int questionsTime;
	public static Action<int,int> onResult;
	// Use this for initialization
	public bool onFinishQuestion {
		get;
		set;
	}

	public Action<int,int> OnResult {
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

	public bool Stoptimer {
		get { 
			return stoptimer;
		}
		set { 
			stoptimer = value;
		}
	}

	void OnEnable ()
	{
		InvokeRepeating ("StartTimer", 0, 1);
	}

	public void SetQuestion (IQuestion questiontype, int qTime, Action<int, int> Result)
	{
		GameObject entity = selectLetterIcon;
		string entityChosen = questiontype.GetType ().ToString ();
		string modalName = "";
		switch (entityChosen) {
		case "TypingIcon":
			entity = typingIcon;
			modalName = "TypingModal";
			break;
		case "SelectLetterIcon":
			entity = selectLetterIcon;
			modalName = "SelectLetterIconModal";
			break;
		case "ChangeOrderIcon":
			entity = changeOrderIcon;
			modalName = "ChangeOrderModal";
			break;
		case "WordChoiceIcon":
			entity = wordChoiceIcon;
			modalName = "WordChoiceIcon";
			break;
		}
		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("input" + i));
			Destroy (GameObject.Find ("output" + i));
			if (i < 3) {
				GameObject.Find ("Indicator" + (i + 1)).GetComponent<Image> ().color = Color.gray;
			}
		}
		timeLeft = qTime;
		questionType = modalName;
		questiontype.Activate (entity, qTime, Result);
		stoptimer = true;
	}


	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
				
			GameTimer.Instance.ToggleTimer (false);
			stoptimer = false;
			ComputeScore ();
				  
		}
	}

	public void ComputeScore ()
	{
		QuestionManager.Instance.QuestionHide ();
		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("input" + i));
		}
		for (int i = 0; i < 12; i++) {
			Destroy (GameObject.Find ("output" + i));
		}
		onResult.Invoke (correctAnswers,timeLeft);
		correctAnswers = 0;

	}


	public void Returner (Action<bool> action, int round, int answerScore)
	{
		action (onFinishQuestion);
		getround = round;
		correctAnswers = answerScore;
		if (round > roundlimit) {
			stoptimer = false;
			ComputeScore ();

		} 

	}
}
