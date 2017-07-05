using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class WordChoiceIcon : MonoBehaviour, IQuestion
{
	private int currentRound = 1;
	private int score;
	private string answerWrote;
	private bool hasSkippedQuestion = false;
	private string questionAnswer = "";
	private GameObject questionContainer;
	public GameObject gpText;
	public GameObject[] selectionButtons = new GameObject[4];
	public Text questionText;
	private bool justAnswered = false;
	private List<GameObject> answerClicked = new List<GameObject>();
	private string answer1 = "";
	private string answer2 = "";
	private List<GameObject> correctAnswers = new List<GameObject> ();

	public void Activate (Action<int,int> Result)
	{
		
		QuestionBuilder.PopulateQuestion ("wordchoice");
		currentRound = 1;
		score = 0;
		NextQuestion ();
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}
		
	public void NextQuestion ()
	{
		LoadQuestion ();
		SelectionInit ();
	}

	private void LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion ();
		questionAnswer = questionLoaded.answer;
		questionText.text = questionLoaded.question;
	}

	public void QuestionCallback (bool result)
	{
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		spe.DeployEffect (result, correctAnswers, questionAnswer, gpText, gameObject);
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		string isCorrectParam = result ? ParamNames.AnswerCorrect.ToString () : ParamNames.AnswerWrong.ToString ();
		param [isCorrectParam] = currentRound;
		score = result ? score += 1 : score;
		FirebaseDatabaseComponent.Instance.SetAnswerParam (new AnswerModel(JsonConverter.DicToJsonStr (param).ToString()));
		QuestionController.Instance.Stoptimer = false;
		Invoke ("OnFinishQuestion", 1f);
	}

	public void TweenCallBack(){
		gpText.transform.DOScale (new Vector3(1,1,1),1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void OnFinishQuestion(){
		TweenCallBack ();
		justAnswered = false;
		QuestionController qc = new QuestionController();
		qc.Stoptimer = true;
		hasSkippedQuestion = false;
		Clear ();
		currentRound = currentRound + 1;
		NextQuestion ();
		qc.Returner (delegate {
			qc.onFinishQuestion = true;
		}, currentRound, score);
		if (currentRound == 4) {
			Clear ();
		}
	}

	public void OnSkipClick(){
		if (!hasSkippedQuestion) {
			QuestionCallback (false);
			hasSkippedQuestion = true;
		}
	}

	public void OnClickSelection ()
	{
		if (!justAnswered) {
			AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
			GameObject wordClicked = EventSystem.current.currentSelectedGameObject;
			String wordClickedString = wordClicked.transform.GetChild (0).GetComponent<Text> ().text;
			if (wordClicked.GetComponent<Image> ().color == Color.gray) {
				wordClicked.GetComponent<Image> ().color = new Color (94f / 255, 255f / 255f, 148f / 255f);
				answerClicked.Remove (wordClicked);
			} else {
				wordClicked.GetComponent<Image> ().color = Color.gray;
				answerClicked.Add (wordClicked);
				if (answerClicked.Count == 2){
					string answerClicked1 = answerClicked [0].transform.GetChild(0).GetComponent<Text>().text.ToUpper();
					string answerClicked2 = answerClicked [1].transform.GetChild(0).GetComponent<Text>().text.ToUpper();
					//Debug.Log (answerClicked1 + "/" + answerClicked2);
					CheckAnswer(answerClicked1,answerClicked2);
					}		
				}
			}
	}


	private void CheckAnswer(string answerClicked1, string answerClicked2){
		if ((answerClicked1.Equals (answer1) || answerClicked1.Equals (answer2)) &&
		   (answerClicked2.Equals (answer1) || answerClicked2.Equals (answer2))) {
			Debug.Log ("hey");
			answerClicked [0].GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 30f / 255f);
			answerClicked [1].GetComponent<Image> ().color = new Color (255f / 255f, 255f / 255f, 30f / 255f);
			QuestionCallback (true);
		} else {
			Debug.Log (answerClicked1 + "/" + answerClicked2);
			Debug.Log (answer1 + "/" + answer2);
			QuestionCallback (false);
		}
	}



	public void SelectionInit ()
	{
		correctAnswers.Clear ();
		int numberOfAnswers = 2;
		List <int> randomList = new List<int>();
		string[] temp = questionAnswer.Split ('/');
		int whileindex = 0;
		for (int i = 0; i < selectionButtons.Length; i++) {
			int randomnum = UnityEngine.Random.Range (0, 4); 
			while (randomList.Contains (randomnum)) {
				randomnum = UnityEngine.Random.Range (0, selectionButtons.Length);
				whileindex++;
			
			}
			randomList.Add (randomnum);
			string wrongChoiceGot = QuestionBuilder.GetRandomChoices ();
			selectionButtons [randomnum].transform.GetChild (0).GetComponent<Text> ().text = 
				i < numberOfAnswers ? temp [i].ToString ().ToUpper () : 
				wrongChoiceGot;
			if (i < numberOfAnswers) {
				correctAnswers.Add (selectionButtons[randomnum]);

			}
		}

		answer1 = correctAnswers [0].GetComponentInChildren<Text> ().text.ToUpper ();
		answer2 = correctAnswers [1].GetComponentInChildren<Text> ().text.ToUpper ();
	}

	public void Clear ()
	{
		answerClicked.Clear ();
		foreach (GameObject g in selectionButtons) {
			g.GetComponent<Image> ().color = new Color(94f/255,255f/255f,148f/255f);
		}
	}
}
