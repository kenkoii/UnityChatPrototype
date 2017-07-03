using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TypingIcon : MonoBehaviour, IQuestion
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
	private List<GameObject> answerButtons = new List<GameObject> ();
	private QuestionController questionControl;
	private AudioController audioControl;
	public GameObject inputPrefab;
	public GameObject answerContent;
	public Text questionText;
	private bool selectionIsClickable = true;

	public void Activate (Action<int,int> Result)
	{
		QuestionBuilder.PopulateQuestion ("SelectChangeTyping");
		currentRound = 1;
		correctAnswers = 0;
		NextRound ();
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}

	public void NextRound ()
	{
		answerIdentifier.Clear ();
		LoadQuestion ();
		PopulateAnswerHolder ();
		QuestionHint ();
	}

	private void QuestionHint ()
	{
		answerButtons [0].transform.GetChild (0).
		GetComponent<Text> ().text = questionAnswer [0].ToString ().ToUpper ();
		answerButtons [0].GetComponent<Button> ().enabled = false;
	}

	private void PopulateAnswerHolder ()
	{
		answerButtons.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject answerPrefab = Instantiate (inputPrefab) as GameObject; 
			answerPrefab.transform.SetParent (gameObject.transform.GetChild (1).
				transform.GetChild (0).GetChild (0).transform, false);
			answerPrefab.name = "output" + (i + 1);
			answerPrefab.GetComponent<Button> ().onClick.AddListener (() => {
				gameObject.GetComponent<TypingIcon> ().OnAnswerClick ();
			});
			answerButtons.Add (answerPrefab);

			answerPrefab.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
	}

	private void LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion ();
		questionAnswer = questionLoaded.answer;
		questionText.text = questionLoaded.question;
	}

	public void OnSelectionClick ()
	{

		if (!selectionIsClickable) {
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition (0.2f, 30.0f, 50, 0f, true);
		} else {
			answerWrote = "";
			int k = 1;
			foreach (GameObject findEmpty in answerButtons) {
				if (findEmpty.transform.GetChild (0).GetComponent<Text> ().text == "") {
					answerindex = k;
					answerButtons [(answerindex - 1)].transform.GetChild (0).
					GetComponent<Text> ().text 
					= EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text;
					break;
				} else {

				}
				k++;
			}
			foreach (GameObject readWrittenAnswer in answerButtons) {
				answerWrote = answerWrote + (readWrittenAnswer.transform.GetChild (0).GetComponent<Text> ().text);
			}
			answerIdentifier.Add (EventSystem.current.currentSelectedGameObject);
			if (answerWrote.Length == questionAnswer.Length) {

				if (answerWrote.ToUpper () == questionAnswer.ToUpper ()) {
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
			AudioController.Instance.PlayAudio (AudioEnum.Correct);
			correctAnswers = correctAnswers + 1;
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerCorrect.ToString ()] = currentRound;
			FirebaseDatabaseComponent.Instance.SetParam (JsonConverter.DicToJsonStr (param));

			for (int i = 0; i < questionAnswer.Length; i++) {
				GameObject ballInstantiated = Resources.Load ("Prefabs/scoreBall") as GameObject;
				Instantiate (ballInstantiated, 
					answerButtons [i].transform.position, 
					Quaternion.identity);
			}
			gPtext.GetComponent<Text> ().text = "1 GP";
			gPtext.transform.DOScale (new Vector3 (5, 5, 5), 1.0f);
			Invoke ("TweenCallBack", 1f);
		} else {
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerWrong.ToString ()] = currentRound;
			FirebaseDatabaseComponent.Instance.SetParam (JsonConverter.DicToJsonStr (param));

			AudioController.Instance.PlayAudio (AudioEnum.Mistake);
			for (int i = 0; i < questionAnswer.Length; i++) {
				answerButtons [i].transform.GetChild (0).GetComponent<Text> ().text = questionAnswer [i].ToString ().ToUpper ();
				answerButtons [i].GetComponent<Image> ().color = Color.green;
			}
		}
	
		gameObject.transform.DOShakePosition (0.2f, 30.0f, 50, 0f, true);

		selectionIsClickable = false;
		QuestionController qc = new QuestionController ();
		qc.Stoptimer = false;
		Invoke ("OnEnd", 1f);

	}

	public void OnSkipClick ()
	{
		if (!hasSkippedQuestion) {
			QuestionDoneCallback (false);
			hasSkippedQuestion = true;
		}
	}

	public void TweenCallBack ()
	{
		gPtext.transform.DOScale (new Vector3 (1, 1, 1), 1.0f);
		gPtext.GetComponent<Text> ().text = " ";
	}

	public void OnEnd ()
	{
		selectionIsClickable = false;
		QuestionController qc = new QuestionController ();
		Clear ();
		answerindex = 1;
		qc.Stoptimer = true;
		currentRound = currentRound + 1;
		NextRound ();
		qc.Returner (delegate {
			qc.onFinishQuestion = true;
		}, currentRound, correctAnswers);
		selectionIsClickable = true;
	}

	public void OnAnswerClick ()
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition (0.2f, 30.0f, 50, 0f, true);
		} else {
			EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
	}

	public void Clear ()
	{
		answerindex = 1;
		foreach (GameObject o in answerButtons) {
			Destroy (o);
		}

	}
}
