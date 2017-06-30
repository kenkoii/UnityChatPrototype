using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnswerController : EnglishRoyaleElement
{
	private Sprite correct;
	private Sprite wrong;
	private Sprite empty;
	private Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();


	void Start ()
	{
//		use this to call
//		param [ParamNames.AnswerCorrect.ToString ()] = 1;
//		app.component.firebaseDatabaseComponent.SetParam(app.model.battleModel.isHost, app.component.rpcWrapperComponent.DicToJsonStr (param));


		correct = app.model.answerModel.correct;
		wrong = app.model.answerModel.wrong;
		empty = app.model.answerModel.empty;
		ResetAnswer ();
	}

	public void ValidateAnswer (bool isCorrect, int questionNumber)
	{
		if (app.model.battleModel.attackerBool.Equals (app.model.battleModel.isHost)) {
			switch (questionNumber) {
			case 1:
				SetValidateAnswer (isCorrect, delegate() {
					app.model.answerModel.playerPlaceHolder1.sprite = correct;
				}, delegate() {
					app.model.answerModel.playerPlaceHolder1.sprite = wrong;
				});
				break;
			case 2:
				SetValidateAnswer (isCorrect, delegate() {
					app.model.answerModel.playerPlaceHolder2.sprite = correct;
				}, delegate() {
					app.model.answerModel.playerPlaceHolder2.sprite = wrong;
				});
				break;
			case 3:
				SetValidateAnswer (isCorrect, delegate() {
					app.model.answerModel.playerPlaceHolder3.sprite = correct;
				}, delegate() {
					app.model.answerModel.playerPlaceHolder3.sprite = wrong;
				});
				break;

			}
		} else {
			switch (questionNumber) {
			case 1:
				SetValidateAnswer (isCorrect, delegate() {
					app.model.answerModel.enemyPlaceHolder1.sprite = correct;
				}, delegate() {
					app.model.answerModel.enemyPlaceHolder1.sprite = wrong;
				});
				break;
			case 2:
				SetValidateAnswer (isCorrect, delegate() {
					app.model.answerModel.enemyPlaceHolder2.sprite = correct;
				}, delegate() {
					app.model.answerModel.enemyPlaceHolder2.sprite = wrong;
				});
				break;
			case 3:
				SetValidateAnswer (isCorrect, delegate() {
					app.model.answerModel.enemyPlaceHolder3.sprite = correct;
				}, delegate() {
					app.model.answerModel.enemyPlaceHolder3.sprite = wrong;
				});
				break;

			}
		}
	}

	private void SetValidateAnswer (bool isCorrect, Action correct, Action wrong)
	{
		if (isCorrect) {
			correct ();
		} else {
			wrong ();
		}
	}

	public void ResetAnswer ()
	{
		
		app.model.answerModel.playerPlaceHolder1.sprite = empty;
		app.model.answerModel.playerPlaceHolder2.sprite = empty;
		app.model.answerModel.playerPlaceHolder3.sprite = empty;

		app.model.answerModel.enemyPlaceHolder1.sprite = empty;
		app.model.answerModel.enemyPlaceHolder2.sprite = empty;
		app.model.answerModel.enemyPlaceHolder3.sprite = empty;
		Debug.Log ("reset answers");
	}
}
