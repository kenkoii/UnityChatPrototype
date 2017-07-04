using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SlotMachineIcon : MonoBehaviour, IQuestion{
	private int currentRound = 1;
	private int correctAnswers;
	private int answerindex = 1;
	private List<GameObject> answerIdentifier = new List<GameObject> ();
	private string answerWrote;
	private bool hasSkippedQuestion = false;
	private string questionAnswer = "";
	public GameObject gpText;
	public GameObject[] selectionButtons = new GameObject[12];
	private List<GameObject> answerButtons = new List<GameObject> ();
	public GameObject inputPrefab;
	public GameObject answerContent;
	public Text questionText;
	private List<GameObject> answerGameObject = new List<GameObject>();

	public string answerwrote {
		get;
		set;
	}

	public void Activate(Action<int,int> Result){
		correctAnswers = 0;
		currentRound = 1;
		NextRound ();
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}

	public void NextRound(){
		
		LoadQuestion ();
		ShuffleAlgo ();
	}

	private void LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion ();
		questionAnswer = questionLoaded.answer;
		questionText.text = questionLoaded.question;
	}
		
	public void CheckAnswer(bool result){
		/*
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		spe.DeployEffect (result, answerButtons, questionAnswer, gpText, gameObject);
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		string isCorrectParam = result ? ParamNames.AnswerCorrect.ToString () : ParamNames.AnswerWrong.ToString ();
		param [isCorrectParam] = currentRound;
		FirebaseDatabaseComponent.Instance.SetParam (new BattleStatus(JsonConverter.DicToJsonStr (param).ToString()));
		QuestionController.Instance.Stoptimer = false;
		Invoke ("OnFinishQuestion", 1f);
		*/
	}

	public void getAnswer(string ans){
		if (questionAnswer == ans) {
			//correctAnswerGot ();
			SlotMachineOnChange smoc = new SlotMachineOnChange ();
			smoc.ClearAnswers ();
		}

	}
		
	public void TweenCallBack ()
	{
		TweenController.TweenTextScale (gpText.transform, Vector3.one, 1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void OnFinishQuestion ()
	{
		TweenCallBack ();
		hasSkippedQuestion = false;
		QuestionController.Instance.Stoptimer = true;
		//ClearAnswerList ();
		answerindex = 1;
		currentRound += 1;
		//NextQuestion ();
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
		}, currentRound, correctAnswers);
	}

	public void PopulateQuestionList(){
		/*
		questionlist.Clear ();
		List<string> databundle = CSVParser.GetQuestions ("slotmachine");
		int i = 0;
		int randomnum = UnityEngine.Random.Range (1, 3);
		foreach(string questions in databundle ){
			string[] splitter = databundle [i].Split (']');
			questionData = splitter [0];
			synonymData = splitter [1];
			antonymData = splitter [2];
			antonymData.Remove (antonymData.Length - 1);
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
			i+=1;
		}*/
	}

	public void ShuffleAlgo ()
	{
		string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int letterIndex = 0;
		int letterStartIndex = 0;
		int letterEndIndex = 3;
		int randomnum = UnityEngine.Random.Range (letterStartIndex+1, letterEndIndex);
		for (int i = 0; i < selectionButtons.Length; i++) {
			selectionButtons [i].transform.GetChild (0).GetComponent<Text> ().text = (i%randomnum)==0 ?
				questionAnswer [letterIndex].ToString ().ToUpper ():
				Letters [UnityEngine.Random.Range (0, Letters.Length)].ToString ().ToUpper ();
			if ((i % randomnum) == 0) {
				letterIndex += 1;
				letterStartIndex = letterEndIndex;
				letterEndIndex = letterEndIndex + 3;
				randomnum = UnityEngine.Random.Range (letterStartIndex, letterEndIndex);
			
			}
		}
	}

}
