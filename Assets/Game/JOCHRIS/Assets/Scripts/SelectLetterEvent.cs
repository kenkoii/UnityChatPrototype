using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SelectLetterEvent : MonoBehaviour {
	
	public static int lettercount = 12;
	private GameObject[] selectionButtons = new GameObject[13];
	private GameObject[] inputButtons = new GameObject[13];
	public int currentround = 1;
	public static int answerindex = 1;
	public List<string> answerIdentifier;
	public string answerwrote;
	public static string questionAnswer;
	public static string questionString;
	public static float timeLeft;
	public static int correctAnswers;
	public Boolean stoptimer = true;
	private Boolean modalRaise = false;
	private static Boolean modalHide = false;
	private GameObject questionModal;


	public void GetTimeDuration(float duration){
		timeLeft = duration;
	}
	public GameObject[] SelectionButtons{
		get{ 
			return selectionButtons;
		}
		set{ 
			selectionButtons = value;
		}

	}
	public GameObject[] InputButtons{
		get{ 
			return inputButtons;
		}
		set{ 
			inputButtons = value;
		}
	}
	void Start () {
		modalRaise = true;
		questionModal = GameObject.Find ("QuestionModal");
	}

	void Update () {
		RaiseModal ();

	}


	public void GetAnswer(string answer){
		questionAnswer = answer;
	}
	public void GetQuestion(string question){
		questionString = question;
	}

	public void AnswerOnClick ()
	{
		string answerclicked = "";
		answerindex = 1;
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
			
		} else {


			for (int i = 1; i < selectionButtons.Length+1; i++) {
				if (EventSystem.current.currentSelectedGameObject.name == ("input" + i)) {
					
					//answerindex = i;
					answerclicked = inputButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text;
					inputButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
					GameObject.Find (answerIdentifier [i - 1]).transform.GetChild (0).GetComponent<Text> ().text = answerclicked;

				}

			}
			for(int j=1;j<=questionAnswer.Length+1;j++){
				GameObject findEmpty = inputButtons [j].transform.GetChild (0).gameObject;

				if (findEmpty.GetComponent<Text> ().text == "") {
					answerindex = j;
					break;
				} 
			}
		}

	}

	public void LetterOnClick ()
	{
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
		} 
		else {
			//SelectLetterIcon sli = new SelectLetterIcon ();

			for (int i = 0; i < selectionButtons.Length - 1; i++) {
				selectionButtons [i] = GameObject.Find ("Letter" + (i + 1));
				if (i <= inputButtons.Length) {
					inputButtons [i] = GameObject.Find ("input" + (i + 1));
				}
			}
			for(int j=1;j<=questionAnswer.Length+1;j++){
				GameObject findEmpty = inputButtons [j-1].transform.GetChild (0).gameObject;

				if (findEmpty.GetComponent<Text> ().text == "") {
					answerindex = j;
					break;
				} 
			}
		
			answerIdentifier [(answerindex - 1)] = EventSystem.current.currentSelectedGameObject.name;
			answerwrote = "";
			for (int j = 0; j < selectionButtons.Length; j++) {
				if (EventSystem.current.currentSelectedGameObject.name == ("Letter" + (j + 1))) {
					inputButtons [(answerindex - 1)].GetComponent<Image> ().transform.GetChild (0).GetComponent<Text> ().text 
					= selectionButtons [j].transform.GetChild (0).GetComponent<Text> ().text;
					selectionButtons [j].transform.GetChild (0).GetComponent<Text> ().text = "";
				}
			}
			for (int j = 0; j < questionAnswer.Length; j++) {

				answerwrote = answerwrote + (GameObject.Find ("input" + (j + 1)).transform.GetChild (0).GetComponent<Text> ().text);
			}
			if (answerwrote.ToUpper() == questionAnswer.ToUpper()) {
				correctAnswers = correctAnswers + 1;

				clear ();
				answerindex = 1;
				GameObject.Find ("Indicator" + currentround).GetComponent<Image> ().color = Color.blue;
				currentround = currentround + 1;
				QuestionDoneCallback ();
				QuestionsEnd ();

			} else {
				answerindex = answerindex + 1;
				if (answerwrote.Length == questionAnswer.Length) {
					clear ();
					GameObject.Find ("Indicator" + currentround).GetComponent<Image> ().color = Color.red;
					currentround = currentround + 1;
					QuestionDoneCallback ();
					QuestionsEnd ();

				}

			}
		}
	}

	public void RaiseModal(){
		if (modalRaise) {
			GameObject modal = GameObject.Find ("QuestionModal");
			if (modal.transform.position.y < -2.68f) {
				modal.transform.position = new Vector3 (0, modal.transform.position.y + 2.0f, 0);
			} else {
				modalRaise = false;
			}
			if (modalHide) {
				HideModal ();
			}
		}
	}
	public void HideModal(){
		GameObject modal = GameObject.Find ("QuestionModal");
		if(modal.transform.position.y >= -3f){
			modal.transform.position = new Vector3 (0, modal.transform.position.y - 1.0f, 0);
			Debug.Log ("Modal y:" + modal.transform.position.y);
		} else {
			modalHide = false;
		}
	}
	public void QuestionsEnd(){
		
		if (currentround == 4) {
			modalRaise = false;
			clear ();

		}
	}

	public void QuestionDoneCallback(){
			QuestionController qc = new QuestionController ();
			qc.Returner (
				delegate {
					bool result = true;
					if (result) {
					SelectLetterIcon sli = new SelectLetterIcon ();
					if(currentround<4){
					sli.NextRound (currentround);
					}
					}
			},currentround,correctAnswers
			);
	}

	public void clear(){
		
		answerindex = 1;
		for (int i = 0; i < selectionButtons.Length-1; i++) {
			selectionButtons [i].transform.GetChild (0).GetComponent<Text> ().text = "";
			if (i <= questionAnswer.Length) {
				Destroy (inputButtons[i]);

			}
		}

	}
	/*
	public void DebugOnClick(){
		GameObject dropBox = GameObject.Find ("Duropu") as GameObject;
		GameObject inputBox = GameObject.Find ("Inputu");
		string duration = inputBox.GetComponent<InputField>().text;
		int num;
		Int32.TryParse (duration, out num);
		if(num>0){
		switch (dropBox.GetComponent<Dropdown> ().value) {
			case 0:
				QuestionManager qm = new QuestionManager ();
				QuestionController qc = new QuestionController ();
				qc.Stoptimer = true;
				Destroy (GameObject.Find ("TextArea"));
				Destroy (GameObject.Find ("DebugMode"));
				clear ();
				currentround = 1;
				correctAnswers = 0;
				answerindex = 0;
				questionModal.SetActive (true);
				GameObject.Find ("Indicator1").GetComponent<Image> ().color = Color.white;
				GameObject.Find ("Indicator2").GetComponent<Image> ().color = Color.white;
				GameObject.Find ("Indicator3").GetComponent<Image> ().color = Color.white;
				qm.SetQuestionEntry (0, num, delegate(int result) {
					Debug.Log("Total score is: " + result);
				});
			break;
		}
		}

	}*/
}
