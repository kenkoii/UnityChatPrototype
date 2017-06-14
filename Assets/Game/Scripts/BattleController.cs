﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

	public GameObject battleResultText;
	private Text cachedBattleResult;

	private int attackCounter = 0;


	/// <summary>
	/// Delay before start of battle
	/// </summary>
	public void StartPreTimer ()
	{
		StartCoroutine (StartPreparationDelay (prepareTime));
		cachedBattleResult = battleResultText.GetComponent<Text>();
	}

	IEnumerator StartPreparationDelay (int timer)
	{
		
		GameTimer.Instance.ToggleTimer (true);
		while (timer > 0) {
			GameTimer.Instance.gameTimerText.text = "" + timer;
			timer--;
			yield return new WaitForSeconds (1);
		}

		//start first phase
		PhaseManager.Instance.StartPhase1 ();
		GameTimer.Instance.ToggleTimer (false);

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

		if (playerHP < 0) {
			playerHP = 0;
		}

		if (enemyHP < 0) {
			enemyHP = 0;
		}
	}
		

	public void Attack ()
	{
		
		if (StatusManager.Instance.attackerParam [ParamNames.Damage.ToString ()] != null) {
			int damage = int.Parse (StatusManager.Instance.attackerParam [ParamNames.Damage.ToString ()].ToString ());

			if (StatusManager.Instance.attackerName.Equals (StatusManager.Instance.playerName)) {
				
					enemyHP -= damage;

			} else {
					playerHP -= damage;

			}
		}
		//reset effects done by skill
		StatusManager.Instance.ResetPlayerStats();
	}

	public void CheckBattleStatus ()
	{
		StartCoroutine (CheckbattlestatusDelay (1));
	}

	/// <summary>
	/// Add timer for animation effects
	/// </summary>
	/// <returns>The delay.</returns>
	/// <param name="timer">Timer.</param>
	IEnumerator CheckbattlestatusDelay (int timer)
	{
		
		yield return new WaitForSeconds (timer);
		if (enemyHP <= 0 || playerHP <= 0) {
			if (enemyHP > 0 && playerHP <= 0) {
				cachedBattleResult.text = "LOSE";
			} else if (playerHP > 0 && enemyHP <= 0) {
				cachedBattleResult.text = "WIN";
			} else {
				cachedBattleResult.text = "DRAW";
			}

			battleResultText.SetActive (true);


		} else {
			if (FirebaseDatabaseFacade.Instance.isHost) {
				FirebaseDatabaseFacade.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0);
			}
			PhaseManager.Instance.StartPhase1 ();
		}

	}

	public void ReturnToLobby(){
		SceneManager.LoadScene ("scene1");
	}

	public void SetPlayerGP(int playerGP){
		Debug.Log ("GP EARNED" + playerGP);
		this.playerGP += playerGP;
	}

	public void InitialPlayerState (int playerHP, string playerName, int playerGP)
	{
		this.playerHP = playerHP;
		this.playerName = playerName;
		this.playerGP = playerGP;
		playerGPBar.maxValue = StatusManager.Instance.playerMaxGP;
		playerMaxHP = playerHP;
		playerHPBar.maxValue = playerMaxHP;
		playerMaxGP = StatusManager.Instance.playerMaxGP;
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
		Attack ();
	}

	public void SetSkill (ISkill skill)
	{
		skill.Activate (this.gameObject);
	}



	//test attack
	public void SendAttackToDatabase ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Damage.ToString ()] = StatusManager.Instance.playerDamage;
		RPCWrapper.Instance.RPCWrapAttack (param);
	}
}
