using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase1Controller : MonoBehaviour, IPhase
{

	public GameObject questionSelect;
	public GameObject[] battleUI;
	private bool hasAnswered = false;



	public void StartPhase ()
	{
		
		DoOnMainThread.ExecuteOnMainThread.Enqueue(() => { StartCoroutine (StartTimer (5)); } );
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
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (0, 40, delegate(int gp) {
			StatusManager.Instance.playerGP = gp;

		});
//		RPCWrapper.Instance.RPCWrapAnswer();
		questionSelect.SetActive (false);
	}

	public void QuestionSelect2 ()
	{
		//call question callback here
		QuestionManager.Instance.SetQuestionEntry (1, 40, delegate(int gp) {
			StatusManager.Instance.playerGP = gp;

		});
		//		RPCWrapper.Instance.RPCWrapAnswer();
		questionSelect.SetActive (false);
	}
	public void QuestionSelect3 ()
	{
		//call question callback here
		//		RPCWrapper.Instance.RPCWrapAnswer();
		questionSelect.SetActive (false);
	}

	public void QuestionSelect4 ()
	{
		//call question callback here
		//		RPCWrapper.Instance.RPCWrapAnswer();
		questionSelect.SetActive (false);
	}

	public void QuestionSelect5 ()
	{
		//call question callback here
		//		RPCWrapper.Instance.RPCWrapAnswer();
		questionSelect.SetActive (false);
	}



	IEnumerator StartTimer (int timeReceive)
	{
		hasAnswered = false;
		int timer = timeReceive;
		GameTimer.Instance.ToggleTimer (true);

		while (timer > 0 && hasAnswered == false) {
			GameTimer.Instance.gameTimerText.text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}

		hasAnswered = true;
		for (int i = 0; i < battleUI.Length; i++) {
			battleUI[i].SetActive (true);
		}
		questionSelect.SetActive (false);
		GameTimer.Instance.ToggleTimer (false);
		FirebaseDatabaseFacade.Instance.CheckAnswerPhase ();

	}

}
