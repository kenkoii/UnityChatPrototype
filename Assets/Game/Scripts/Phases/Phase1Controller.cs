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
	private bool stoptimer = false;
	private int timeLeft;
	private int timeCount = 20;

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
		CancelInvoke ("StartTimer");

	}

	public void OnQuestionSelect ()
	{
		stoptimer = false;
		string questionSelectedName = EventSystem.current.currentSelectedGameObject.name;
		int questionNumber = Int32.Parse (questionSelectedName [questionSelectedName.Length - 1].ToString ()) - 1;	
		hasAnswered = true;
		questionSelect.SetActive (false);
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (questionNumber, timeCount, delegate(int gp, int qtimeLeft) {
			if (MyGlobalVariables.Instance.modePrototype == 2) {
				RPCWrapper.Instance.RPCWrapAnswer (qtimeLeft, gp);
			} else {
				RPCWrapper.Instance.RPCWrapAnswer ();
			}
			battleController.SetPlayerGP (gp);
			HideUI ();
		});

	}


	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0 && hasAnswered == false) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
				
			QuestionManager.Instance.SetQuestionEntry (UnityEngine.Random.Range (0, 2), timeCount, delegate(int gp, int qtimeLeft) {
				if (MyGlobalVariables.Instance.modePrototype == 2) {
					RPCWrapper.Instance.RPCWrapAnswer (qtimeLeft, gp);
				} else {
					RPCWrapper.Instance.RPCWrapAnswer ();
				}
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
