﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TypingIcon : EnglishRoyaleElement, IQuestion{
	private static List<Question> questionlist = new List<Question> ();
	private static string questionAnswer;
	private string questionString;
	private string questionData = "";
	private static string[] answerIdentifier = new string[13];
	public static int answerindex = 1;
	private int roundlimit = 3;
	private string answerwrote;
	public static int currentround = 1;
	private bool clickable = true;
	public static int correctAnswers;
	private string answerData = "";
	private static GameObject questionModal;
	private static List<GameObject> inputlist = new List<GameObject>();
	private static List<GameObject> outputlist = new List<GameObject>();
	private static List<string> questionsDone = new List<string>();
	private bool justSkipped = false;
	public GameObject gpText;
	public void Activate(GameObject entity,float timeduration,Action<int,int> Result){
		currentround = 1;
		correctAnswers = 0;
		NextRound (currentround);
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}

	public void NextRound(int round){
		PopulateQuestionList ();
		int randomize = UnityEngine.Random.Range (0, questionlist.Count);
		questionAnswer = questionlist [randomize].answer.ToUpper().ToString();
		questionString = questionlist [randomize].question;
		while (questionsDone.Contains (questionString)) {
			randomize = UnityEngine.Random.Range (0, questionlist.Count);
			questionAnswer = questionlist [randomize].answer.ToUpper().ToString();
			questionString = questionlist [randomize].question;
			if (!questionsDone.Contains (questionString)) {
				break;
			}
		} 
		questionsDone.Add (questionString);
		GameObject questionInput = Resources.Load ("Prefabs/inputContainer") as GameObject;
		questionModal = GameObject.Find("TypingModal");
		inputlist.Clear ();
		outputlist.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject output = Instantiate (questionInput) as GameObject; 
			output.transform.SetParent (questionModal.transform.GetChild (1).
				transform.GetChild (0).GetChild (0).transform, false);
			output.name = "output" + (i + 1);
			output.GetComponent<Button>().onClick.AddListener (() => {
				questionModal.GetComponent<TypingIcon>().OutputOnClick();
			});
			outputlist.Add(output);

			output.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
		questionModal.transform.GetChild (0).GetComponent<Text> ().text = questionString;
		outputlist [0].transform.GetChild (0).
		GetComponent<Text> ().text = questionAnswer[0].ToString().ToUpper();
		outputlist [0].GetComponent<Button> ().enabled = false;
	}

	public void InputOnClick(){
		if (!clickable) {
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition(0.2f, 30.0f, 50, 0f, true);
		} 
		else {
			answerwrote = "";
			int k = 1;
			foreach (GameObject findEmpty in outputlist) {
				if (findEmpty.transform.GetChild (0).GetComponent<Text> ().text == "") {
					answerindex = k;
					outputlist [(answerindex - 1)].transform.GetChild (0).
					GetComponent<Text> ().text 
					= EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text;
					break;
				} else {

				}
				k++;
			}
			foreach(GameObject readWrittenAnswer in outputlist){
				answerwrote = answerwrote + (readWrittenAnswer.transform.GetChild (0).GetComponent<Text> ().text);
			}
			answerIdentifier [(answerindex - 1)] = EventSystem.current.currentSelectedGameObject.name;
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
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerCorrect.ToString ()] = currentround;
			app.component.firebaseDatabaseComponent.SetParam(app.model.battleModel.isHost, app.component.rpcWrapperComponent.DicToJsonStr (param));

			for (int i = 0; i < questionAnswer.Length; i++) {
				GameObject ballInstantiated = Resources.Load ("Prefabs/scoreBall") as GameObject;
				Instantiate (ballInstantiated, 
					outputlist [i].transform.position, 
					Quaternion.identity);
			}
			gpText.GetComponent<Text> ().text = "1 GP";
			gpText.transform.DOScale (new Vector3 (5, 5, 5), 1.0f);
			Invoke("TweenCallBack", 1f);
		} else {
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerWrong.ToString ()] = currentround;
			app.component.firebaseDatabaseComponent.SetParam(app.model.battleModel.isHost, app.component.rpcWrapperComponent.DicToJsonStr (param));

			app.controller.audioController.PlayAudio (AudioEnum.Mistake);
			for (int i = 0; i < questionAnswer.Length; i++) {
				outputlist [i].transform.GetChild (0).GetComponent<Text> ().text = questionAnswer [i].ToString().ToUpper();
				outputlist [i].GetComponent<Image> ().color = Color.green;
			}
		}
	
		questionModal.transform.DOShakePosition(0.2f, 30.0f, 50, 0f, true);
		clickable = false;
		QuestionController qc = new QuestionController ();
		qc.Stoptimer = false;
		Invoke("OnEnd", 1f);

	}
	public void OnSkipClick(){
		if (!justSkipped) {
			QuestionDoneCallback (false);
			justSkipped = true;
		}
	}
	public void TweenCallBack(){
		gpText.transform.DOScale (new Vector3(1,1,1),1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void OnEnd(){
		justSkipped = false;
		QuestionController qc = new QuestionController ();
		Clear ();
		answerindex = 1;
		qc.Stoptimer = true;
		currentround = currentround + 1;

		NextRound (currentround);
		qc.Returner (delegate {
			qc.onFinishQuestion = true;
		}, currentround, correctAnswers);
		if (currentround > roundlimit) {
			Clear ();
		}
		clickable = true;
	}

	public void OutputOnClick(){
		app.controller.audioController.PlayAudio (AudioEnum.ClickButton);
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition(0.2f, 30.0f, 50, 0f, true);
		} else {
			EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
	}
	public void PopulateQuestionList(){
		//CSVParser cs = new CSVParser ();
		List<string> databundle = CSVParser.GetQuestions ("SelectChangeTyping");
		int i = 0;
		foreach(string questions in databundle ){
			string[] splitter = databundle[i].Split (']');	
			questionData = splitter [0];
			answerData = splitter [1];
				questionlist.Add (new Question (questionData, answerData, 0));
			i+=1;
		}
	}

	public void Clear(){
		answerindex = 1;
		foreach (GameObject o in outputlist) {
			Destroy (o);
		}
		questionModal.transform.GetChild (0).GetComponent<Text> ().text = "";
	}
}
