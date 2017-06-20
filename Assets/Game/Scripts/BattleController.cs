using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Controls the battle */
public class BattleController : EnglishRoyaleElement
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
	private bool stoptimer = false;
	private int timeLeft;

	public GameObject battleResultText;
	private Text cachedBattleResult;

	private int attackCounter = 0;


	/// <summary>
	/// Delay before start of battle
	/// </summary>
	public void StartPreTimer ()
	{
		app.controller.cameraWorksController.StartIntroCamera ();
		cachedBattleResult = battleResultText.GetComponent<Text> ();
		timeLeft = 3;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			app.view.gameTimerView.ToggleTimer (true);
			if (timeLeft > 0) {
				app.view.gameTimerView.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			Debug.Log ("hello");
			app.component.phaseManagerComponent.StartPhase1 ();
			app.view.gameTimerView.ToggleTimer (false);
			stoptimer = false;
			CancelInvoke ("StartTimer");
		}
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
			if (app.component.firebaseDatabaseComponent.isHost) {
				if (app.model.battleModel.modePrototype == ModeEnum.Mode2 || app.model.battleModel.modePrototype == ModeEnum.Mode3) {
					app.component.firebaseDatabaseComponent.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
				}  else if (app.model.battleModel.modePrototype == ModeEnum.Mode4) {
					
					app.component.firebaseDatabaseComponent.UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);

				}else {
					app.component.firebaseDatabaseComponent.UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0);
				}
			}
			Debug.Log ("hello");
			app.component.phaseManagerComponent.StartPhase1 ();
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
		playerGPBar.maxValue = app.model.battleModel.playerMaxGP;
		playerMaxHP = playerHP;
		playerHPBar.maxValue = playerMaxHP;
		playerMaxGP = app.model.battleModel.playerMaxGP;
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

		StartCoroutine (AttackMode2 (currentParam));
	}

	IEnumerator AttackMode2 (Dictionary<string, Dictionary<string, object>> currentParam)
	{
		List<string> username = new List<string> ();
		List<Dictionary<string, System.Object>> param = new List<Dictionary<string, object>> ();

		foreach (KeyValuePair<string, Dictionary<string, System.Object>> newParam in currentParam) {
			username.Add (newParam.Key.ToString ());
			param.Add (newParam.Value);
		}


		//change order of list if host or visitor
		if (app.component.firebaseDatabaseComponent.isHost) {
			if (username [0] != app.model.battleModel.playerName) {
				string tempName = username [1];
				Dictionary<string, System.Object> tempParam = param [1];

				username.Insert (1, username [0]);
				username.Insert (0, tempName);
				param.Insert (1, param [0]);
				param.Insert (0, tempParam);
			}
		} else {
			if (username [1] != app.model.battleModel.playerName) {
				string tempName = username [0];
				Dictionary<string, System.Object> tempParam = param [0];

				username.Insert (0, username [1]);
				username.Insert (1, tempName);
				param.Insert (0, param [1]);
				param.Insert (1, tempParam);
			}

		}

		Debug.Log ("HOST IS" +username[0]);

		int attackOrder = 0;

		if (app.model.battleModel.hAnswer > app.model.battleModel.vAnswer) {
			attackOrder = 0;
		} else if (app.model.battleModel.hAnswer < app.model.battleModel.vAnswer) {
			attackOrder = 1;
		} else {
			if (app.model.battleModel.hTime > app.model.battleModel.vTime) {
				attackOrder = 0;
			} else if (app.model.battleModel.hTime < app.model.battleModel.vTime) {
				attackOrder = 1;
			} else {
				attackOrder = 2;
			}
		}

		//fix this soon very redundant!!!!!!!!!!!!!!!!
		switch (attackOrder) {
		case 0:
			Debug.Log ("player1 first attack");

			AttackParameter (username [0], param [0]);
			yield return new WaitForSeconds (2);
			AttackParameter (username [1], param [1]);

			break;
		case 1:
			Debug.Log ("player2 first attack");

			AttackParameter (username [1], param [1]);
			yield return new WaitForSeconds (2);
			AttackParameter (username [0], param [0]);

			break;
		case 2:
			Debug.Log ("same attack");

			AttackParameter (username [0], param [0], true);
			AttackParameter (username [1], param [1], true);
			StartCoroutine( StartAttackSequence (3));
			break;
		}
			

	
	}



	public void CheckMode2BattleStatus (bool secondCheck)
	{
		
		StartCoroutine (CheckMode2BattleDelay (secondCheck));
	}

	private void AnimationWinLose (string param1, string param2, string param3, AudioEnum param4)
	{
		
		app.controller.characterAnimationController.SetTriggerAnim (true, param1);
		app.controller.characterAnimationController.SetTriggerAnim (false, param2);
		cachedBattleResult.text = param3;
		app.controller.audioController.PlayAudio (param4);
	}

	IEnumerator CheckMode2BattleDelay (bool secondCheck)
	{
		if (enemyHP <= 0 || playerHP <= 0) {
			if (enemyHP > 0 && playerHP <= 0) {
				AnimationWinLose ("lose", "win", "LOSE", AudioEnum.Lose);

			} else if (playerHP > 0 && enemyHP <= 0) {
				AnimationWinLose ("win", "lose", "WIN", AudioEnum.Win);

			} else {
				AnimationWinLose ("lose", "lose", "DRAW", AudioEnum.Lose);
			
			}

			battleResultText.SetActive (true);
			StopAllCoroutines ();

		} else {
			if (secondCheck) {
				if (app.component.firebaseDatabaseComponent.isHost) {
					if (app.model.battleModel.modePrototype == ModeEnum.Mode2 || app.model.battleModel.modePrototype == ModeEnum.Mode3) {
						app.component.firebaseDatabaseComponent.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
					} else if (app.model.battleModel.modePrototype == ModeEnum.Mode4) {
						app.component.firebaseDatabaseComponent.UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);

					}else {
						app.component.firebaseDatabaseComponent.UpdateBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0);
					}
				}
				yield return new WaitForSeconds (3);
				Debug.Log ("hello");
				app.component.phaseManagerComponent.StartPhase1 ();

			}
		}
	}

	private void AttackParameter (string attackerName, Dictionary<string, System.Object> attackerParam, bool sameAttack = false)
	{
		if (attackerParam [ParamNames.Damage.ToString ()] != null) {
			int damage = int.Parse (attackerParam [ParamNames.Damage.ToString ()].ToString ());
		
			if (attackerName.Equals (app.model.battleModel.playerName)) {
		
				enemyHP -= damage;
				if (sameAttack == false) {
					StartCoroutine (StartAttackSequence (1));
				}
		
			} else {
				playerHP -= damage;
				if (sameAttack == false) {
					StartCoroutine (StartAttackSequence (2));
				}
			}
		}

	}

	IEnumerator StartAttackSequence(int sequenceType){

		switch (sequenceType) {
		case 1:
			app.controller.characterAnimationController.SetTriggerAnim (true, "attack");
			app.controller.audioController.PlayAudio (AudioEnum.Attack);
			yield return new WaitForSeconds (0.5f);
			app.controller.characterAnimationController.SetTriggerAnim (false, "hit");
			app.controller.audioController.PlayAudio (AudioEnum.Hit);
			CheckMode2BattleStatus (false);
			break;
		case 2:
			app.controller.characterAnimationController.SetTriggerAnim (false, "attack");
			app.controller.audioController.PlayAudio (AudioEnum.Attack);
			yield return new WaitForSeconds (0.5f);
			app.controller.characterAnimationController.SetTriggerAnim (true, "hit");
			app.controller.audioController.PlayAudio (AudioEnum.Hit);
			CheckMode2BattleStatus (true);
			break;
		case 3:
			app.controller.characterAnimationController.SetTriggerAnim (true, "attack");
			app.controller.audioController.PlayAudio (AudioEnum.Attack);
			app.controller.characterAnimationController.SetTriggerAnim (false, "attack");
			app.controller.audioController.PlayAudio (AudioEnum.Attack);
			yield return new WaitForSeconds (0.5f);
			app.controller.characterAnimationController.SetTriggerAnim (false, "hit");
			app.controller.audioController.PlayAudio (AudioEnum.Hit);
			app.controller.characterAnimationController.SetTriggerAnim (true, "hit");
			app.controller.audioController.PlayAudio (AudioEnum.Hit);
			CheckMode2BattleStatus (true);
			break;
		
		}

		//reset effects done by skill
		app.model.battleModel.ResetPlayerStats();
		Debug.Log ("player damage reset! now damage is: " + app.model.battleModel.playerDamage);
		
	}



	private void Attack ()
	{
		AttackParameter (app.model.battleModel.attackerName, app.model.battleModel.attackerParam);
		//reset effects done by skill
		app.model.battleModel.ResetPlayerStats ();
	}


	public void SetSkill (ISkill skill)
	{
		skill.Activate (this.gameObject);
	}
		
	//send attack to firebase
	public void SendAttackToDatabase ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Damage.ToString ()] = app.model.battleModel.playerDamage;
		app.component.rpcWrapperComponent.RPCWrapAttack (param);
	}
}
