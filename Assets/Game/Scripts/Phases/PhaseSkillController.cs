﻿using UnityEngine;
using UnityEngine.UI;
using System;

public class PhaseSkillController : EnglishRoyaleElement
{
	public GameObject[] battleUI;

	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	public GameObject skillDescription;

	private bool stoptimer = false;
	private int timeLeft;
	public Button attackButton;

	public void OnEnable ()
	{
		Debug.Log ("Starting Skill Phase");
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			ButtonEnable (true);
		} else {
			if (app.model.battleModel.Skill1GPCost > app.controller.battleController.playerGP) {
				skillButton1.interactable = false;
			} else {
				skillButton1.interactable = true;
			}

			if (app.model.battleModel.Skill2GPCost > app.controller.battleController.playerGP) {
				skillButton2.interactable = false;
			} else {
				skillButton2.interactable = true;
			}

			if (app.model.battleModel.Skill3GPCost > app.controller.battleController.playerGP) {
				skillButton3.interactable = false;
			} else {
				skillButton3.interactable = true;
			}
		}

	

		attackButton.interactable = true;
		attackButton.gameObject.SetActive (true);


		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}


		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);

	
	}

	void OnDisable ()
	{
		CancelInvoke ("StartTimer");
	}

	public void HideSkillUI ()
	{
		attackButton.gameObject.SetActive (false);
		skillDescription.SetActive (false);
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (false);
		}
	}

	public void AttackButton ()
	{
		ButtonEnable (false);
		app.view.gameTimerView.ToggleTimer (false);
		app.component.rpcWrapperComponent.RPCWrapSkill ();
		stoptimer = false;
	}

	private void ButtonEnable (bool buttonEnable)
	{
		skillButton1.interactable = buttonEnable;
		skillButton2.interactable = buttonEnable;
		skillButton3.interactable = buttonEnable;
		attackButton.interactable = buttonEnable;

	}

	public void SelectSkill1 ()
	{
		SelectSkill (delegate() {
			app.component.skillManagerComponent.ActivateSkill1 ();

		}, delegate() {
			app.model.battleModel.skillChosenCost = app.model.battleModel.Skill1GPCost;

		});

	}

	public void SelectSkill2 ()
	{
		SelectSkill (delegate() {
			app.component.skillManagerComponent.ActivateSkill2 ();
		}, delegate() {
			app.model.battleModel.skillChosenCost = app.model.battleModel.Skill2GPCost;

		});
	}

	public void SelectSkill3 ()
	{
		SelectSkill (delegate() {
			app.component.skillManagerComponent.ActivateSkill3 ();
		}, delegate() {
			app.model.battleModel.skillChosenCost = app.model.battleModel.Skill3GPCost;

		});
	}

	public void Skill1Description ()
	{
		skillDescription.transform.GetChild (0).GetComponent<Text> ().text = app.model.battleModel.skill1Description;
		skillDescription.SetActive (true);

	}

	public void Skill2Description ()
	{
		skillDescription.transform.GetChild (0).GetComponent<Text> ().text = app.model.battleModel.skill2Description;
		skillDescription.SetActive (true);
	}

	public void Skill3Description ()
	{
		skillDescription.transform.GetChild (0).GetComponent<Text> ().text = app.model.battleModel.skill3Description;
		skillDescription.SetActive (true);
	}

	public void CloseDescription ()
	{
		skillDescription.SetActive (false);
	}

	private void SelectSkill (Action activateSkill, Action skillCost)
	{
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			app.model.battleModel.playerSkillChosen = delegate() {
				activateSkill ();
			};
			skillCost ();
			app.component.rpcWrapperComponent.RPCWrapSkill ();
			Debug.Log ("skilled!");
		} else {
			activateSkill ();
		}
		ButtonEnable (false);
		app.view.gameTimerView.ToggleTimer (false);
		stoptimer = false;
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			app.view.gameTimerView.ToggleTimer (true);
			if (timeLeft > 0) {
				app.view.gameTimerView.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			ButtonEnable (false);
			app.view.gameTimerView.ToggleTimer (false);
				
			app.component.rpcWrapperComponent.RPCWrapSkill ();
			Debug.Log ("stopped phase2 timer");
			stoptimer = false;

		}
	}
		

}
