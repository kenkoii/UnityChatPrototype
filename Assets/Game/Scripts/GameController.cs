using UnityEngine;

public class GameController : EnglishRoyaleElement
{
	[SerializeField] private int playerLife = 45;
	[SerializeField] private int answerQuestionTime = 25;
	[SerializeField] private int playerGP = 0;
	[SerializeField] private int playerMaxGP = 9;
	[SerializeField] private int playerDamage = 4;

	void Start ()
	{
		app.model.battleModel.modePrototype = ModeEnum.Mode1;
	}

	public void UpdateGame ()
	{
		app.model.battleModel.playerLife = playerLife;
		app.model.battleModel.answerQuestionTime = answerQuestionTime;
		app.model.battleModel.playerGP = playerGP;
		app.model.battleModel.playerMaxGP = playerMaxGP;
		app.model.battleModel.playerDamage = playerDamage;
	}

	public void ResetPlayerDamage ()
	{
		app.model.battleModel.playerDamage = playerDamage;
	}


}
