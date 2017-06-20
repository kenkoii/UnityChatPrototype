﻿using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class PhaseAnswerController : EnglishRoyaleElement
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
		app.component.questionManagerComponent.SetQuestionEntry (questionNumber, app.model.battleModel.answerQuestionTime, delegate(int gp, int qtimeLeft) {
			if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
				app.component.rpcWrapperComponent.RPCWrapAnswer (qtimeLeft, gp);
			} else {
				app.component.rpcWrapperComponent.RPCWrapAnswer ();
			}

			//for mode 4
			app.model.battleModel.gpEarned = gp;

			battleController.SetPlayerGP (gp);
			if (app.model.battleModel.modePrototype == ModeEnum.Mode4) {
				if (app.model.battleModel.skillChosenCost < app.controller.battleController.playerGP) {
					Debug.Log("player GP is " + app.controller.battleController.playerGP + " and skill cost is " + app.model.battleModel.skillChosenCost);
					app.model.battleModel.playerSkillChosen ();
				}else{
					Debug.Log("skill less gp");
				}
			}
			HideUI ();
		});

	}


	private void StartTimer ()
	{
		if (stoptimer) {
			app.view.gameTimerView.ToggleTimer (true);
			if (timeLeft > 0 && hasAnswered == false) {
				app.view.gameTimerView.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
				
			app.component.questionManagerComponent.SetQuestionEntry (UnityEngine.Random.Range (0, 2), app.model.battleModel.answerQuestionTime, delegate(int gp, int qtimeLeft) {
				if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
					app.component.rpcWrapperComponent.RPCWrapAnswer (qtimeLeft, gp);
				} else {
					app.component.rpcWrapperComponent.RPCWrapAnswer ();
				}

				//for mode 4
				app.model.battleModel.gpEarned = gp;

				battleController.SetPlayerGP (gp);
				if (app.model.battleModel.modePrototype == ModeEnum.Mode4) {
					if (app.model.battleModel.skillChosenCost < app.controller.battleController.playerGP) {
						Debug.Log("player GP is " + app.controller.battleController.playerGP + " and skill cost is " + app.model.battleModel.skillChosenCost);
						app.model.battleModel.playerSkillChosen ();
					}else{
						Debug.Log("skill less gp");
					}
				}
				HideUI ();
			});



			app.view.gameTimerView.ToggleTimer (false);
			stoptimer = false;
		
		}
	}



	private void HideUI ()
	{
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}
		questionSelect.SetActive (false);
		app.view.gameTimerView.ToggleTimer (false);

	}

}