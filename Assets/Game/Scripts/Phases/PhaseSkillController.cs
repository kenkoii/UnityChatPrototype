using UnityEngine;
using UnityEngine.UI;
using System;

public class PhaseSkillController : SingletonMonoBehaviour<PhaseSkillController>, IPhase
{
	public GameObject[] battleUI;

	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	public GameObject skillDescription;
	public Text skillDescriptionText;
	private bool stoptimer = false;
	private int timeLeft;
	public Button attackButton;


	private void SkillButtonInteractable (int skillNumber, Button button)
	{
		if (SkillManagerComponent.Instance.GetSkill (skillNumber).skillGpCost > BattleController.Instance.playerGP) {
			button.interactable = false;
		} else {
			button.interactable = true;
		}
	}

	public void OnStartPhase ()
	{
		
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


		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}

		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);

	
	}

	public void OnEndPhase ()
	{
		ShowSkillUI (false);
		CancelInvoke ("StartTimer");

	}

	public void ShowSkillUI (bool toggle, bool isIncludeDescription = true)
	{
		attackButton.gameObject.SetActive (toggle);
		if (isIncludeDescription) {
			skillDescription.SetActive (toggle);
		}
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (toggle);
		}
	}

	public void AttackButton ()
	{
		ButtonEnable (false);
		GameTimerView.Instance.ToggleTimer (false);
		RPCWrapperComponent.Instance.RPCWrapSkill ();
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
			RPCWrapperComponent.Instance.RPCWrapSkill ();
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
				
			RPCWrapperComponent.Instance.RPCWrapSkill ();
			Debug.Log ("stopped phase2 timer");
			stoptimer = false;

		}
	}
		

}
