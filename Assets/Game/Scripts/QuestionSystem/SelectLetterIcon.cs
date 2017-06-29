using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;
using DG.Tweening;

public class SelectLetterIcon : EnglishRoyaleElement, IQuestion 
{
	private string questionData = "";
	private string answerData = "";
	private GameObject[] answerlist = new GameObject[13];
	public static List<Question> questionlist;
	private static List<string> questionsDone = new List<string> ();
	private static int round = 1;
	private int letterno;
	private static List<GameObject> selectionButtons = new List<GameObject>();
	private GameObject[] inputButtons = new GameObject[12];
	private static int currentround = 1;
	public static int answerindex = 1;
	public List<string> answerIdentifier;
	public string answerwrote;
	private bool justSkipped = false;
	public static string questionAnswer;
	public static string questionString;
	public static int correctAnswers;
	private static GameObject questionModal;
	public GameObject[] indicators = new GameObject[3];

	public void Activate (GameObject entity, float timeduration, Action<int,int> Result)
	{
		round = 1;
		currentround = 1;
		correctAnswers = 0;
		NextRound (round);
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}

	public void NextRound (int round)
	{
		selectionButtons.Clear ();
		foreach (Transform child in GameObject.Find("QuestionModalContent").transform) {
			GameObject.Destroy(child.gameObject);
		}
		foreach (Transform child in GameObject.Find("LetterViewContent").transform) {
			selectionButtons.Add (child.gameObject);
		}
		questionlist = new List<Question> ();

		PopulateQuestionList ();
		int randomize = UnityEngine.Random.Range (0, questionlist.Count);
		questionAnswer = questionlist [randomize].answer.ToUpper ().ToString ();
		questionString = questionlist [randomize].question;
		while (questionsDone.Contains (questionString)) {
			randomize = UnityEngine.Random.Range (0, questionlist.Count);
			questionAnswer = questionlist [randomize].answer.ToUpper ().ToString ();
			questionString = questionlist [randomize].question;
			if (!questionsDone.Contains (questionString)) {
				break;
			}
		} 
		questionsDone.Add (questionString);

		GameObject questionInput = Resources.Load ("Prefabs/inputContainer") as GameObject;
		questionModal = GameObject.Find ("SelectLetterIconModal");
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject input = Instantiate (questionInput) as GameObject; 
			input.transform.SetParent (questionModal.transform.GetChild (1).
				transform.GetChild (0).GetChild (0).transform, false);
			input.name = "input" + (i + 1);
			input.GetComponent<Button> ().onClick.AddListener (() => {
				questionModal.GetComponent<SelectLetterIcon> ().AnswerOnClick ();
			});
			answerlist [i] = input;
			input.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
		ShuffleAlgo ();
		questionAnswer = questionlist [randomize].answer;
		questionModal.transform.GetChild (0).GetComponent<Text> ().text = questionString;
	}

	public void AnswerOnClick ()
	{
		app.controller.audioController.PlayAudio (AudioEnum.ClickButton);
		Debug.Log (EventSystem.current.currentSelectedGameObject);
		string answerclicked = "";

		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
			//iTween.ShakePosition (EventSystem.current.currentSelectedGameObject, new Vector3 (10, 10, 10), 0.5f);
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition(0.2f, 30.0f, 50, 0f, true);
		} else {
			for (int i = 1; i < selectionButtons.Count + 1; i++) {
				if (EventSystem.current.currentSelectedGameObject.name == ("input" + i)) {
					answerclicked = inputButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text;
					inputButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
					GameObject.Find (answerIdentifier [i - 1]).transform.GetChild (0).GetComponent<Text> ().text = answerclicked;
				}
			}
			for (int j = 1; j <= questionAnswer.Length + 1; j++) {
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
		app.controller.audioController.PlayAudio (AudioEnum.ClickButton);
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition(0.2f, 30.0f, 50, 0f, true);
		} else {
			for (int i = 0; i < selectionButtons.Count - 1; i++) {
				selectionButtons [i] = GameObject.Find ("Letter" + (i + 1));
				if (i <= inputButtons.Length) {
					inputButtons [i] = GameObject.Find ("input" + (i + 1));
				}
			}
			for (int j = 1; j <= questionAnswer.Length + 1; j++) {
				GameObject findEmpty = inputButtons [j - 1].transform.GetChild (0).gameObject;

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
					QuestionDoneCallback (true);
				} else {
					QuestionDoneCallback (false);
				}
			}
		}
	}

	public void QuestionDoneCallback (bool result)
	{
		if (result) {
			app.controller.audioController.PlayAudio (AudioEnum.Correct);

			correctAnswers = correctAnswers + 1;
			indicators[currentround-1].GetComponent<Image> ().color = Color.blue;
			for (int i = 0; i < questionAnswer.Length; i++) {
				GameObject ballInstantiated = Resources.Load ("Prefabs/scoreBall") as GameObject;
				Instantiate (ballInstantiated, 
					inputButtons [i].transform.position, 
					Quaternion.identity, questionModal.transform);
			}
			indicators[currentround-1].transform.GetChild (0).GetComponent<Text> ().text = "1 GP";
			indicators[currentround-1].transform.GetChild (0).DOScale (new Vector3 (5, 5, 5), 1.0f);
			Invoke("TweenCallBack", 1f);

		} else {
			app.controller.audioController.PlayAudio (AudioEnum.Mistake);
			indicators[currentround-1].GetComponent<Image> ().color = Color.red;
			for (int i = 0; i < questionAnswer.Length; i++) {
				answerlist [i].transform.GetChild (0).GetComponent<Text> ().text = questionAnswer [i].ToString().ToUpper();
				answerlist [i].GetComponent<Image> ().color = Color.green;
			}
		}
		questionModal.transform.DOShakePosition(1.0f, 30.0f, 50,90, true);
		QuestionController qc = new QuestionController ();
		qc.Stoptimer = false;
		Invoke("OnEnd", 1f);
	}

	public void TweenCallBack(){
		indicators[currentround-1].
		transform.GetChild (0).DOScale (new Vector3(1,1,1),1.0f);
		indicators[currentround-1].
		transform.GetChild (0).GetComponent<Text> ().text = " ";
	}

	public void OnEnd(){
		QuestionController qc = new QuestionController ();
		justSkipped = false;
		qc.Stoptimer = true;

		Clear ();
		answerindex = 1;
		currentround = currentround + 1;
		NextRound (currentround);
		qc.Returner (delegate {
			qc.onFinishQuestion = true;
		}, currentround, correctAnswers);
		if (currentround == 4) {
			Clear ();
		}
	}

	public void PopulateQuestionList(){
		List<string> databundle = CSVParser.GetQuestions ("wingquestion");
		int i = 0;
		foreach(string questions in databundle ){
			string[] splitter = databundle[i].Split (']');	

			questionData = splitter [0];
			answerData = splitter [1];
			questionlist.Add (new Question (questionData, answerData, 0));

			i+=1;
		}
	}

	public void ShuffleAlgo ()
	{
		Debug.Log ("SelectionListCount"+selectionButtons.Count);
		int[] RandomExist = new int[questionAnswer.Length];
		string temp = questionAnswer;
		for (int f = 1; f < 13; f++) {
			string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			int randomnum2 = UnityEngine.Random.Range (1, 26);
			int index = Array.IndexOf (RandomExist, f);
			if (index > -1) {

			} else {
				selectionButtons[f-1].GetComponent<Image> ().transform.GetChild (0).GetComponent<Text> ().text = 
					alphabet [randomnum2].ToString ().ToUpper ();
			}
		}
		letterno = 0;
		int randomnum = 0;      
		for (int z = 0; z < temp.Length; z++) {
			randomnum = UnityEngine.Random.Range (1, selectionButtons.Count);        
			RandomExist [letterno] = randomnum;
			while (true) {
				int index = Array.IndexOf (RandomExist, randomnum);
				if (index > -1) {
					randomnum = UnityEngine.Random.Range (1, selectionButtons.Count);
				} else {
					break;
				}
			}
			for (int i = 0; i < selectionButtons.Count; i++) {
				if (randomnum == i) {
					selectionButtons[i].GetComponent<Image> ().
					transform.GetChild (0).GetComponent<Text> ().text = temp [letterno].ToString ().ToUpper ();    
					Debug.Log (selectionButtons[i].name);
				}			
			}
			RandomExist [letterno] = randomnum;
			letterno = letterno + 1;

		}



	}
	public void OnSkipClick(){
		if (!justSkipped) {
			QuestionDoneCallback (false);
			justSkipped = true;
		}
	}
	public void Clear ()
	{
		answerindex = 1;
		for (int i = 0; i < selectionButtons.Count; i++) {
			selectionButtons [i].transform.GetChild (0).GetComponent<Text> ().text = "";
			if (i <= questionAnswer.Length) {
				Destroy (inputButtons [i]);
			}
		}

	}
}
