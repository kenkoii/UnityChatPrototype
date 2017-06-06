using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
	public int questiontype;

	void Start ()
	{
		SetQuestionEntry (0, 25, delegate(int result) {
			Debug.Log("Total score is: " + result);
		});
	}

	public void SetQuestionEntry(int questionType, int questionTime, Action<int> onResult){
		switch (questionType) {
		case 0:
			SelectLetterIcon selectletterIcon = new SelectLetterIcon ();
			QuestionController qc = new QuestionController ();
			qc.SetQuestion (selectletterIcon, questionTime, onResult);
			qc.TimeLeft = questionTime;
			break;
		}
	}
}

