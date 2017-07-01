using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SelectLetterIcon : EnglishRoyaleElement, IQuestion
{
	private int currentRound = 1;
	private int correctAnswers;
	private int answerindex = 1;
	public List<GameObject> answerIdentifier;
	private string answerWrote;
	private bool hasSkippedQuestion = false;
	private string questionAnswer = "";
	private GameObject questionContainer;
	public GameObject gPtext;
	public GameObject[] selectionButtons = new GameObject[12];
	private GameObject[] answerButtons = new GameObject[12];
	private QuestionController questionControl;
	private AudioController audioControl;
	public GameObject inputPrefab;
	public GameObject answerContent;
	public Text questionText;

	void Start ()
	{
		questionControl = app.controller.questionController;
		audioControl = app.controller.audioController;
		questionContainer = gameObject;
	}

	public void Activate (Action<int,int> result)
	{
		QuestionBuilder.PopulateQuestion ("SelectChangeTyping");
		currentRound = 1;
		correctAnswers = 0;
		NextQuestion();
		app.controller.questionController.OnResult = result;
	}

	public void NextQuestion ()
	{
		ClearAnswerList ();
		LoadQuestion ();
		PopulateAnswerHolder ();
		SelectionInit ();
	}

	private void LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion ();
		questionAnswer = questionLoaded.answer;
		questionText.text = questionLoaded.question;
	}


	private void PopulateAnswerHolder()
	{
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject answerPrefab = Instantiate (inputPrefab) as GameObject; 
			answerPrefab.transform.SetParent (answerContent.transform, false);
			answerPrefab.name = "input" + (i + 1);
			answerPrefab.GetComponent<Button> ().onClick.AddListener (() => {
				AnswerOnClick (answerPrefab.GetComponent<Button> ());
			});
			answerButtons [i] = answerPrefab;
			answerPrefab.transform.GetChild (0).GetComponent<Text> ().text = "";
			answerPrefab.GetComponent<Image> ().color = new Color(136f/255,236f/255f,246f/255f);
		}
	}


	public void AnswerOnClick (Button answerButton)
	{
		audioControl.PlayAudio (AudioEnum.ClickButton);
		string answerclicked = "";
		if (string.IsNullOrEmpty (answerButton.transform.GetChild (0).GetComponent<Text> ().text)) {
			TweenController.TweenShakePosition (answerButton.transform, 0.5f, 15.0f, 50, 90f);
		} else {
			for (int i = 1; i < selectionButtons.Length + 1; i++) {
				if (answerButton.name.Equals ("input" + i)) {
					answerclicked = answerButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text;
					answerButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
					answerIdentifier [i - 1].transform.GetChild (0).GetComponent<Text> ().text = answerclicked;
				}
			}
			for (int j = 1; j <= questionAnswer.Length + 1; j++) {
				GameObject findEmpty = answerButtons [j].transform.GetChild (0).gameObject;
				if (string.IsNullOrEmpty (findEmpty.GetComponent<Text> ().text)) {
					answerindex = j;
					break;
				} 
			}
		}
	}

	public void SelectionOnClick (Button letterButton)
	{
		audioControl.PlayAudio (AudioEnum.ClickButton);
		if (string.IsNullOrEmpty (letterButton.transform.GetChild (0).GetComponent<Text> ().text)) {
			TweenController.TweenShakePosition (letterButton.transform, 1.0f, 30.0f, 50, 90f);
		} else {
			
			for (int j = 1; j <= questionAnswer.Length + 1; j++) {
				GameObject findEmpty = answerButtons [j - 1].transform.GetChild (0).gameObject;

				if (string.IsNullOrEmpty(findEmpty.GetComponent<Text> ().text)) {
					answerindex = j;
					break;
				} 
			}

			answerIdentifier [(answerindex - 1)] = letterButton.gameObject;
			answerWrote = "";

			answerButtons [(answerindex - 1)].transform.GetChild (0).GetComponent<Text> ().text 
			= letterButton.transform.GetChild (0).GetComponent<Text> ().text;

			letterButton.transform.GetChild (0).GetComponent<Text> ().text = "";

			for (int j = 0; j < questionAnswer.Length; j++) {
				answerWrote += answerButtons [j].transform.GetChild (0).GetComponent<Text> ().text;
			}
			if (answerWrote.Length.Equals (questionAnswer.Length)) {
				if (answerWrote.ToUpper ().Equals (questionAnswer.ToUpper ())) {
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
			audioControl.PlayAudio (AudioEnum.Correct);
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerCorrect.ToString ()] = currentRound;
			app.component.firebaseDatabaseComponent.SetParam (app.model.battleModel.isHost, app.component.rpcWrapperComponent.DicToJsonStr (param));

			correctAnswers = correctAnswers + 1;
			for (int i = 0; i < questionAnswer.Length; i++) {
				GameObject ballInstantiated = Resources.Load ("Prefabs/scoreBall") as GameObject;
				Instantiate (ballInstantiated, 
					answerButtons [i].transform.position, 
					Quaternion.identity, gameObject.transform);
			}

			gPtext.GetComponent<Text> ().text = "1 GP";
			TweenController.TweenTextScale (gPtext.transform, new Vector3 (5, 5, 5), 1.0f);
			Invoke ("TweenCallBack", 1f);

		} else {
			audioControl.PlayAudio (AudioEnum.Mistake);
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerWrong.ToString ()] = currentRound;
			app.component.firebaseDatabaseComponent.SetParam (app.model.battleModel.isHost, app.component.rpcWrapperComponent.DicToJsonStr (param));

			for (int i = 0; i < questionAnswer.Length; i++) {
				answerButtons [i].transform.GetChild (0).GetComponent<Text> ().text = questionAnswer [i].ToString ().ToUpper ();
				answerButtons [i].GetComponent<Image> ().color = new Color(229f/255,114f/255f,114f/255f);
			}
		}
		TweenController.TweenShakePosition (gameObject.transform, 1.0f, 30.0f, 50, 90f);
		TweenController.TweenTextScale (gPtext.transform, new Vector3 (5, 5, 5), 1.0f);
		questionControl.Stoptimer = false;
		Invoke ("OnFinishQuestion", 1f);

	}

	public void TweenCallBack ()
	{
		TweenController.TweenTextScale (gPtext.transform, Vector3.one, 1.0f);
		gPtext.GetComponent<Text> ().text = " ";
	}

	public void OnFinishQuestion ()
	{
		hasSkippedQuestion = false;
		questionControl.Stoptimer = true;
		ClearAnswerList ();
		answerindex = 1;
		currentRound += 1;
		NextQuestion ();
		questionControl.Returner (delegate {
			questionControl.onFinishQuestion = true;
		}, currentRound, correctAnswers);
	
	}


	public void SelectionInit ()
	{
		
		int[] RandomExist = new int[questionAnswer.Length];
		string temp = questionAnswer;
		for (int f = 1; f < 13; f++) {
			string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			int randomnum2 = UnityEngine.Random.Range (1, 26);
			int index = Array.IndexOf (RandomExist, f);
			if (index > -1) {

			} else {
				selectionButtons [f - 1].GetComponent<Image> ().transform.GetChild (0).GetComponent<Text> ().text = 
					alphabet [randomnum2].ToString ().ToUpper ();
			}
		}
		int letterno = 0;
		int randomnum = 0;      
		for (int z = 0; z < temp.Length; z++) {
			randomnum = UnityEngine.Random.Range (1, selectionButtons.Length);        
			RandomExist [letterno] = randomnum;
			while (true) {
				int index = Array.IndexOf (RandomExist, randomnum);
				if (index > -1) {
					randomnum = UnityEngine.Random.Range (1, selectionButtons.Length);
				} else {
					break;
				}
			}
			for (int i = 0; i < selectionButtons.Length; i++) {
				if (randomnum == i) {
					selectionButtons [i].GetComponent<Image> ().
					transform.GetChild (0).GetComponent<Text> ().text = temp [letterno].ToString ().ToUpper ();    
				}			
			}
			RandomExist [letterno] = randomnum;
			letterno += 1;

		}
	}

	public void OnSkipClick ()
	{
		if (!hasSkippedQuestion) {
			QuestionDoneCallback (false);
			hasSkippedQuestion = true;
		}
	}

	public void ClearAnswerList ()
	{
		if (answerButtons.Length > 0) {
			answerindex = 1;

			for (int i = 0; i < selectionButtons.Length; i++) {
				selectionButtons [i].transform.GetChild (0).GetComponent<Text> ().text = "";
				if (i <= questionAnswer.Length) {
					Destroy (answerButtons [i]);
				}
			}
		}

	}
}
