using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase2Controller : MonoBehaviour
{
	public GameObject[] battleUI;
	public Text skillTimerText;
	public int skill1GPCost = 3;
	public int skill2GPCost = 3;
	public int skill3GPCost = 3;
	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	private bool stoptimer = false;
	private int timeLeft;
	private BattleController battleController;
	public Button attackButton;


	public void OnEnable ()
	{
		
		battleController = FindObjectOfType<BattleController> ();

		if (skill1GPCost > battleController.playerGP) {
			skillButton1.interactable = false;
		} else {
			skillButton1.interactable = true;
		}

		if (skill2GPCost > battleController.playerGP) {
			skillButton2.interactable = false;
		} else {
			skillButton2.interactable = true;
		}

		if (skill3GPCost > battleController.playerGP) {
			skillButton3.interactable = false;
		} else {
			skillButton3.interactable = true;
		}

		if (MyGlobalVariables.Instance.modePrototype == 2) {
			attackButton.interactable = true;
			attackButton.gameObject.SetActive (true);
		}

		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}


		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);

	
	}
		
	void OnDisable ()
	{
		if (MyGlobalVariables.Instance.modePrototype == 2) {
			attackButton.gameObject.SetActive (false);
		}
		CancelInvoke ("StartTimer");
	}

	public void AttackButton(){
		ButtonEnable (false);
		GameTimer.Instance.ToggleTimer (false);
		RPCWrapper.Instance.RPCWrapSkill ();
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
		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);
		GameTimer.Instance.ToggleTimer (false);
		stoptimer = false;
	}

	public void SelectSkill2 ()
	{
		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);
		GameTimer.Instance.ToggleTimer (false);
		stoptimer = false;
	}

	public void SelectSkill3 ()
	{
		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);
		GameTimer.Instance.ToggleTimer (false);
		stoptimer = false;
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			ButtonEnable (false);
			GameTimer.Instance.ToggleTimer (false);
				
			RPCWrapper.Instance.RPCWrapSkill ();
			Debug.Log ("stopped phase2 timer");
			stoptimer = false;

		}
	}
		

}
