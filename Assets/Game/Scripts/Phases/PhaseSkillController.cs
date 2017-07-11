using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class PhaseSkillController : BasePhase
{
	public Button[] skillButton;
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
			SkillButtonInteractable (1, skillButton[0]);
			SkillButtonInteractable (2, skillButton[1]);
			SkillButtonInteractable (3, skillButton[2]);
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
		ButtonEnable (false);
		attackButton.gameObject.SetActive (false);
		CancelInvoke ("StartTimer");
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
		skillButton[0].interactable = buttonEnable;
		skillButton[1].interactable = buttonEnable;
		skillButton[2].interactable = buttonEnable;
		attackButton.interactable = buttonEnable;

	}

	public void SelectSkill (int skillNumber)
	{
		if (skillButton [skillNumber - 1].interactable) {
			TweenController.TweenScaleToLarge (EventSystem.current.currentSelectedGameObject.transform, Vector3.one, 0.3f);
			SelectSkillReduce (skillNumber);
		}
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
