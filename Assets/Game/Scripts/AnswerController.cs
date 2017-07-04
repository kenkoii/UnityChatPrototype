using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AnswerController : SingletonMonoBehaviour<AnswerController>
{
	public Sprite correct;
	public Sprite wrong;
	public Sprite empty;
	public Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
	public Image playerPlaceHolder1;
	public Image playerPlaceHolder2;
	public Image playerPlaceHolder3;

	public Image enemyPlaceHolder1;
	public Image enemyPlaceHolder2;
	public Image enemyPlaceHolder3;


	void Start ()
	{
		ResetAnswer ();
	}

	public void SetPlayerAnswerParameter (string answerParameter)
	{


		BattleStatus answerResult = JsonConverter.JsonStrToDic(answerParameter);

		foreach (SkillParameter skill in skillResult.skillList) {

			if (skill.skillKey == ParamNames.Damage.ToString ()) {
				GameData.Instance.player.playerDamage += skill.skillValue;
			}

			if (skill.skillKey == ParamNames.Recover.ToString ()) {
				BattleController.Instance.playerHP += skill.skillValue;
			}
		}
	}

	public void ValidateAnswer (bool isCorrect, int questionNumber)
	{
		Debug.Log ("Question No : "+questionNumber);
		if (GameData.Instance.attackerBool.Equals (GameData.Instance.isHost)) {
			switch (questionNumber) {
			case 1:
				SetValidateAnswer (isCorrect, playerPlaceHolder1.sprite);
				break;
			case 2:
				SetValidateAnswer (isCorrect, playerPlaceHolder2.sprite);
				break;
			case 3:
				SetValidateAnswer (isCorrect, playerPlaceHolder3.sprite);
				break;

			}
		} else {
			switch (questionNumber) {
			case 1:
				SetValidateAnswer (isCorrect, enemyPlaceHolder1.sprite);
				break;
			case 2:
				SetValidateAnswer (isCorrect, enemyPlaceHolder2.sprite);
				break;
			case 3:
				SetValidateAnswer (isCorrect, enemyPlaceHolder3.sprite);
				break;

			}
		}
	}

	private void SetValidateAnswer (bool isCorrect, Sprite answerResult)
	{
		if (isCorrect) {
			answerResult = correct;
		} else {
			answerResult = wrong;
		}
	}

	public void ResetAnswer ()
	{
		
		playerPlaceHolder1.sprite = empty;
		playerPlaceHolder2.sprite = empty;
		playerPlaceHolder3.sprite = empty;

		enemyPlaceHolder1.sprite = empty;
		enemyPlaceHolder2.sprite = empty;
		enemyPlaceHolder3.sprite = empty;
		Debug.Log ("reset answers");
	}
}
