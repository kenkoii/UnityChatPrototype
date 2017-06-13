using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase2Controller : MonoBehaviour
{
	public GameObject[] battleUI;
	public int chooseSkillTimer = 5;
	public Text skillTimerText;
	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	private static bool stoptimer = false;
	private static int timeLeft;



	public void OnEnable ()
	{
		
		Debug.Log ("phase2 started");
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}
		ButtonEnable (true);

		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating("StartTimer",0,1);
		Debug.Log ("phase2 started......");

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

	}

	public void SelectSkill2 ()
	{
		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);

	}

	public void SelectSkill3 ()
	{
		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);

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
