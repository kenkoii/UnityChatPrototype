using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TypingIcon : QuestionSystemBase, IQuestion
{

	public GameObject gpText;
	public Text questionText;
	private bool selectionIsClickable = true;
	public GameObject inputPrefab;
	public GameObject answerContent;

	public void Activate (Action<int,int> Result)
	{
		QuestionBuilder.PopulateQuestion ("SelectChangeTyping");
		currentRound = 1;
		correctAnswers = 0;
		NextRound ();
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}

	public void NextRound ()
	{
		
		LoadQuestion ();
		PopulateAnswerHolder (gameObject, inputPrefab, answerContent);
		QuestionHint ();
	}

	private void QuestionHint ()
	{
		answerButtons [0].transform.GetChild (0).
		GetComponent<Text> ().text = questionAnswer [0].ToString ().ToUpper ();
		answerButtons [0].GetComponent<Button> ().enabled = false;
	}
		
	public void OnSkipClick ()
	{
		if (!hasSkippedQuestion) {
			CheckAnswer (false);
			hasSkippedQuestion = true;
		}
	}

	public void TweenCallBack(){
		gpText.transform.DOScale (new Vector3(1,1,1),1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void OnFinishQuestion(){
		TweenCallBack ();
		hasSkippedQuestion = false;
		Clear ();
		answerIndex = 1;
		currentRound = currentRound + 1;
		NextRound ();
		QuestionController.Instance.Stoptimer = true;
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
		}, currentRound, correctAnswers);
		selectionIsClickable = true;
	}

	public void OnAnswerClick ()
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition (0.2f, 30.0f, 50, 0f, true);
		} else {
			EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
	}

	public void Clear ()
	{
		answerIndex = 1;
		foreach (GameObject o in answerButtons) {
			Destroy (o);
		}

	}
}
