using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Controls the battle */
public class BattleController : SingletonMonoBehaviour<BattleController>
{
	public Text[] skillName;
	public Text[] skillGpCost;

	public int playerHP{ get; set; }

	public int enemyHP { get; set; }

	private int prepareTime = 3;

	private int playerMaxHP = 10;
	private int enemyMaxHP = 10;

	public Slider playerHPBar;
	public Slider enemyHPBar;

	public int playerGP{ get; set; }

	private int playerMaxGP = 10;
	public Slider playerGPBar;

	public string playerName;
	public string enemyName;

	public Text playerNameText;
	public Text playerHPText;
	public Text playerGPText;
	public Text enemyNameText;
	public Text enemyHPText;
	private bool stoptimer = false;
	private int timeLeft;

	public GameObject battleResultText;
	private Text cachedBattleResult;

	List<bool> userHome = new List<bool> ();
	List<Dictionary<string, System.Object>> param = new List<Dictionary<string, object>> ();

	/// <summary>
	/// Delay before start of battle
	/// </summary>
	public void StartPreTimer ()
	{
		CameraWorksController.Instance.StartIntroCamera ();
		cachedBattleResult = battleResultText.GetComponent<Text> ();
		timeLeft = 3;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerView.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimerView.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			PhaseManagerComponent.Instance.StartPhase1 ();
			GameTimerView.Instance.ToggleTimer (false);
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

	}

	public void InitialPlayerState (int playerHP, string playerName, int playerGP)
	{
		this.playerHP = playerHP;
		this.playerName = playerName;
		this.playerGP = playerGP;
		playerGPBar.maxValue = GameData.Instance.player.playerMaxGP;
		playerGPBar.value = 0;
		playerMaxHP = playerHP;
		playerHPBar.maxValue = playerMaxHP;
		playerMaxGP = GameData.Instance.player.playerMaxGP;
	}

	public void InitialEnemyState (int enemyHP, string enemyName)
	{
		this.enemyHP = enemyHP;
		this.enemyName = enemyName;
		enemyMaxHP = enemyHP;
		enemyHPBar.maxValue = enemyMaxHP;
	}

	public void SetAttack (Dictionary<bool, Dictionary<string, object>> currentParam)
	{

		Attack (currentParam);
	}

	private void ChangeUserOrder (int index0, int index1)
	{
		bool tempName = userHome [index0];
		Dictionary<string, System.Object> tempParam = param [index0];

		userHome.Insert (index0, userHome [index1]);
		userHome.Insert (index1, tempName);
		param.Insert (index0, param [index1]);
		param.Insert (index1, tempParam);
	}

	private void Attack (Dictionary<bool, Dictionary<string, object>> currentParam)
	{
		foreach (KeyValuePair<bool, Dictionary<string, System.Object>> newParam in currentParam) {
			userHome.Add (newParam.Key);
			param.Add (newParam.Value);
		}

		//change order of list if host or visitor
		if (GameData.Instance.isHost) {
			if (userHome [0] != GameData.Instance.isHost) {
				ChangeUserOrder (0, 1);
			}
		} else {
			if (userHome [1] != GameData.Instance.isHost) {
				ChangeUserOrder (1, 0);
			}

		}

		Debug.Log ("HOST IS" + userHome [0]);

		int attackOrder = 0;

		if (GameData.Instance.hAnswer > GameData.Instance.vAnswer) {
			attackOrder = 0;
		} else if (GameData.Instance.hAnswer < GameData.Instance.vAnswer) {
			attackOrder = 1;
		} else {
			if (GameData.Instance.hTime > GameData.Instance.vTime) {
				attackOrder = 0;
			} else if (GameData.Instance.hTime < GameData.Instance.vTime) {
				attackOrder = 1;
			} else {
				attackOrder = 2;
			}
		}

		//fix this soon very redundant!!!!!!!!!!!!!!!!
		switch (attackOrder) {
		case 0:
			Debug.Log ("player1 first attack");

			StartCoroutine (AttackParameterReduce (0, 1, 2));

			break;
		case 1:
			Debug.Log ("player2 first attack");

			StartCoroutine (AttackParameterReduce (1, 0, 2));

			break;
		case 2:
			Debug.Log ("same attack");
			StartCoroutine (AttackParameterReduce (0, 1, 0, true));
			StartCoroutine (StartAttackSequence (3));
			break;
		}
			
	}

	private IEnumerator AttackParameterReduce (int firstIndex, int secondIndex, int yieldTime, bool isSameAttack = false)
	{
		AttackParameter (userHome [firstIndex], param [firstIndex], isSameAttack);
		yield return new WaitForSeconds (yieldTime);
		AttackParameter (userHome [secondIndex], param [secondIndex], isSameAttack);

		userHome.Clear ();
	}

	public void CheckBattleStatus (bool secondCheck)
	{
		
		StartCoroutine (CheckBattleDelay (secondCheck));
	}

	private void AnimationWinLose (string param1, string param2, string param3, AudioEnum param4)
	{
		CharacterAnimationController.Instance.SetTriggerAnim (true, param1);
		CharacterAnimationController.Instance.SetTriggerAnim (false, param2);
		cachedBattleResult.text = param3;
		AudioController.Instance.PlayAudio (param4);
	}

	IEnumerator CheckBattleDelay (bool secondCheck)
	{
		if (enemyHP <= 0 || playerHP <= 0) {
//			app.controller.tweenController.TweenStopWaitOpponent (0.2f);

			CameraWorksController.Instance.StartWinLoseCamera ();
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
				if (GameData.Instance.isHost) {
					if (GameData.Instance.modePrototype == ModeEnum.Mode1) {
						FirebaseDatabaseComponent.Instance.UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
					} else if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
						FirebaseDatabaseComponent.Instance.UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);

					}
				}
				yield return new WaitForSeconds (3);
				PhaseManagerComponent.Instance.StartPhase1 ();

			}
		}
	}

	private void AttackParameter (bool attackerBool, Dictionary<string, System.Object> attackerParam, bool sameAttack = false)
	{
		if (attackerParam [ParamNames.Damage.ToString ()] != null) {
			int damage = int.Parse (attackerParam [ParamNames.Damage.ToString ()].ToString ());
		
			if (attackerBool.Equals (GameData.Instance.isHost)) {
		
				enemyHP -= damage;
				TweenController.TweenEnemyHPSlider (enemyHP, 1, true, enemyHPBar);
				if (sameAttack == false) {
					StartCoroutine (StartAttackSequence (1));
				}
		
			} else {
				playerHP -= damage;
				TweenController.TweenPlayerHPSlider (playerHP, 1, true, playerHPBar);
				if (sameAttack == false) {
					StartCoroutine (StartAttackSequence (2));
				}
			}
		}

	}

	IEnumerator StartAttackSequence (int sequenceType)
	{

		switch (sequenceType) {
		case 1:
			StartAttackSequenceReduce (AudioEnum.Attack, true, "attack");
			yield return new WaitForSeconds (0.5f);
			StartAttackSequenceReduce (AudioEnum.Hit, false, "hit");
			CheckBattleStatus (false);
			break;
		case 2:
			StartAttackSequenceReduce (AudioEnum.Hit, true, "hit");
			yield return new WaitForSeconds (0.5f);
			StartAttackSequenceReduce (AudioEnum.Attack, false, "attack");
			CheckBattleStatus (true);
			break;
		case 3:
			StartAttackSequenceReduce (AudioEnum.Attack, true, "attack");
			StartAttackSequenceReduce (AudioEnum.Attack, false, "attack");
			yield return new WaitForSeconds (0.5f);
			StartAttackSequenceReduce (AudioEnum.Hit, true, "hit");
			StartAttackSequenceReduce (AudioEnum.Hit, false, "hit");

			CheckBattleStatus (true);
			break;
		
		}

		//reset effects done by skill
		GameController.Instance.ResetPlayerDamage ();
		Debug.Log ("player damage reset! now damage is: " + GameData.Instance.player.playerDamage);
		
	}

	private void StartAttackSequenceReduce (AudioEnum audioType, bool isPlayer, string animParam)
	{

		CharacterAnimationController.Instance.SetTriggerAnim (isPlayer, animParam);
		AudioController.Instance.PlayAudio (audioType);
	}

	public void SetSkill (SkillModel skill)
	{
		FirebaseDatabaseComponent.Instance.SetSkillParam (skill);
		if (GameData.Instance.modePrototype == ModeEnum.Mode1) {
			RPCWrapperComponent.Instance.RPCWrapSkill ();
		} 
	}

	public void SetSkillUI (int skillNumber, ParamNames skillName, int skillGp)
	{
		this.skillName [skillNumber - 1].text = name.ToString ();
		this.skillGpCost [skillNumber - 1].text = "" + skillGp + "GP";
	}
		
	//send attack to firebase
	public void SendAttackToDatabase ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Damage.ToString ()] = GameData.Instance.player.playerDamage + GameData.Instance.gpEarned;
		RPCWrapperComponent.Instance.RPCWrapAttack (param);
	}
}
