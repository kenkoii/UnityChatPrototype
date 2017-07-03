using UnityEngine;

public class GameController : SingletonMonoBehaviour<GameController>
{
	[SerializeField] private int playerLife = 45;
	[SerializeField] private int answerQuestionTime = 25;
	[SerializeField] private int playerGP = 0;
	[SerializeField] private int playerMaxGP = 9;
	[SerializeField] private int playerDamage = 4;

	void Start ()
	{
		GameData.Instance.modePrototype = ModeEnum.Mode1;
	}

	public void UpdateGame ()
	{
		GameData.Instance.player.playerLife = playerLife;
		GameData.Instance.answerQuestionTime = answerQuestionTime;
		GameData.Instance.player.playerGP = playerGP;
		GameData.Instance.player.playerMaxGP = playerMaxGP;
		GameData.Instance.player.playerDamage = playerDamage;
	}

	public void ResetPlayerDamage ()
	{
		GameData.Instance.player.playerDamage = playerDamage;
	}


}
