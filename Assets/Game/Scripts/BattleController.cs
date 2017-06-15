using System.Collections;
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
		cachedBattleResult = battleResultText.GetComponent<Text> ();
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
				if (MyGlobalVariables.Instance.modePrototype == ModeEnum.Mode2) {
					FirebaseDatabaseFacade.Instance.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
				} else {
					FirebaseDatabaseFacade.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0);
				}
			}
			PhaseManager.Instance.StartPhase1 ();
		}

	}

	public void ReturnToLobby ()
	{
		SceneManager.LoadScene ("scene1");
	}

	public void SetPlayerGP (int playerGP)
	{
		Debug.Log ("GP EARNED" + playerGP);
		this.playerGP += playerGP;
	}

	public void InitialPlayerState (int playerHP, string playerName, int playerGP)
	{
		this.playerHP = playerHP;
		this.playerName = playerName;
		this.playerGP = playerGP;
		playerGPBar.maxValue = MyGlobalVariables.Instance.playerMaxGP;
		playerMaxHP = playerHP;
		playerHPBar.maxValue = playerMaxHP;
		playerMaxGP = MyGlobalVariables.Instance.playerMaxGP;
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

	public void SetAttackMode2 (Dictionary<string, Dictionary<string, object>> currentParam)
	{
		int attackOrder = 0;

		if (MyGlobalVariables.Instance.hAnswer > MyGlobalVariables.Instance.vAnswer) {
			attackOrder = 0;
		} else if (MyGlobalVariables.Instance.hAnswer < MyGlobalVariables.Instance.vAnswer) {
			attackOrder = 1;
		} else {
			if (MyGlobalVariables.Instance.hTime > MyGlobalVariables.Instance.vTime) {
				attackOrder = 0;
			} else if (MyGlobalVariables.Instance.hTime < MyGlobalVariables.Instance.vTime) {
				attackOrder = 1;
			} else {
				attackOrder = 2;
			}
		}
		StartCoroutine (AttackMode2 (attackOrder, currentParam));
	}

	IEnumerator AttackMode2 (int attackOrder, Dictionary<string, Dictionary<string, object>> currentParam)
	{
		List<string> username = new List<string> ();
		List<Dictionary<string, System.Object>> param = new List<Dictionary<string, object>> ();

		foreach (KeyValuePair<string, Dictionary<string, System.Object>> newParam in currentParam) {
			username.Add (newParam.Key.ToString ());
			param.Add (newParam.Value);
		}


		//change order of list if host or visitor
		if (FirebaseDatabaseFacade.Instance.isHost) {
			if (username [0] != MyGlobalVariables.Instance.playerName) {
				string tempName = username [1];
				Dictionary<string, System.Object> tempParam = param [1];

				username.Insert (1, username [0]);
				username.Insert (0, tempName);
				param.Insert (1, param [0]);
				param.Insert (0, tempParam);
			}
		} else {
			if (username [1] != MyGlobalVariables.Instance.playerName) {
				string tempName = username [0];
				Dictionary<string, System.Object> tempParam = param [0];

				username.Insert (0, username [1]);
				username.Insert (1, tempName);
				param.Insert (0, param [1]);
				param.Insert (1, tempParam);
			}

		}


		switch (attackOrder) {
		case 0:
			AttackParameter (username [0], param [0]);
			CheckMode2BattleStatus (false);
			yield return new WaitForSeconds (1);
			AttackParameter (username [1], param [1]);
			CheckMode2BattleStatus (true);
			break;
		case 1:
			AttackParameter (username [1], param [1]);
			CheckMode2BattleStatus (false);
			yield return new WaitForSeconds (1);
			AttackParameter (username [0], param [0]);
			CheckMode2BattleStatus (true);
			break;
		case 2:
			AttackParameter (username [0], param [0]);
			AttackParameter (username [1], param [1]);
			CheckMode2BattleStatus (true);
			break;
		}
			
		//reset effects done by skill
		MyGlobalVariables.Instance.ResetPlayerStats ();
	
	}

	public void CheckMode2BattleStatus (bool secondCheck)
	{
		
		StartCoroutine (CheckMode2BattleDelay (secondCheck));
	}

	IEnumerator CheckMode2BattleDelay (bool secondCheck)
	{
		if (enemyHP <= 0 || playerHP <= 0) {
			if (enemyHP > 0 && playerHP <= 0) {
				cachedBattleResult.text = "LOSE";
			} else if (playerHP > 0 && enemyHP <= 0) {
				cachedBattleResult.text = "WIN";
			} else {
				cachedBattleResult.text = "DRAW";
			}

			battleResultText.SetActive (true);
			StopAllCoroutines ();

		} else {
			if (secondCheck) {
				if (FirebaseDatabaseFacade.Instance.isHost) {
					if (MyGlobalVariables.Instance.modePrototype == ModeEnum.Mode2) {
						FirebaseDatabaseFacade.Instance.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
					} else {
						FirebaseDatabaseFacade.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0);
					}
				}
				yield return new WaitForSeconds (1);
				PhaseManager.Instance.StartPhase1 ();
			}
		}
	}

	private void AttackParameter (string attackerName, Dictionary<string, System.Object> attackerParam)
	{
		if (attackerParam [ParamNames.Damage.ToString ()] != null) {
			int damage = int.Parse (attackerParam [ParamNames.Damage.ToString ()].ToString ());
		
			if (attackerName.Equals (MyGlobalVariables.Instance.playerName)) {
		
				enemyHP -= damage;
		
			} else {
				playerHP -= damage;
		
			}
		}

	}

	private void Attack ()
	{
		AttackParameter (MyGlobalVariables.Instance.attackerName, MyGlobalVariables.Instance.attackerParam);
		//reset effects done by skill
		MyGlobalVariables.Instance.ResetPlayerStats ();
	}
		

	public void SetSkill (ISkill skill)
	{
		skill.Activate (this.gameObject);
	}
		
	//send attack to firebase
	public void SendAttackToDatabase ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Damage.ToString ()] = MyGlobalVariables.Instance.playerDamage;
		RPCWrapper.Instance.RPCWrapAttack (param);
	}
}
