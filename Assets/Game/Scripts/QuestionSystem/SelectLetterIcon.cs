using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SelectLetterIcon : MonoBehaviour, IQuestion
{
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
	private List<GameObject> answerGameObject = new List<GameObject>();

	public void Activate (Action<int,int> result)
	{
		QuestionBuilder.PopulateQuestion ("SelectChangeTyping");
		currentRound = 1;
		correctAnswers = 0;
		NextQuestion ();
		QuestionController.Instance.OnResult = result;
	}

	void Start()
	{
		//InvokeRepeating ("ProvideHint", 3, 3);
	}

	public void NextQuestion ()
	{
		ClearAnswerList ();
		LoadQuestion ();
		PopulateAnswerHolder ();
		SelectionInit ();

	}

	private void ProvideHint(){
		Transform selectionObj = answerGameObject [answerindex - 1].transform;
		TweenController.TweenJumpTo (selectionObj,selectionObj.transform.localPosition,20f,1,0.5f);
		selectionObj.gameObject.GetComponent<Image> ().color = new Color (255f / 255, 249f / 255f, 149f / 255f);
	}

	private void LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion ();
		questionAnswer = questionLoaded.answer;
		string question = questionLoaded.question;
		gameObject.transform.GetChild (0).GetComponent<Text> ().text = question;
	}

	private void PopulateAnswerHolder ()
	{
		answerButtons.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject answerPrefab = Instantiate (inputPrefab) as GameObject; 
			answerPrefab.transform.SetParent (answerContent.transform, false);
			answerPrefab.name = "input" + (i + 1);
			answerPrefab.GetComponent<Button> ().onClick.AddListener (() => {
				OnAnswerClick (answerPrefab.GetComponent<Button> ());
			});
			answerButtons.Add (answerPrefab);
			answerPrefab.transform.GetChild (0).GetComponent<Text> ().text = "";
			answerPrefab.GetComponent<Image> ().color = new Color (136f / 255, 236f / 255f, 246f / 255f);
		}
	}

	public void OnAnswerClick (Button answerButton)
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
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

	public void OnSelectionClick (Button letterButton)
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);

		if (string.IsNullOrEmpty (letterButton.transform.GetChild (0).GetComponent<Text> ().text)) {
			
			TweenController.TweenShakePosition (letterButton.transform, 1.0f, 30.0f, 50, 90f);
			//ProvideHint (answerGameObject [answerindex - 1].transform);
		} else {
			
			answerIdentifier.Add (letterButton.gameObject);
			answerWrote = "";

			answerButtons [(answerindex - 1)].GetComponentInChildren<Text>().text 
			= letterButton.GetComponentInChildren<Text>().text;
			letterButton.GetComponentInChildren<Text>().text = "";
			for (int j = 0; j < questionAnswer.Length; j++) {
				answerWrote += answerButtons [j].transform.GetChild (0).GetComponent<Text> ().text;
			}

			if (answerWrote.Length.Equals (questionAnswer.Length)) {
				if (answerWrote.ToUpper ().Equals (questionAnswer.ToUpper ())) {
					CheckAnswer (true);
				} else {
					CheckAnswer (false);
				}
			}
			CheckAnswerHolder ();
		}
	}

	private void CheckAnswerHolder ()
	{
		for (int j = 1; j <= questionAnswer.Length + 1; j++) {
			GameObject findEmpty = answerButtons [j - 1].transform.GetChild (0).gameObject;
			if (string.IsNullOrEmpty (findEmpty.GetComponent<Text> ().text)) {
				answerindex = j;
				break;
			}
		}
	}

	public void CheckAnswer (bool result)
	{
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		spe.DeployEffect (result, answerButtons, questionAnswer, gpText, gameObject);
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		string isCorrectParam;
		if (result) {
			correctAnswers += 1;
			 isCorrectParam = ParamNames.AnswerCorrect.ToString ();
		} else {
			isCorrectParam = ParamNames.AnswerWrong.ToString ();
		}
		hasSkippedQuestion = true;
		//correctAnswers = result ? correctAnswers += 1 : correctAnswers;
		param [isCorrectParam] = currentRound;
		FirebaseDatabaseComponent.Instance.SetAnswerParam (new AnswerModel(JsonConverter.DicToJsonStr (param).ToString()));
		QuestionController.Instance.Stoptimer = false;

		Invoke ("OnFinishQuestion", 1f);
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
		ClearAnswerList ();
		answerindex = 1;
		currentRound += 1;
		NextQuestion ();
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
		}, currentRound, correctAnswers);
	}


	public void SelectionInit ()
	{
		answerGameObject.Clear ();
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
					answerGameObject.Add (selectionButtons [i]);
				}			
			}
			RandomExist [letterno] = randomnum;
			letterno += 1;

		}
	}

	public void OnSkipClick ()
	{
		if (!hasSkippedQuestion) {
			CheckAnswer (false);
			hasSkippedQuestion = true;

		}
	}

	public void ClearAnswerList ()
	{

		answerIdentifier.Clear ();
		if (answerButtons.Count > 0) {
			answerindex = 1;
			foreach (GameObject o in answerButtons) {
				Destroy (o);
			}
		}

	}
}
