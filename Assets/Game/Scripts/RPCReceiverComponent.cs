using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Handles receiving of RPC status */
public class RPCReceiverComponent: EnglishRoyaleElement
{
	Dictionary<string, Dictionary<string, object>> thisCurrentParameter = new Dictionary<string, Dictionary<string, object>> ();
	int battleCount;
	string battleState;


	/// <summary>
	/// Receives the RPC status.
	/// </summary>
	/// <param name="rpcDetails">Rpc details.</param>
	public void ReceiveRPC (Dictionary<string, System.Object> rpcDetails)
	{
		string username = (string)rpcDetails ["username"];
		Dictionary<string, System.Object> param = JsonStrToDic ((string)rpcDetails ["param"]);
	
		app.model.battleModel.attackerName = username;
		app.model.battleModel.attackerParam = param;

		foreach (KeyValuePair<string, System.Object> newParam in param) {
			if (newParam.Key == ParamNames.Damage.ToString ()) {
				thisCurrentParameter.Add (app.model.battleModel.attackerName, app.model.battleModel.attackerParam);
				

				if (thisCurrentParameter.Count == 2) {
					app.controller.battleController.SetAttackMode2 (thisCurrentParameter);
					thisCurrentParameter.Clear ();

				} 

			} 	
		}
			
	}

	public void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		battleState = battleStatusDetails [MyConst.BATTLE_STATUS_STATE].ToString ();
		battleCount = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_COUNT].ToString ());

		Debug.Log ("receive battle status:" + battleState + "battle count:" + battleCount);

		switch (battleState) {
		case MyConst.BATTLE_STATUS_ANSWER:

			app.model.battleModel.hAnswer = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_HANSWER].ToString ());
			app.model.battleModel.hTime = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_HTIME].ToString ());
			app.model.battleModel.vAnswer = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_VANSWER].ToString ());
			app.model.battleModel.vTime = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_VTIME].ToString ());

			 
			if (battleCount > 1) {
				if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
					app.component.phaseManagerComponent.StartPhase3 ();
				} else {
					app.component.phaseManagerComponent.StartPhase2 ();
				}
				app.controller.tweenController.TweenStopWaitOpponent (0.2f);
			} 
			break;

		case MyConst.BATTLE_STATUS_SKILL:
			if (battleCount > 1) {
				if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
					app.component.phaseManagerComponent.StartPhase2 ();
				} else {
					app.component.phaseManagerComponent.StartPhase3 ();
				}
				app.controller.tweenController.TweenStopWaitOpponent (0.2f);
			}
			break;

		case MyConst.BATTLE_STATUS_ATTACK:
			if (battleCount > 1) {
				app.controller.tweenController.TweenStopWaitOpponent (0.2f);
			}
		
			break;

		case MyConst.BATTLE_STATUS_END:
			app.component.phaseManagerComponent.StopAll ();
			break;

		}

		app.model.battleModel.battleState = battleState;
		app.model.battleModel.battleCount = battleCount;

	}

	/// <summary>
	/// Receives the initial state of the home.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	public void ReceiveInitialHomeState (Dictionary<string, System.Object> ititialState)
	{
		ReceivInitialState (ititialState, true);
	}

	/// <summary>
	/// Receives the initial state of the visitor.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	public void ReceiveInitialVisitorState (Dictionary<string, System.Object> ititialState)
	{
		ReceivInitialState (ititialState, false);
	}

	/// <summary>
	/// Receivs the initial state.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	/// <param name="isHome">If set to <c>true</c> is home.</param>
	private void ReceivInitialState (Dictionary<string, System.Object> ititialState, bool isHome)
	{
		string username = (string)ititialState ["username"];
		int life = int.Parse (ititialState ["life"].ToString ());
		int gp = int.Parse (ititialState ["gp"].ToString ());


		if (isHome) {
			if (app.model.battleModel.isPlayerVisitor) {
				app.controller.battleController.InitialEnemyState (life, username);
			} else {
				app.controller.battleController.InitialPlayerState (life, username, gp);
			}
		} else {
			if (app.model.battleModel.isPlayerVisitor) {
				app.controller.battleController.InitialPlayerState (life, username, gp);
			} else {
				app.controller.battleController.InitialEnemyState (life, username);
			}
		}

	}

	/// <summary>
	/// Converts Json String to Dictionary<string, System.Object>
	/// </summary>
	/// <returns>The string to dic.</returns>
	/// <param name="param">Parameter.</param>
	private Dictionary<string, System.Object> JsonStrToDic (string param)
	{
		Dictionary<string, System.Object> Dic = (Dictionary<string, System.Object>)MiniJSON.Json.Deserialize (param);
		return Dic;
	}
	
}
