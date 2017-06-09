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

	private Coroutine timerCoroutine;


	public void OnEnable ()
	{
		
		Debug.Log ("phase2 started");
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}
		ButtonEnable (true);
		StartCoroutine (StartTimer (chooseSkillTimer));

	}

	private void ButtonEnable (bool buttonEnable)
	{
		skillButton1.interactable = buttonEnable;
		skillButton2.interactable = buttonEnable;
		skillButton3.interactable = buttonEnable;
	}

	public void SelectSkill1 ()
	{
//		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);

	}

	public void SelectSkill2 ()
	{
//		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);

	}

	public void SelectSkill3 ()
	{
//		SkillManager.Instance.ActivateSkill1 ();
		ButtonEnable (false);

	}

	IEnumerator StartTimer (int timeReceive)
	{
		int timer = timeReceive;
		yield return new WaitForSeconds (0.1f);
		GameTimer.Instance.ToggleTimer (true);

		while (timer > 0) {
			GameTimer.Instance.gameTimerText.text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}
			
		ButtonEnable (false);
		GameTimer.Instance.ToggleTimer (false);

		FirebaseDatabaseFacade.Instance.CheckSkillPhase ();
	}

		
		


}
