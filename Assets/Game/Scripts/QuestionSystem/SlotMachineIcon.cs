using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SlotMachineIcon : MonoBehaviour, IQuestion{
	private int currentRound = 1;
	private int correctAnswers;
	private int answerindex = 1;
	private List<GameObject> answerIdentifier = new List<GameObject> ();
	private bool hasSkippedQuestion = false;
	private static string questionAnswer = "";
	public GameObject gpText;
	public GameObject[] roulletes = new GameObject[12];
	private static List<GameObject> answerButtons = new List<GameObject> ();
	public Text questionText;
	private List<GameObject> answerGameObject = new List<GameObject>();
	private List<GameObject> roulleteText = new List<GameObject> ();
	private bool gotAnswer = false;
	private SlotMachineOnChange smoc;

	public void Activate(Action<int,int> result){
		QuestionBuilder.PopulateQuestion ("slotmachine");
		correctAnswers = 0;
		currentRound = 1;
		NextQuestion ();
		//QuestionController qc = new QuestionController ();
		QuestionController.Instance.OnResult = result;
	}

	void Start()
	{
	 smoc = new SlotMachineOnChange ();
		//InvokeRepeating ("ProvideHint", 3, 3);
	}

	private void ProvideHint(){
		CheckAnswer (true);
		
	}
	public void NextQuestion(){
		LoadQuestion ();
		findSlotMachines ();
		ShuffleAlgo ();
	}
	void Update(){

		getAnswer(smoc.WrittenAnswer);

	}
	private void LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion ();
		Debug.Log (questionLoaded.question + "/" + questionLoaded.answer);
		questionAnswer = questionLoaded.answer;
		string question = questionLoaded.question;
		gameObject.transform.GetChild (0).GetComponent<Text> ().text = question;
	}
		
	public void CheckAnswer(bool result){
		
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		spe.DeployEffect (result, answerButtons, questionAnswer, gpText, gameObject);
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		string isCorrectParam = result ? ParamNames.AnswerCorrect.ToString () : ParamNames.AnswerWrong.ToString ();
		param [isCorrectParam] = currentRound;
		FirebaseDatabaseComponent.Instance.SetAnswerParam (new AnswerModel(JsonConverter.DicToJsonStr (param).ToString()));
		QuestionController.Instance.Stoptimer = false;
		Invoke ("OnFinishQuestion", 1f);

	}

	public void getAnswer(string ans){
		if (questionAnswer == ans && !gotAnswer) {
			Debug.Log ("hey");
			gotAnswer = true;
			CheckAnswer (true);
			SlotMachineOnChange smoc = new SlotMachineOnChange ();
			smoc.ClearAnswers ();
		}

	}

	public void TweenCallBack ()
	{
		TweenController.TweenTextScale (gpText.transform, Vector3.one, 1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void findSlotMachines(){

		roulleteText.Clear ();
		GameObject content;
		for (int i = 0; i < questionAnswer.Length; i++) {
			content = roulletes [i];
			for (int j = 0; j < 3; j++) {
				roulleteText.Add (content.transform.GetChild(j).gameObject);
			}
		}
		for(int i = 6 ; i > questionAnswer.Length ;i--){
			roulletes[i-1].transform.parent.parent.parent.parent.gameObject.SetActive(false);
		}
	}

	public void OnFinishQuestion ()
	{
		gotAnswer = true;
		TweenCallBack ();
		hasSkippedQuestion = false;
		QuestionController.Instance.Stoptimer = true;
		//ClearAnswerList ();
		answerindex = 1;
		currentRound += 1;
		NextQuestion ();
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
		}, currentRound, correctAnswers);
	}
		
	public void ShuffleAlgo ()
	{
		string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		int letterIndex = 0;
		int letterStartIndex = 0;
		int letterEndIndex = 3;
		int randomnum = UnityEngine.Random.Range (letterStartIndex+1, letterEndIndex);
		for (int i = 0; i < roulleteText.Count; i++) {
			roulleteText [i].transform.GetChild (0).GetComponent<Text> ().text = (i%randomnum)==0 ?
				questionAnswer [letterIndex].ToString ().ToUpper ():
				Letters [UnityEngine.Random.Range (0, Letters.Length)].ToString ().ToUpper ();
			if ((i % randomnum) == 0) {
				letterIndex += 1;
				letterStartIndex = letterEndIndex;
				letterEndIndex = letterEndIndex + 3;
				randomnum = UnityEngine.Random.Range (letterStartIndex, letterEndIndex);
				answerButtons.Add (roulleteText [i]);
			}
		}
	}

}
