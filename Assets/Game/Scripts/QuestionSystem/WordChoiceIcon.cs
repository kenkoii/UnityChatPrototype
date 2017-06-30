using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WordChoiceIcon : EnglishRoyaleElement, IQuestion
{
	private static List<Question> questionlist = new List<Question> ();
	private static string questionAnswer;
	private string questionString;
	private string questionData = "";
	private int letterno;
	private string answertemp;
	private int roundlimit = 3;
	public static int currentround = 1;
	public static int correctAnswers;
	private string synonymData = "";
	private string antonymData = "";
	private static string answer1 = "";
	private static string answer2 = "";
	private static GameObject questionModal;
	private static List<GameObject> inputlist = new List<GameObject> ();
	private static List<string> questionsDone = new List<string> ();
	private List<string> wrongChoices = new List<string> ();
	private List<GameObject> answerClicked = new List<GameObject> ();
	private bool isSynonym = false;
	private bool justSkipped = false;
	private bool justAnswered = false;
	private string wrongChoiceGot = "";
	public GameObject gpText;

	public void Activate (GameObject entity, float timeduration, Action<int,int> Result)
	{
		currentround = 1;
		correctAnswers = 0;
		NextRound (currentround);
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}

	public void NextRound (int round)
	{
		PopulateQuestionList ();
		int randomize;
		randomize = UnityEngine.Random.Range (0, questionlist.Count);
		questionAnswer = questionlist [randomize].answer.ToUpper ().ToString ();

		questionString = questionlist [randomize].question;
		int whileIndex = 0 ;
		while (questionsDone.Contains (questionString)) {
			randomize = UnityEngine.Random.Range (0, questionlist.Count);
			questionAnswer = questionlist [randomize].answer.ToUpper ().ToString ();
			questionString = questionlist [randomize].question;
			if (!questionsDone.Contains (questionString )|| whileIndex>0) {
				break;
			}
			whileIndex += 1;
		} 
		inputlist.Clear ();
		for (int i = 1; i < 5; i++) {
			inputlist.Add (GameObject.Find ("Word" + i));
		}
		questionsDone.Add (questionString);
		questionModal = GameObject.Find ("WordChoiceModal");
		ShuffleAlgo ();
		if (isSynonym) {
			questionModal.transform.GetChild (0).GetComponent<Text> ().text = "Synonym: " + questionString.ToUpper ();
		} else {
			questionModal.transform.GetChild (0).GetComponent<Text> ().text = "Antonym: " + questionString.ToUpper ();
		}
	}

	public void InputOnClick ()
	{
		if (!justAnswered) {
			app.controller.audioController.PlayAudio (AudioEnum.ClickButton);
			GameObject answerclick = EventSystem.current.currentSelectedGameObject;

			if (answerclick.GetComponent<Image> ().color == Color.gray) {
				answerclick.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
			} else {
				answerclick.GetComponent<Image> ().color = Color.gray;
			}
			int colorindex = 0;
			for (int i = 0; i < 4; i++) {
				if (inputlist [i].GetComponent<Image> ().color == Color.gray) {
					colorindex += 1;
					answerClicked.Add (inputlist [i]);
					if (colorindex >= 2) {
						answertemp = answerClicked [0].transform.GetChild (0).GetComponent<Text> ().text;
						int answerclickindex = 0;
						foreach (GameObject c in answerClicked) {
							answerclickindex += 1;
							string answertemp = c.transform.GetChild (0).GetComponent<Text> ().text;
							if (answerclickindex == 2) {
								justAnswered = true;
								if (answertemp == answer1 || answertemp == answer2) {
									QuestionDoneCallback (true);
									answerClicked[0].GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 30f / 255f);
									answerClicked[1].GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 30f / 255f);
								} else {
									QuestionDoneCallback (false);

								}
							}
						}

					}

				}
			}
			answerClicked.Clear ();
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
			for (int i = 0; i < answerClicked.Count; i++) {
				GameObject ballInstantiated = Resources.Load ("Prefabs/scoreBall") as GameObject;
				Instantiate (ballInstantiated, 
					answerClicked[i].transform.position, 
					Quaternion.identity);
			}
			gpText.GetComponent<Text> ().text = "1 GP";
			gpText.transform.DOScale (new Vector3 (5, 5, 5), 1.0f);
			Invoke("TweenCallBack", 1f);
		} else {
			app.controller.audioController.PlayAudio (AudioEnum.Mistake);
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerWrong.ToString ()] = currentround;
			app.component.firebaseDatabaseComponent.SetParam(app.model.battleModel.isHost, app.component.rpcWrapperComponent.DicToJsonStr (param));

			for (int i = 0; i < inputlist.Count; i++) {

				if (answer1 == inputlist [i].transform.GetChild (0).GetComponent<Text> ().text ||
				    answer2 == inputlist [i].transform.GetChild (0).GetComponent<Text> ().text) {
					inputlist [i].GetComponent<Image> ().color = Color.red;

				} else {
					inputlist [i].GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
				}
			}
		}
		questionModal.transform.DOShakePosition(1.0f, 30.0f, 50,90, true);
		QuestionController qc = new QuestionController();
		qc.Stoptimer = false;
		Invoke("OnEnd", 1f);
	
	}

	public void TweenCallBack(){
		gpText.transform.DOScale (new Vector3(1,1,1),1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void OnEnd(){
		justAnswered = false;
		QuestionController qc = new QuestionController();
		qc.Stoptimer = true;
		justSkipped = false;
		Clear ();
		currentround = currentround + 1;
		NextRound (currentround);
		qc.Returner (delegate {
			qc.onFinishQuestion = true;
		}, currentround, correctAnswers);
		if (currentround == 4) {
			Clear ();
		}
	}

	public void PopulateQuestionList ()
	{
		questionlist.Clear ();
		List<string> databundle = CSVParser.GetQuestions ("wordchoice");
		int i = 0;
		int randomnum = UnityEngine.Random.Range (1, 3);
		foreach (string questions in databundle) {
			string[] splitQuestion = databundle [i].Split (']');	
			questionData = splitQuestion [0];	
			synonymData = splitQuestion [1];
			antonymData = splitQuestion [2];
			wrongChoices.Add (splitQuestion [3]);
			if (questionData.Length > 1) {
				switch (randomnum) {
				case 1:
					questionlist.Add (new Question (questionData, synonymData, 3));
					isSynonym = true;
					break;
				case 2:
					questionlist.Add (new Question (questionData, antonymData, 3));
					isSynonym = false;
					break;
				}
			}
			i += 1;
		}
	}

	public void OnSkipClick(){
		if (!justSkipped) {
			QuestionDoneCallback (false);
			justSkipped = true;
		}
	}

	public void ShuffleAlgo ()
	{
		List<int> RandomExist = new List<int> ();
		string[] temp = questionAnswer.Split ('/');
		letterno = 0;
		int randomnum = UnityEngine.Random.Range (0, 5); 
		int whileindex = 0;
		for (int i = 0; i < 4; i++) {
			
			while (true) {
				if (whileindex > 100) {
					break;
				}
				randomnum = UnityEngine.Random.Range (0, 4);
				bool index = RandomExist.Contains (randomnum);
				if (index) {
					randomnum = UnityEngine.Random.Range (0, 4);
				} else {
					RandomExist.Add (randomnum);
					letterno = letterno + 1;
					switch (letterno) {
					case 1:
						inputlist [randomnum].transform.GetChild (0).GetComponent<Text> ().text = 
						temp [0].ToString ().ToUpper ();
						answer1 = temp [0].ToString ().ToUpper ();

						break;
					case 2:
						inputlist [randomnum].transform.GetChild (0).GetComponent<Text> ().text = 
						temp [1].ToString ().ToUpper ();
						answer2 = temp [1].ToString ().ToUpper ();

						break;
					case 3:
						inputlist [randomnum].transform.GetChild (0).GetComponent<Text> ().text = 
							wrongChoices [UnityEngine.Random.Range (0, wrongChoices.Count)].ToUpper ();
						wrongChoiceGot = inputlist [randomnum].transform.GetChild (0).GetComponent<Text> ().text;
						break;
					case 4:
						while(true){
							string secondWrongChoice = wrongChoices [UnityEngine.Random.Range (0, wrongChoices.Count)].ToUpper ();
							if (secondWrongChoice != wrongChoiceGot) {
								inputlist [randomnum].transform.GetChild (0).GetComponent<Text> ().text = secondWrongChoice; 
								break;
							}

						}
						break;
					}

					break;

				}
				whileindex++;
			}
		}
			
			

	}

	public void Clear ()
	{
		foreach (GameObject g in inputlist) {
			g.GetComponent<Image> ().color = new Color(94f/255,255f/255f,148f/255f);
		}
	}
}
