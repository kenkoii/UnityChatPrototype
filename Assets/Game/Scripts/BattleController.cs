using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Controls the battle */
public class BattleController : EnglishRoyaleElement
{
	public Text skill1Name;
	public Text skill1GpCost;

	public Text skill2Name;
	public Text skill2GpCost;

	public Text skill3Name;
	public Text skill3GpCost;

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

		if (playerHP < 0) {
			playerHP = 0;
		}

		if (enemyHP < 0) {
			enemyHP = 0;
		}
	}

	public void ReturnToLobby ()
	{
		SceneManager.LoadScene ("scene1");
	}

	public void SetPlayerGP (int playerGP)
	{
		this.playerGP += playerGP;
		app.controller.tweenController.TweenPlayerGPSlider (this.playerGP, 1, true);
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
	public void CheckBattleStatus (bool secondCheck)
	{
		
		StartCoroutine (CheckBattleDelay (secondCheck));
	}

	private void AnimationWinLose (string param1, string param2, string param3, AudioEnum param4)
	{
		
		app.controller.characterAnimationController.SetTriggerAnim (true, param1);
		app.controller.characterAnimationController.SetTriggerAnim (false, param2);
		cachedBattleResult.text = param3;
		app.controller.audioController.PlayAudio (param4);
	}

	IEnumerator CheckBattleDelay (bool secondCheck)
	{
		if (enemyHP <= 0 || playerHP <= 0) {

			app.controller.cameraWorksController.StartWinLoseCamera ();
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
					if (app.model.battleModel.modePrototype == ModeEnum.Mode1) {
						app.component.firebaseDatabaseComponent.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
					} else if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
						app.component.firebaseDatabaseComponent.UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);

					}
				}
				yield return new WaitForSeconds (3);
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
				app.controller.tweenController.TweenEnemyHPSlider (enemyHP, 1, true);
				if (sameAttack == false) {
					StartCoroutine (StartAttackSequence (1));
				}
		
			} else {
				playerHP -= damage;
				app.controller.tweenController.TweenPlayerHPSlider (playerHP, 1, true);
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
			CheckBattleStatus (false);
			break;
		case 2:
			app.controller.characterAnimationController.SetTriggerAnim (false, "attack");
			app.controller.audioController.PlayAudio (AudioEnum.Attack);
			yield return new WaitForSeconds (0.5f);
			app.controller.characterAnimationController.SetTriggerAnim (true, "hit");
			app.controller.audioController.PlayAudio (AudioEnum.Hit);
			CheckBattleStatus (true);
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
			CheckBattleStatus (true);
			break;
		
		}

		//reset effects done by skill
		app.controller.gameController.ResetPlayerDamage();
		Debug.Log ("player damage reset! now damage is: " + app.model.battleModel.playerDamage);
		
	}

	public void SetSkill (ISkill skill)
	{
		skill.Activate ();
	}
		
	//send attack to firebase
	public void SendAttackToDatabase ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Damage.ToString ()] = app.model.battleModel.playerDamage;
		app.component.rpcWrapperComponent.RPCWrapAttack (param);
	}
}
