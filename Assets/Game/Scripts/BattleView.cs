﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* Controls the battle */
public class BattleView : SingletonMonoBehaviour<BattleView>
{
	public Text[] skillName;
	public Text[] skillGpCost;

	private string playerName;
	public Text playerNameText;

	private int playerHP;

	private int playerMaxHP = 10;
	public Slider playerHPBar;
	public Text playerHPText;
	public Text playerGPText;

	private int playerGP;

	private int playerMaxGP = 10;
	public Slider playerGPBar;

	private string enemyName;

	private int enemyHP;

	private int enemyMaxHP = 10;
	public Slider enemyHPBar;
	public Text enemyNameText;
	public Text enemyHPText;

	public Text battleResultText;
	public Button backToLobbyButton;
	public Image backToLobbyImage;

	public int PlayerHP {
		get{ return playerHP; }
		set {
			playerHP = value;
			TweenController.TweenEnemyHPSlider (playerHP, 1, true, playerHPBar);
		}
	}

	public int PlayerGP {
		get{ return playerGP; }
		set {
			playerGP = value;
			TweenController.TweenEnemyHPSlider (playerGP, 1, true, playerGPBar);
		}
	}

	public int EnemyHP {
		get{ return enemyHP; }
		set {
			enemyHP = value;
			TweenController.TweenEnemyHPSlider (enemyHP, 1, true, enemyHPBar);
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


	public void SetStateParam(Firebase.Database.DataSnapshot dataSnapShot, bool isHome){
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey ("param")) {
			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
			ReceiveInitialState (param, isHome);
		}
	}

	private void ReceiveInitialState (Dictionary<string, System.Object> initialState, bool isHome)
	{
		string playerName = (string)initialState ["playerName"];
		int playerLife = int.Parse (initialState ["playerLife"].ToString ());
		int playerGp = int.Parse (initialState ["playerGP"].ToString ());
	
		Debug.Log (playerName);
		Debug.Log (playerLife);

		if (isHome) {
			SetInitialPlayerState (playerName, playerLife, playerGp);
		} else {
			SetInitialEnemyState (playerName, playerLife);
		}
	}

	public void SetInitialPlayerState (string playerName, int playerHP, int playerGP)
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

	public void SetInitialEnemyState (string enemyName, int enemyHP)
	{
		this.enemyHP = enemyHP;
		this.enemyName = enemyName;
		enemyMaxHP = enemyHP;
		enemyHPBar.maxValue = enemyMaxHP;
	}

	public void SetSkillUI (int skillNumber, ParamNames skillName, int skillGp)
	{
		this.skillName [skillNumber - 1].text = skillName.ToString ();
		this.skillGpCost [skillNumber - 1].text = "" + skillGp + "GP";
	}

	public void ShowWinLose (string param1, string param2, string param3, AudioEnum param4)
	{
		CharacterAnimationController.Instance.SetTriggerAnim (true, param1);
		CharacterAnimationController.Instance.SetTriggerAnim (false, param2);
		battleResultText.text = param3;
		battleResultText.enabled = true;
		backToLobbyButton.enabled = true;
		backToLobbyImage.enabled = true;
		AudioController.Instance.PlayAudio (param4);
	}


}
