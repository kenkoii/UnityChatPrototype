using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controls the battle */
public class BattleController : MonoBehaviour
{
	public int playerHP = 10;
	public int enemyHP = 10;
	public int prepareTime = 3;

	private int playerMaxHP = 10;
	private int enemyMaxHP = 10;

	public Slider playerHPBar;
	public Slider enemyHPBar;

	public int playerGP = 10;
	private int playerMaxGP = 10;
	public Slider playerGPBar;

	public string playerName = "Anonymous1";
	public string enemyName = "Anonymous2";

	public Text playerNameText;
	public Text playerHPText;
	public Text playerGPText;
	public Text enemyNameText;
	public Text enemyHPText;

	public Text battleResultText;

	public GameObject preBattleTimer;


	/// <summary>
	/// Delay before start of battle
	/// </summary>
	public void StartPreTimer ()
	{
		StartCoroutine (StartPreparationDelay (prepareTime));
		
	}

	IEnumerator StartPreparationDelay (int timer)
	{
		preBattleTimer.SetActive (true);

		while (timer > 0) {
			preBattleTimer.transform.Find ("ReadyTime").GetComponent<Text> ().text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}

		//start first phase
		PhaseManager.Instance.StartPhase2 ();
		preBattleTimer.SetActive (false);

	}

	void Update ()
	{
		playerNameText.text = "" + playerName;
		playerHPText.text = "" + playerHP + "/" + playerMaxHP;
		enemyNameText.text = "" + enemyName;
		enemyHPText.text = "" + enemyHP + "/" + enemyMaxHP;

		playerGPText.text = "" + playerGP + "/" + playerMaxGP;

		playerHPBar.value = playerHP;
		enemyHPBar.value = enemyHP;

		playerGPBar.value = playerGP;


	}

	public void Attack ()
	{
		if (GameManager.Instance.attackerParam [ParamNames.Damage.ToString ()] != null) {
			int damage = int.Parse (GameManager.Instance.attackerParam [ParamNames.Damage.ToString ()].ToString ());

			if (GameManager.Instance.attackerName.Equals (GameManager.Instance.playerName)) {
				playerHP -= damage;
			} else {
				enemyHP -= damage;
			}
		}
	}

	public void InitialPlayerState (int playerHP, string playerName, int playerGP)
	{
		this.playerHP = playerHP;
		this.playerName = playerName;
		this.playerGP = playerGP;
		playerMaxGP = playerGP;
		playerGPBar.maxValue = playerGP;
		playerMaxHP = playerHP;
		playerHPBar.maxValue = playerMaxHP;
	}

	public void InitialEnemyState (int enemyHP, string enemyName)
	{
		this.enemyHP = enemyHP;
		this.enemyName = enemyName;
		enemyMaxHP = enemyHP;
		enemyHPBar.maxValue = enemyMaxHP;
	}

	/// <summary>
	/// Attack enemy. 
	/// </summary>
	/// <param name="playerAction">Player action.</param>
	public void SetAttack ()
	{
		if (enemyHP > 0 && playerHP > 0) {
			Attack ();
			return;
		} 
		if (enemyHP > 0 && playerHP <= 0) {
			battleResultText.text = "LOSE";
		} else if (playerHP > 0 && enemyHP <= 0) {
			battleResultText.text = "WIN";
		} 
		battleResultText.enabled = true;

	}

	public void SetSkill (ISkill skill)
	{
		skill.Activate (this.gameObject);
		Debug.Log ("Setting Skill");
	}



	//test attack
	public void SendAttackToDatabase ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Damage.ToString ()] = 10;
		RPCWrapper.Instance.RPCWrapAttack (param);
	}
}
