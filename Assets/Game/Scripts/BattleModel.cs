using System.Collections.Generic;

public class BattleModel : EnglishRoyaleElement
{

	public string playerName{ get; set; }

	public int playerLife{ get; set; }

	public int playerGP{ get; set; }

	public int playerMaxGP{ get; set; }

	public float playerDamage{ get; set; }

	public bool isPlayerVisitor{ get; set; }

	public string attackerName{ get; set; }

	public string battleState{ get; set; }

	public int battleCount{ get; set; }

	public ModeEnum modePrototype { get; set; }

	public Dictionary<string, System.Object> attackerParam{ get; set; }


	public int hAnswer{ get; set; }

	public int hTime{ get; set; }

	public int vAnswer{ get; set; }

	public int vTime{ get; set; }

	public int answerQuestionTime{ get; set; }

	public int gpEarned{ get; set; }


	public void ResetPlayerStats ()
	{
		playerDamage = 5;
	}
}
