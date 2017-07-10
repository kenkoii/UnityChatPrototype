using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class QuestionSystemBase : MonoBehaviour{

	protected int currentRound = 1;
	protected int correctAnswers;
	protected int answerIndex = 1;
	protected string answerWrote;
	protected bool hasSkippedQuestion = false;
	protected string questionAnswer = "";
	protected List<GameObject> answerButtons = new List<GameObject> ();
	protected List<GameObject> answerGameObject = new List<GameObject>();
	protected GameObject[] answerIdentifier = new GameObject[30];

	protected void LoadQuestion ()
	{
		Question questionLoaded = QuestionBuilder.GetQuestion ();
		questionAnswer = questionLoaded.answer;
		string question = questionLoaded.question;
		gameObject.transform.GetChild (0).GetComponent<Text> ().text = question;
	}

	protected void PopulateAnswerHolder (GameObject g, GameObject inputPrefab, GameObject answerContent)
	{
		answerButtons.Clear ();
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject answerPrefab = Instantiate (inputPrefab) as GameObject; 
			answerPrefab.transform.SetParent (answerContent.transform, false);
			answerPrefab.GetComponent<Button> ().onClick.AddListener (() => {
					OnAnswerClick (answerPrefab.GetComponent<Button> ());
			});
			answerPrefab.name = "input" + (i+1);
			answerButtons.Add (answerPrefab);
			answerPrefab.transform.GetChild (0).GetComponent<Text> ().text = "";
			answerPrefab.GetComponent<Image> ().color = new Color (136f / 255, 236f / 255f, 246f / 255f);
		}
	}

	protected void OnAnswerClick (Button answerButton)
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		string answerclicked = "";
		if (string.IsNullOrEmpty (answerButton.transform.GetChild (0).GetComponent<Text> ().text)) {
			TweenController.TweenShakePosition (answerButton.transform, 0.5f, 15.0f, 50, 90f);
		} else {
			Debug.Log (answerButton.name);
			for (int i = 1; i < 12 + 1; i++) {
				if (answerButton.name.Equals ("input" + i)) {
					
					answerclicked = answerButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text;
					answerButtons [i - 1].transform.GetChild (0).GetComponent<Text> ().text = "";
					answerIdentifier [i].transform.GetChild (0).GetComponent<Text> ().text = answerclicked;
				}
			}
			CheckAnswerHolder ();

		}
	}
	protected void CheckAnswer (bool result)
	{
		QuestionSpecialEffects spe = new QuestionSpecialEffects ();
		spe.DeployEffect (result, answerButtons, questionAnswer, gameObject);

		UpdateFirebaseAnswerModel (result);
		hasSkippedQuestion = true;
		QuestionController.Instance.Stoptimer = false;
		Invoke ("OnFinishQuestion", 1f);
	}

	protected void UpdateFirebaseAnswerModel(bool isCorrect){
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		string isCorrectParam = isCorrect ? ParamNames.AnswerCorrect.ToString () : ParamNames.AnswerWrong.ToString ();
		param [isCorrectParam] = currentRound;
		FirebaseDatabaseComponent.Instance.SetAnswerParam (new AnswerModel(JsonConverter.DicToJsonStr (param).ToString()));
	}

	protected void CheckAnswerHolder ()
	{
		for (int j = 0; j < questionAnswer.Length; j++) {
			GameObject findEmpty = answerButtons [j].transform.GetChild (0).gameObject;
			if (string.IsNullOrEmpty (findEmpty.GetComponent<Text> ().text)) {
				answerIndex = j +1;
				break;
			}
		}

	}

	public void OnFinishQuestion ()
	{
		//TweenCallBack ();
		hasSkippedQuestion = false;
		QuestionController.Instance.Stoptimer = true;
		ClearAnswerList ();
		answerIndex = 1;
		currentRound += 1;
		//NextQuestion ();
		QuestionController.Instance.Returner (delegate {
			QuestionController.Instance.onFinishQuestion = true;
		}, currentRound, correctAnswers);
	}

	public void ClearAnswerList ()
	{
		if (answerButtons.Count > 0) {
			answerIndex = 1;
			foreach (GameObject o in answerButtons) {
				Destroy (o);
			}
		}
	}
	public void OnSkipClick ()
	{
		if (!hasSkippedQuestion) {
			CheckAnswer (false );
			hasSkippedQuestion = true;
		}
	}
	public void OnSelectionClick (Button letterButton)
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);

		if (string.IsNullOrEmpty (letterButton.transform.GetChild (0).GetComponent<Text> ().text)) {
			TweenController.TweenShakePosition (letterButton.transform, 1.0f, 30.0f, 50, 90f);
		} else {
			answerIdentifier[answerIndex] = letterButton.gameObject;
			answerWrote = "";
			answerButtons [(answerIndex - 1)].GetComponentInChildren<Text>().text 
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
}
