using UnityEngine;
using UnityEngine.UI;
using System;

public class PhaseSkillController : EnglishRoyaleElement
{
	public GameObject[] battleUI;

	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	private bool stoptimer = false;
	private int timeLeft;
	public Button attackButton;



	public void OnEnable ()
	{
		

		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			ButtonEnable (true);
		} else {
			if (app.model.battleModel.skill1GPCost > app.controller.battleController.playerGP) {
				skillButton1.interactable = false;
			} else {
				skillButton1.interactable = true;
			}

			if (app.model.battleModel.skill2GPCost > app.controller.battleController.playerGP) {
				skillButton2.interactable = false;
			} else {
				skillButton2.interactable = true;
			}

			if (app.model.battleModel.skill3GPCost > app.controller.battleController.playerGP) {
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
		attackButton.gameObject.SetActive (false);

		CancelInvoke ("StartTimer");
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
			app.model.battleModel.skillChosenCost = app.model.battleModel.skill1GPCost;
		});

	}

	public void SelectSkill2 ()
	{
		SelectSkill (delegate() {
			app.component.skillManagerComponent.ActivateSkill1 ();
		}, delegate() {
			app.model.battleModel.skillChosenCost = app.model.battleModel.skill2GPCost;
		});
	}

	public void SelectSkill3 ()
	{
		SelectSkill (delegate() {
			app.component.skillManagerComponent.ActivateSkill1 ();
		}, delegate() {
			app.model.battleModel.skillChosenCost = app.model.battleModel.skill3GPCost;
		});
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
