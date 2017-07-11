﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PhaseAttackController : BasePhase
{
	public override void OnStartPhase ()
	{
		Debug.Log ("Starting attack phase");
		AnswerIndicatorController.Instance.ResetAnswer ();
		RPCDicObserver.AddObserver (BattleLogic.Instance);
		GameTimerView.Instance.ToggleTimer (false);
		stoptimer = true;
		timeLeft = 20;
		InvokeRepeating ("StartTimer", 0, 1);
		Attack ();
	}

	public void Attack ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Attack.ToString ()] = GameData.Instance.player.playerDamage + GameData.Instance.gpEarned;
		FDController.Instance.AttackPhase (new AttackModel(JsonConverter.DicToJsonStr(param)));
	}


	public override void OnEndPhase(){
		RPCDicObserver.RemoveObserver (BattleLogic.Instance);
		CancelInvoke ("StartTimer");
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			if (timeLeft > 0) {
				timeLeft--;
				return;
			} 

//			app.component.firebaseDatabaseComponent.CheckAttackPhase();

			stoptimer = false;

		}
	}
		
}
