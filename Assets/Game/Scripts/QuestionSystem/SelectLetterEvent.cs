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
	private Boolean modalRaise = false;
	private static Boolean modalHide = false;
	private GameObject questionModal;
	private int roundsLimit = 3;


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
		questionModal = GameObject.Find ("SelectLetterIconModal");
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
			//CODE FOR CLICKING ON EMPTY
		} else {
			for (int i = 1; i < selectionButtons.Length+1; i++) {
				if (EventSystem.current.currentSelectedGameObject.name == ("input" + i)) {
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
			inputButtons [(answerindex - 1)].transform.GetChild (0).
			GetComponent<Text> ().text 
			= EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text;
			EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text = "";
			for (int j = 0; j < questionAnswer.Length; j++) {
				answerwrote = answerwrote + (GameObject.Find ("input" + (j + 1)).transform.GetChild (0).GetComponent<Text> ().text);
			}
			if (answerwrote.Length == questionAnswer.Length) {
				if (answerwrote.ToUpper () == questionAnswer.ToUpper ()) {
					correctAnswers = correctAnswers + 1;
					GameObject.Find ("Indicator" + currentround).GetComponent<Image> ().color = Color.blue;
				} else {
					GameObject.Find ("Indicator" + currentround).GetComponent<Image> ().color = Color.red;
				}
				Clear ();
				answerindex = 1;
				currentround = currentround + 1;
				QuestionDoneCallback (true);
			}
		}
	}
	public void QuestionDoneCallback(bool result){
			QuestionController qc = new QuestionController ();
			qc.Returner (
				delegate {
				qc.onFinishQuestion =true;
					if (result) {
					SelectLetterIcon sli = new SelectLetterIcon ();
					if(currentround>roundsLimit){
						currentround = 1;
					}
					else{
						sli.NextRound (currentround);
					}
					}
			},currentround,correctAnswers
			);
	}

	public void Clear(){
		
		answerindex = 1;
		for (int i = 0; i < selectionButtons.Length-1; i++) {
			selectionButtons [i].transform.GetChild (0).GetComponent<Text> ().text = "";
			if (i <= questionAnswer.Length) {
				Destroy (inputButtons[i]);

			}
		}

	}
}
