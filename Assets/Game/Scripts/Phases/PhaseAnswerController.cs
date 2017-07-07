﻿using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class PhaseAnswerController : AbstractPhase
{
	public GameObject questionSelect;
	private bool hasAnswered = false;


	public override void OnStartPhase ()
	{
		Debug.Log ("Starting Answer Phase");
		hasAnswered = false;

		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);
		questionSelect.SetActive (true);

	}

	public override void OnEndPhase ()
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
		QuestionManagerComponent.Instance.SetQuestionEntry (questionNumber, GameData.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
			QuestionStart (gp, qtimeLeft);
		});

	}


	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerView.Instance.ToggleTimer (true);
			if (timeLeft > 0 && hasAnswered == false) {
				GameTimerView.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 

			HideUI ();
			QuestionManagerComponent.Instance.SetQuestionEntry (UnityEngine.Random.Range (0, 2), GameData.Instance.answerQuestionTime, delegate(int gp, int qtimeLeft) {
				QuestionStart (gp, qtimeLeft);
			});
				
			GameTimerView.Instance.ToggleTimer (false);
			stoptimer = false;
		
		}
	}

	private void QuestionStart (int gp, int qtimeLeft)
	{
		GameData.Instance.gpEarned = gp;
		BattleView.Instance.playerGP += gp;
		RPCWrapperComponent.Instance.RPCWrapAnswer (qtimeLeft, gp);

		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			if (GameData.Instance.skillChosenCost <= BattleView.Instance.playerGP) {
				if (GameData.Instance.playerSkillChosen != null) {
					GameData.Instance.playerSkillChosen ();
				}
			} else {
				Debug.Log ("LESS GP CANNOT ACTIVATE SKILL");
			}
		} 
		HideUI ();
	}

	private void HideUI ()
	{
		questionSelect.SetActive (false);
		GameTimerView.Instance.ToggleTimer (false);

	}

}
