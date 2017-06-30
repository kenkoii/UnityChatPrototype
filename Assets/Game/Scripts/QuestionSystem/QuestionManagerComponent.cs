﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionManagerComponent : EnglishRoyaleElement
{
	//private static int questiontype = 0;
	public GameObject[] questionTypeModals;

	//private int numberofQuestionTypes = 2;
	//private List<string> questionTypeName = new List<string>();
	void Start ()
	{

		//Debug.Log (questionTypeModals [0].name);
		//questionTypeModals [3].SetActive (true);
		//questionTypeName.Add ("SelectLetterIconModal");
		//questionTypeName.Add ("TypingModal");
		//questionTypeName.Add ("ChangeOrderModal");
		//questionTypeName.Add ("WordChoiceModal");
		//numberofQuestionTypes = questionTypeName.Count;
		// 0 = SelectLetter
		// 1 = Order
		// 2 = ChangeOrder
//		questiontype = 0;
//
//		SetQuestionEntry (questiontype, 3, delegate(int result) {
//			
//		});
	}

	public void QuestionHide(){

		for (int i = 0; i < questionTypeModals.Length; i++) {
			//Debug.Log (questionTypeModals[i].name);
			questionTypeModals [i].SetActive (false);
		}
	}

	public void SetQuestionEntry(int questionType, int questionTime, Action<int, int> onResult){
		questionTypeModals[questionType].SetActive (true);

		switch (questionType) {
		case 0:
			SelectLetterIcon selectletterIcon = app.view.questionSystemView.selectLetterIcon;
			//questionTypeModals[0].SetActive (true);
			app.controller.questionController.SetQuestion (selectletterIcon, questionTime, onResult);
			app.controller.questionController.TimeLeft = questionTime;

			break;
		case 1:
			TypingIcon typingicon = app.view.questionSystemView.typingIcon;
			//questionTypeModals[1].SetActive (true);
			app.controller.questionController.SetQuestion (typingicon, questionTime, onResult);
			app.controller.questionController.TimeLeft = questionTime;
			break;
		case 2:
			//questionTypeModals[2].SetActive (true);
			ChangeOrderIcon changeOrderIcon = app.view.questionSystemView.changeOrderIcon;
			app.controller.questionController.SetQuestion (changeOrderIcon, questionTime, onResult);
			app.controller.questionController.TimeLeft = questionTime;
			break;
		case 3:
			//questionTypeModals[2].SetActive (true);
			WordChoiceIcon wordchoiceIcon = app.view.questionSystemView.wordChoiceIcon;
			app.controller.questionController.SetQuestion (wordchoiceIcon, questionTime, onResult);
			app.controller.questionController.TimeLeft = questionTime;
			break;
		case 4:
			//questionTypeModals[2].SetActive (true);
			SlotMachineIcon slotMachineIcon = app.view.questionSystemView.slotMachineIcon;
			app.controller.questionController.SetQuestion (slotMachineIcon, questionTime, onResult);
			app.controller.questionController.TimeLeft = questionTime;
			break;
		}
			
	}
	public void DebugOnClick(){
		/*
		int time;
		Int32.TryParse (GameObject.Find ("Inputu").GetComponent<InputField> ().text, out time);
		if (time > 0) {

			switch (EventSystem.current.currentSelectedGameObject.name) {
			case "sellet":
				SetQuestionEntry (0, time, delegate(int result) {
					Debug.Log ("Total score is: " + result);
					questionTypeModals [0].SetActive (false);
					questionTypeModals [3].SetActive (true);
				});
				break;
			case "chaord":
				SetQuestionEntry (2, time, delegate(int result) {
					Debug.Log ("Total score is: " + result);
					questionTypeModals [2].SetActive (false);
					questionTypeModals [3].SetActive (true);
				});
				break;
			}
			questionTypeModals [3].SetActive (false);
			GameObject.Find ("Indicator"+1).GetComponent<Image>().color = Color.white;
			GameObject.Find ("Indicator"+2).GetComponent<Image>().color = Color.white;
			GameObject.Find ("Indicator"+3).GetComponent<Image>().color = Color.white;

		}*/
	}

}

