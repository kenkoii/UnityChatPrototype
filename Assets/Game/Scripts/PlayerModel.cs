public class PlayerModel
{
	public string playerName;

	public int playerLife;

	public int playerGP;

	public int playerMaxGP;

	public float playerDamage;

	public PlayerModel (string playerName, int playerLife, int playerGP, int playerMaxGP, float playerDamage)
	{
		this.playerName = playerName;
		this.playerLife = playerLife;
		this.playerGP = playerGP;
		this.playerMaxGP = playerMaxGP;
		this.playerDamage = playerDamage;
	}

}
