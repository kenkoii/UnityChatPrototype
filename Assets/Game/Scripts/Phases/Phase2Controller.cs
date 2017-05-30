using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase2Controller : MonoBehaviour
{

	public int chooseSkillTimer = 5;
	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;

	private Coroutine timerCoroutine;


	void OnEnable ()
	{
		ButtonEnable (true);
		timerCoroutine = StartCoroutine (StartTimer (chooseSkillTimer));

	}

	private void ButtonEnable (bool buttonEnable)
	{
		skillButton1.enabled = buttonEnable;
		skillButton2.enabled = buttonEnable;
		skillButton2.enabled = buttonEnable;
	}

	public void SelectSkill1 ()
	{
		SkillManager.Instance.ActivateSkill1 ();
		StopTimer ();

	}

	public void SelectSkill2 ()
	{
		StopTimer ();

	}

	public void SelectSkill3 ()
	{
		StopTimer ();

	}

	IEnumerator StartTimer (int timeReceive)
	{
		int timer = timeReceive;

		while (timer > 0) {
			timer--;
			yield return new WaitForSeconds (1);
		}
			
		StopTimer ();

	}

		

	private void StopTimer ()
	{
		StopCoroutine (timerCoroutine);
		ButtonEnable (false);

		//CHECK FIREBASE FOR STATUS FOR NEXT PHASE

	}
}
