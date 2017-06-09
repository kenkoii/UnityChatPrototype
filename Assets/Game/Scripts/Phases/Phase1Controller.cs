using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase1Controller : MonoBehaviour
{

	public GameObject questionSelect;
	public GameObject[] battleUI;
	private bool hasAnswered = false;
	BattleController battleController;


	public void OnEnable ()
	{
		battleController = FindObjectOfType<BattleController> ();
		hasAnswered = false;
		StartCoroutine (StartTimer (5));
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI[i].SetActive (false);
		}

		questionSelect.SetActive (true);
		
	}

	void OnDisable(){
		if (questionSelect.activeInHierarchy) {
			questionSelect.SetActive (false);
		}
	}

	public void QuestionSelect1 ()
	{
		hasAnswered = true;
	
		questionSelect.SetActive (false);
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (0, 40, delegate(int gp) {
			RPCWrapper.Instance.RPCWrapAnswer();
			battleController.SetPlayerGP(gp);
			NextPhase();
		});


	}

	public void QuestionSelect2 ()
	{
		hasAnswered = true;
	
		questionSelect.SetActive (false);
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (1, 40, delegate(int gp) {
			RPCWrapper.Instance.RPCWrapAnswer();
			battleController.SetPlayerGP(gp);
			NextPhase();
		});

	}
	public void QuestionSelect3 ()
	{

	}

	public void QuestionSelect4 ()
	{

	}

	public void QuestionSelect5 ()
	{
		
	}



	IEnumerator StartTimer (int timeReceive)
	{
		
		int timer = timeReceive;
		yield return new WaitForSeconds (0.1f);
		GameTimer.Instance.ToggleTimer (true);
		while (timer > 0 && hasAnswered == false) {
			GameTimer.Instance.gameTimerText.text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}

		if (hasAnswered == false) {
			NextPhase ();
			GameTimer.Instance.ToggleTimer (false);

			switch (UnityEngine.Random.Range (1, 2)) {
			case 1:
				QuestionSelect1 ();
				break;
			
			case 2:
				QuestionSelect2 ();
				break;
			}
		
		}
	}

	private void NextPhase(){
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}
		questionSelect.SetActive (false);
		GameTimer.Instance.ToggleTimer (false);

	}

}
