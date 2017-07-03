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
	public GameObject inputPrefab;
	public GameObject outputPrefab;
	private GameObject questionContainer;

	void Start(){
		questionContainer = gameObject;
	}

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
			output.GetComponent<Image> ().color = new Color(136f/255,236f/255f,246f/255f);
		}
	}

	public void OnFinishQuestion ()
	{
		TweenCallBack ();
		justSkippedQuestion = false;
		Clear ();
		QuestionController.Instance.Stoptimer = true;
		answerindex = 1;
		currentRound = currentRound + 1;

		NextQuestion ();
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
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
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		spe.DeployEffect (result, answerButtons, questionAnswer, gpText, gameObject);
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		string isCorrectParam = result ? ParamNames.AnswerCorrect.ToString () : ParamNames.AnswerWrong.ToString ();
		param [isCorrectParam] = currentRound;
		FirebaseDatabaseComponent.Instance.SetParam (JsonConverter.DicToJsonStr(param));
		QuestionController.Instance.Stoptimer = false;
		Invoke ("OnFinishQuestion", 1f);
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
