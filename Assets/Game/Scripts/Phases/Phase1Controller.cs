using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Phase1Controller : MonoBehaviour
{

	public GameObject questionSelect;
	public GameObject[] battleUI;
	private bool hasAnswered = false;
	BattleController battleController;
	private static bool stoptimer = false;
	private static int timeLeft;

	public void OnEnable ()
	{
		battleController = FindObjectOfType<BattleController> ();
		hasAnswered = false;
		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (false);
		}
		questionSelect.SetActive (true);

	}

	void OnDisable ()
	{
		if (questionSelect.activeInHierarchy) {
			questionSelect.SetActive (false);
		}
	}

	public void OnQuestionSelect ()
	{
		stoptimer = false;
		string questionSelectedName = EventSystem.current.currentSelectedGameObject.name;
		int questionNumber = Int32.Parse (questionSelectedName [questionSelectedName.Length - 1].ToString ()) - 1;	
		hasAnswered = true;
		questionSelect.SetActive (false);
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (questionNumber, 20, delegate(int gp) {
			RPCWrapper.Instance.RPCWrapAnswer ();
			battleController.SetPlayerGP (gp);
			HideUI ();
		});

	}
	/*
	public void QuestionSelect1 ()
	{
		hasAnswered = true;
	
		questionSelect.SetActive (false);
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (0, 40, delegate(int gp) {
			RPCWrapper.Instance.RPCWrapAnswer();
			battleController.SetPlayerGP(gp);
			NextPhase();
		});


	}

	public void QuestionSelect2 ()
	{
		hasAnswered = true;
	
		questionSelect.SetActive (false);
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (1, 40, delegate(int gp) {
			RPCWrapper.Instance.RPCWrapAnswer();
			battleController.SetPlayerGP(gp);
			NextPhase();
		});

	}
	public void QuestionSelect3 ()
	{

	}

	public void QuestionSelect4 ()
	{

	}

	public void QuestionSelect5 ()
	{
		
	}

	*/

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0 && hasAnswered == false) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
				
			QuestionManager.Instance.SetQuestionEntry (UnityEngine.Random.Range (0, 2), 20, delegate(int gp) {
					RPCWrapper.Instance.RPCWrapAnswer ();
					battleController.SetPlayerGP (gp);
					HideUI ();
			});



			GameTimer.Instance.ToggleTimer (false);
			stoptimer = false;

			
		}
	}



	private void HideUI ()
	{
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}
		questionSelect.SetActive (false);
		GameTimer.Instance.ToggleTimer (false);

	}

}
