using UnityEngine;

public class GameController : SingletonMonoBehaviour<GameController>
{
	void Start ()
	{
		MyGlobalVariables.Instance.modePrototype = ModeEnum.Mode1;
		Debug.Log (MyGlobalVariables.Instance.modePrototype);
	}

	public void UpdateGame ()
	{
		if (MyGlobalVariables.Instance.modePrototype == ModeEnum.Mode1) {
			MyGlobalVariables.Instance.playerLife = 30;
			MyGlobalVariables.Instance.answerQuestionTime = 20;
		}
		if (MyGlobalVariables.Instance.modePrototype == ModeEnum.Mode2) {
			MyGlobalVariables.Instance.playerLife = 45;
			MyGlobalVariables.Instance.answerQuestionTime = 20;

		} else if (MyGlobalVariables.Instance.modePrototype == ModeEnum.Mode3) {
			MyGlobalVariables.Instance.playerLife = 30;
			MyGlobalVariables.Instance.answerQuestionTime = 15;
		} else if (MyGlobalVariables.Instance.modePrototype == ModeEnum.Mode4) {
			MyGlobalVariables.Instance.playerLife = 30;
			MyGlobalVariables.Instance.answerQuestionTime = 15;
		} 

		MyGlobalVariables.Instance.playerGP = 0;
		MyGlobalVariables.Instance.playerMaxGP = 9;
		MyGlobalVariables.Instance.playerDamage = 5;
	}






}
