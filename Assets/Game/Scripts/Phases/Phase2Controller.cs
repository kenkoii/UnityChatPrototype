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
	private static bool stoptimer = false;
	private static int timeLeft;



	public void OnEnable ()
	{
		ButtonEnable (true);

		if (skill1GPCost > StatusManager.Instance.playerGP) {
			skillButton1.interactable = false;
		}

		if (skill2GPCost > StatusManager.Instance.playerGP) {
			skillButton2.interactable = false;
		}

		if (skill3GPCost > StatusManager.Instance.playerGP) {
			skillButton3.interactable = false;
		}

		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}


		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating("StartTimer",0,1);

	
	}

	private void ButtonEnable (bool buttonEnable)
	{
		skillButton1.interactable = buttonEnable;
		skillButton2.interactable = buttonEnable;
		skillButton3.interactable = buttonEnable;
	}

	public void SelectSkill1 ()
	{
		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);
		stoptimer = false;
		GameTimer.Instance.ToggleTimer (false);
	}

	public void SelectSkill2 ()
	{
		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);
		stoptimer = false;
		GameTimer.Instance.ToggleTimer (false);
	}

	public void SelectSkill3 ()
	{
		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);
		stoptimer = false;
		GameTimer.Instance.ToggleTimer (false);
	}
	private void StartTimer(){
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
				ButtonEnable (false);
				GameTimer.Instance.ToggleTimer (false);
				
				FirebaseDatabaseFacade.Instance.CheckSkillPhase ();
				Debug.Log ("stopped phase2 timer");
				stoptimer = false;

		}
	}
		

}
