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
		QuestionManager.Instance.SetQuestionEntry (questionNumber, MyGlobalVariables.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
			if (MyGlobalVariables.Instance.modePrototype == ModeEnum.Mode2) {
				RPCWrapper.Instance.RPCWrapAnswer (qtimeLeft, gp);
			} else {
				RPCWrapper.Instance.RPCWrapAnswer ();
			}

			//for mode 4
			MyGlobalVariables.Instance.gpEarned = gp;

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
				
			QuestionManager.Instance.SetQuestionEntry (UnityEngine.Random.Range (0, 2),  MyGlobalVariables.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
				if (MyGlobalVariables.Instance.modePrototype == ModeEnum.Mode2) {
					RPCWrapper.Instance.RPCWrapAnswer (qtimeLeft, gp);
				} else {
					RPCWrapper.Instance.RPCWrapAnswer ();
				}

				//for mode 4
				MyGlobalVariables.Instance.gpEarned = gp;

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
