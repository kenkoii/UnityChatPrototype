using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AnswerIndicatorController : SingletonMonoBehaviour<AnswerIndicatorController>, IRPCDicObserver
{
	public Sprite correct;
	public Sprite wrong;
	public Sprite empty;
	public Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
	public Image[] playerPlaceHolder;
	public Image[] enemyPlaceHolder;

	void Start ()
	{
		ResetAnswer ();
	}

	public void OnNotify(Dictionary<string, System.Object> rpcReceive){
//		SetAnswerParameter (string answerParameter)

	}

	public void SetAnswerParameter (string answerParameter)
	{

		Dictionary<string, System.Object> answerResult = JsonConverter.JsonStrToDic (answerParameter);

		foreach (KeyValuePair<string, System.Object> answer in answerResult) {

			if (answer.Key == ParamNames.AnswerCorrect.ToString ()) {
				ValidateAnswer (true, int.Parse (answer.Value.ToString ()));
			} else {
				ValidateAnswer (false, int.Parse (answer.Value.ToString ()));

			}
		}
	}

	public void ValidateAnswer (bool isCorrect, int questionNumber)
	{
		Debug.Log ("AnswerCorrect: " + isCorrect);
		Debug.Log ("Question No : " + questionNumber);
		if (GameData.Instance.attackerBool.Equals (GameData.Instance.isHost)) {
			
			SetValidateAnswer (isCorrect, delegate(Sprite result) {
				playerPlaceHolder [questionNumber - 1].sprite = result;
				playerPlaceHolder[questionNumber - 1].color = isCorrect ? new Color(237f/255f,232f/255f,54f/255f):
					playerPlaceHolder[questionNumber - 1].color = new Color(239/255f,87f/255f,86f/255f);
			});
		} else {
			SetValidateAnswer (isCorrect, delegate(Sprite result) {
				enemyPlaceHolder [questionNumber - 1].sprite = result;
				enemyPlaceHolder[questionNumber - 1].color = isCorrect ?new Color(237f/255f,232f/255f,54f/255f):
					enemyPlaceHolder[questionNumber - 1].color = new Color(239/255f,87f/255f,86f/255f);
			});
		}
	}

	private void SetValidateAnswer (bool isCorrect, Action<Sprite> action)
	{
		if (isCorrect) {
			action (correct);
		} else {
			action (wrong);
		}
	}

	public void ResetAnswer ()
	{
		for (int i = 0; i < playerPlaceHolder.Length; i++) {
			playerPlaceHolder [i].sprite = empty;
			enemyPlaceHolder [i].sprite = empty;
		}
		Debug.Log ("reset answers");
	}
}
