using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ChangeOrderIcon : MonoBehaviour, IQuestion
{
	private int currentRound = 1;
	private int correctAnswers;
	private int answerindex = 1;
	public List<GameObject> answerIdentifier;
	private string answerWrote;
	private bool justSkippedQuestion = false;
	private string questionAnswer = "";
	private string question = "";
	public GameObject gpText;
	private List<GameObject> selectionButtons = new List<GameObject>();
	private List<GameObject> answerButtons = new List<GameObject>();
	private QuestionController questionControl;
	private AudioController audioControl;
	public GameObject inputPrefab;
	public GameObject outputPrefab;

	public void Activate (Action<int,int> Result)
	{
		QuestionBuilder.PopulateQuestion ("SelectChangeTyping");
		currentRound = 1;
		correctAnswers = 0;
		NextQuestion ();
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;

	}

	public void NextQuestion ()
	{
		answerIdentifier.Clear();
		LoadQuestion ();
		PopulateInputHolders ();
		ShuffleAlgo ();

	}

	private void LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion ();
		questionAnswer = questionLoaded.answer;
		question = questionLoaded.question;
		gameObject.transform.GetChild (0).GetComponent<Text> ().text = question;
	}

	private void PopulateInputHolders ()
	{
		selectionButtons.Clear ();
		answerButtons.Clear();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject input = Instantiate (outputPrefab) as GameObject; 
			input.transform.SetParent (gameObject.transform.GetChild (2).
				transform.GetChild (0).GetChild (0).transform, false);
			input.name = "input" + (i + 1);
			input.GetComponent<Button> ().onClick.AddListener (() => {
				gameObject.GetComponent<ChangeOrderIcon> ().SelectionOnClick ();
			});
			selectionButtons.Add(input);

			input.transform.GetChild (0).GetComponent<Text> ().text = "";

			GameObject output = Instantiate (inputPrefab) as GameObject; 
			output.transform.SetParent (gameObject.transform.GetChild (1).
				transform.GetChild (0).GetChild (0).transform, false);
			output.name = "output" + (i + 1);
			output.GetComponent<Button> ().onClick.AddListener (() => {
				gameObject.GetComponent<ChangeOrderIcon> ().AnswerOnClick ();
			});
			answerButtons.Add(output);
			output.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
	}

	public void OnEnd ()
	{
		justSkippedQuestion = false;
		QuestionController qc = new QuestionController ();
		Clear ();
		qc.Stoptimer = true;
		answerindex = 1;
		currentRound = currentRound + 1;

		NextQuestion ();
		qc.Returner (delegate {
			qc.onFinishQuestion = true;
		}, currentRound, correctAnswers);
		if (currentRound == 4) {
			Clear ();
		}
	}

	public void OnSkipClick ()
	{
		if (!justSkippedQuestion) {
			QuestionDoneCallback (false);
			justSkippedQuestion = true;
		}
	}

	public void SelectionOnClick ()
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition (0.2f, 30.0f, 50, 0f, true);
		} else {
			answerWrote = "";
			int i = 1;
			foreach (GameObject findEmpty in answerButtons) {
				if (findEmpty.transform.GetChild (0).GetComponent<Text> ().text == "") {
					answerindex = i;
					answerButtons [(answerindex - 1)].transform.GetChild (0).
					GetComponent<Text> ().text 
					= EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text;
					EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text = "";
					break;
				} else {
					
				}
				i++;
			}

			foreach (GameObject readWrittenAnswer in answerButtons) {
				answerWrote = answerWrote + (readWrittenAnswer.transform.GetChild (0).GetComponent<Text> ().text);
			}
			answerIdentifier.Add(EventSystem.current.currentSelectedGameObject);
			if (answerWrote.Length == questionAnswer.Length) {
				if (answerWrote.ToUpper () == questionAnswer.ToUpper ()) {
					QuestionDoneCallback (true);
				} else {
					QuestionDoneCallback (false);
				}
			}
		}
	}

	public void AnswerOnClick ()
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		string answerclicked = "";
		if (EventSystem.current.currentSelectedGameObject.transform.GetChild (0).GetComponent<Text> ().text == "") {
			EventSystem.current.currentSelectedGameObject.transform.DOShakePosition (0.2f, 30.0f, 50, 0f, true);
		} else {
			for (int i = 1; i < selectionButtons.Count+ 1; i++) {
				if (EventSystem.current.currentSelectedGameObject.name == ("output" + i)) {
					answerclicked = answerButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text;
					answerButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
					answerIdentifier [i - 1].transform.GetChild (0).GetComponent<Text> ().text = 
						answerclicked;
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
			gpText.GetComponent<Text> ().text = "1 GP";
			gpText.transform.DOScale (new Vector3 (5, 5, 5), 1.0f);
			Invoke ("TweenCallBack", 1f);

		} else {
			AudioController.Instance.PlayAudio (AudioEnum.Mistake);
			Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
			param [ParamNames.AnswerWrong.ToString ()] = currentRound;
			FirebaseDatabaseComponent.Instance.SetParam (JsonConverter.DicToJsonStr (param));

			for (int i = 0; i < questionAnswer.Length; i++) {
				answerButtons [i].transform.GetChild (0).GetComponent<Text> ().text = questionAnswer [i].ToString ().ToUpper ();
				answerButtons [i].GetComponent<Image> ().color = Color.green;
			}
		}
		gameObject.transform.DOShakePosition (1.0f, 30.0f, 50, 90, true);
		QuestionController qc = new QuestionController ();
		qc.Stoptimer = false;
		Invoke ("OnEnd", 1f);
	}

	public void TweenCallBack ()
	{
		gpText.transform.DOScale (new Vector3 (1, 1, 1), 1.0f);
		gpText.GetComponent<Text> ().text = " ";
	}

	public void ShuffleAlgo ()
	{
		List<int> RandomExist = new List<int> ();
		string temp = questionAnswer;

		int letterno = 0;
		int randomnum = 0;      
		for (int z = 0; z < temp.Length; z++) {
			randomnum = UnityEngine.Random.Range (0, questionAnswer.Length);        
			int whileindex = 0;
			while (true) {
				if (whileindex > 100) {
					break;
				}
				bool index = RandomExist.Contains (randomnum);
				if (index) {
					randomnum = UnityEngine.Random.Range (0, questionAnswer.Length);
				} else {
					break;
				}
				whileindex++;
			}
			Debug.Log (letterno);
			selectionButtons [letterno].transform.GetChild (0).GetComponent<Text> ().text = 
				temp [randomnum].ToString ().ToUpper ();
			RandomExist.Add (randomnum);
			letterno = letterno + 1;
		}
	}

	public void Clear ()
	{

		answerindex = 1;
		foreach (GameObject i in selectionButtons) {
			Destroy (i);
		}
		foreach (GameObject o in answerButtons) {
			Destroy (o);
		}
		gameObject.transform.GetChild (0).GetComponent<Text> ().text = "";
	}
}
