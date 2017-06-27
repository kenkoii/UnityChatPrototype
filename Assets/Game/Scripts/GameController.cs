using UnityEngine;

public class GameController : EnglishRoyaleElement
{
	void Start ()
	{
		app.model.battleModel.modePrototype = ModeEnum.Mode1;
	}

	public void UpdateGame ()
	{
		app.model.battleModel.playerLife = 45;
		app.model.battleModel.answerQuestionTime = 25;
		app.model.battleModel.playerGP = 0;
		app.model.battleModel.playerMaxGP = 9;
		app.model.battleModel.playerDamage = 5;
	}

	public void ResetPlayerDamage ()
	{
		app.model.battleModel.playerDamage = 5;
	}


}
