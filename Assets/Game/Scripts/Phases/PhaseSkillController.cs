﻿using UnityEngine;
using UnityEngine.UI;
using System;

public class PhaseSkillController : BasePhase
{
	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	public GameObject skillDescription;
	public Text skillDescriptionText;

	public Button attackButton;


	private void SkillButtonInteractable (int skillNumber, Button button)
	{
		if (SkillManagerComponent.Instance.GetSkill (skillNumber).skillGpCost > BattleView.Instance.PlayerGP) {
			button.interactable = false;
		} else {
			button.interactable = true;
		}
	}

	public override void OnStartPhase ()
	{
		RPCDicObserver.AddObserver(SkillActivatorComponent.Instance);
		Debug.Log ("Starting Skill Phase");
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			ButtonEnable (true);
		} else {

			SkillButtonInteractable (1, skillButton1);
			SkillButtonInteractable (2, skillButton2);
			SkillButtonInteractable (3, skillButton3);

		}

		attackButton.interactable = true;
		attackButton.gameObject.SetActive (true);

		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);

	
	}

	public override void OnEndPhase ()
	{
		RPCDicObserver.RemoveObserver(SkillActivatorComponent.Instance);
		ShowSkillUI (false);

		CancelInvoke ("StartTimer");
	}

	public void ShowSkillUI (bool toggle, bool isIncludeDescription = true)
	{
		attackButton.gameObject.SetActive (toggle);
		if (isIncludeDescription) {
			skillDescription.SetActive (toggle);
		}
	}

	public void AttackButton ()
	{
		ButtonEnable (false);
		GameTimerView.Instance.ToggleTimer (false);
		FDController.Instance.SkillPhase ();
		stoptimer = false;
	}

	public void ButtonEnable (bool buttonEnable)
	{
		skillButton1.interactable = buttonEnable;
		skillButton2.interactable = buttonEnable;
		skillButton3.interactable = buttonEnable;
		attackButton.interactable = buttonEnable;

	}

	public void SelectSkill (int skillNumber)
	{
		
		SelectSkillReduce (skillNumber);

	}
		

	public void SkillDescription (int skillNumber)
	{
		SkillDescriptionReduce (SkillManagerComponent.Instance.GetSkill (skillNumber).skillDescription, true);

	}



	private void SkillDescriptionReduce (string description, bool isShow)
	{
		skillDescriptionText.text = description;
		skillDescription.SetActive (isShow);
	}

	public void CloseDescription ()
	{
		skillDescription.SetActive (false);
	}

	private void SelectSkillReduce (int skillNumber)
	{
		SelectSkillActivate (delegate() {
			SkillManagerComponent.Instance.ActivateSkill (skillNumber);

		}, delegate() {
			GameData.Instance.skillChosenCost = SkillManagerComponent.Instance.GetSkill (skillNumber).skillGpCost;

		});
	
	}

	private void SelectSkillActivate (Action activateSkill, Action skillCost)
	{
		//change to mode 2
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			GameData.Instance.playerSkillChosen = delegate() {
				activateSkill ();
			};
			skillCost ();
			FDController.Instance.SkillPhase ();
			Debug.Log ("skilled!");
		} else {
			activateSkill ();
		}
		ButtonEnable (false);
		GameTimerView.Instance.ToggleTimer (false);
		stoptimer = false;
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerView.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimerView.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			ButtonEnable (false);
			GameTimerView.Instance.ToggleTimer (false);
				
			FDController.Instance.SkillPhase ();
			Debug.Log ("stopped phase2 timer");
			stoptimer = false;

		}
	}
		

}
