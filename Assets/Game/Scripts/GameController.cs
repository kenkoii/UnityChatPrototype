using UnityEngine;

public class GameController : EnglishRoyaleElement
{
	void Start ()
	{
		app.model.battleModel.modePrototype = ModeEnum.Mode2;
	}

	public void UpdateGame ()
	{
		if (app.model.battleModel.modePrototype == ModeEnum.Mode1) {
			app.model.battleModel.playerLife = 30;
			app.model.battleModel.answerQuestionTime = 20;
		}
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			app.model.battleModel.playerLife = 45;
			app.model.battleModel.answerQuestionTime = 20;

		} else if (app.model.battleModel.modePrototype == ModeEnum.Mode3) {
			app.model.battleModel.playerLife = 30;
			app.model.battleModel.answerQuestionTime = 15;
		} else if (app.model.battleModel.modePrototype == ModeEnum.Mode4) {
			app.model.battleModel.playerLife = 30;
			app.model.battleModel.answerQuestionTime = 15;
		} 

		app.model.battleModel.playerGP = 0;
		app.model.battleModel.playerMaxGP = 9;
		app.model.battleModel.playerDamage = 5;
	}






}
