using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Phase1Controller : MonoBehaviour
{

	public GameObject questionSelect;
	public GameObject[] battleUI;
	private bool hasAnswered = false;
	BattleController battleController;
	private static bool stoptimer = false;
	private static int timeLeft;

	public void OnEnable ()
	{
		battleController = FindObjectOfType<BattleController> ();
		hasAnswered = false;
		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating("StartTimer2",0,1);
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

	public void OnQuestionSelect(){
		stoptimer = false;
		string questionSelectedName = EventSystem.current.currentSelectedGameObject.name;
		int questionNumber = Int32.Parse(questionSelectedName[questionSelectedName.Length-1].ToString())-1;	
		hasAnswered = true;
		questionSelect.SetActive (false);
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (questionNumber, 40, delegate(int gp) {
			RPCWrapper.Instance.RPCWrapAnswer();
			battleController.SetPlayerGP(gp);
			NextPhase();
		});

	}
	/*
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

	*/

	private void StartTimer2(){
		if (stoptimer) {
			GameTimer.Instance.ToggleTimer (true);
			if (timeLeft > 0 && hasAnswered == false) {
				GameTimer.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;

			} else {
				GameTimer.Instance.ToggleTimer (false);
				stoptimer = false;
				if (hasAnswered) {
					QuestionManager.Instance.SetQuestionEntry (UnityEngine.Random.Range (0, 2), 40, delegate(int gp) {
						RPCWrapper.Instance.RPCWrapAnswer();
						battleController.SetPlayerGP(gp);
						NextPhase();
					});
				}
			}
		}
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
			//NextPhase ();
			GameTimer.Instance.ToggleTimer (false);
			/*
			switch (UnityEngine.Random.Range (1, 2)) {
			case 1:
				QuestionSelect1 ();
				break;
			
			case 2:
				QuestionSelect2 ();
				break;
			}*/
			QuestionManager.Instance.SetQuestionEntry (UnityEngine.Random.Range (0, 2), 40, delegate(int gp) {
				RPCWrapper.Instance.RPCWrapAnswer();
				battleController.SetPlayerGP(gp);
				NextPhase();
			});
		
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
