using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Handles receiving of RPC status */
public class RPCReceiverComponent: SingletonMonoBehaviour<RPCReceiverComponent>
{
	Dictionary<bool, Dictionary<string, object>> thisCurrentParameter = new Dictionary<bool, Dictionary<string, object>> ();

	/// <summary>
	/// Receives the RPC status.
	/// </summary>
	/// <param name="rpcDetails">Rpc details.</param>
	public void ReceiveRPC (Dictionary<string, System.Object> rpcDetails)
	{
		bool userHome = (bool)rpcDetails ["userHome"];
		Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcDetails ["param"];

		GameData.Instance.attackerBool = userHome;

		foreach (KeyValuePair<string, System.Object> newParam in param) {

			//NORMAL ATTACK
			if (newParam.Key == "Attack") {
				Dictionary<string, System.Object> attackerParam = JsonConverter.JsonStrToDic (newParam.Value.ToString ());
				thisCurrentParameter.Add (GameData.Instance.attackerBool, attackerParam);
				if (thisCurrentParameter.Count == 2) {
					BattleController.Instance.SetAttack (thisCurrentParameter);
					thisCurrentParameter.Clear ();
				} 

			} 

			//ANSWER INDICATORS

			if (newParam.Key == "AnswerIndicator") {
				AnswerController.Instance.SetAnswerParameter (newParam.Value.ToString ());
			}
				

			// GESTURE

			if (newParam.Key == "Gesture") {
				if (!(GameData.Instance.attackerBool.Equals (GameData.Instance.isHost))) {
					GestureController.Instance.SetEnemyGesture (newParam.Value.ToString ());
				}
			}

			//SKILL PARAMETERS

			if (newParam.Key == "SkillName") {


				SkillActivatorComponent.Instance.CheckSkillName (newParam.Value.ToString ());
			}

			if (newParam.Key == "SkillParam") {
				if (!(GameData.Instance.attackerBool.Equals (GameData.Instance.isHost))) {
					SkillActivatorComponent.Instance.SetPlayerSkillParameter (newParam.Value.ToString ());
				} else {
					SkillActivatorComponent.Instance.SetEnemySkillParameter (newParam.Value.ToString ());
				}
			}

		}
			
	}

	public void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		string battleState = battleStatusDetails [MyConst.BATTLE_STATUS_STATE].ToString ();
		int battleCount = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_COUNT].ToString ());

		switch (battleState) {
		case MyConst.BATTLE_STATUS_ANSWER:

			GameData.Instance.hAnswer = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_HANSWER].ToString ());
			GameData.Instance.hTime = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_HTIME].ToString ());
			GameData.Instance.vAnswer = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_VANSWER].ToString ());
			GameData.Instance.vTime = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_VTIME].ToString ());

			 
			if (battleCount > 1) {
				if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
					PhaseManagerComponent.Instance.StartPhase3 ();
				} else {
					PhaseManagerComponent.Instance.StartPhase2 ();
				}
				ScreenController.Instance.StopWaitOpponentScreen ();
			
			} 

			break;

		case MyConst.BATTLE_STATUS_SKILL:
			if (battleCount > 1) {
				
				if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
					PhaseManagerComponent.Instance.StartPhase2 ();
				} else {
					PhaseManagerComponent.Instance.StartPhase3 ();
				}
				ScreenController.Instance.StopWaitOpponentScreen ();
			}
			break;

		case MyConst.BATTLE_STATUS_ATTACK:
			if (battleCount > 1) {
				ScreenController.Instance.StopWaitOpponentScreen ();
			} else {
				//hide skill ui 
				PhaseSkillController.Instance.ShowSkillUI (false);
			}
		
			break;

		}

	}

	/// <summary>
	/// Receives the initial state of the home.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	public void ReceiveInitialHomeState (Dictionary<string, System.Object> initialState)
	{
		ReceivInitialState (initialState, true);
	}

	/// <summary>
	/// Receives the initial state of the visitor.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	public void ReceiveInitialVisitorState (Dictionary<string, System.Object> initialState)
	{
		ReceivInitialState (initialState, false);
	}

	/// <summary>
	/// Receivs the initial state.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	/// <param name="isHome">If set to <c>true</c> is home.</param>
	private void ReceivInitialState (Dictionary<string, System.Object> initialState, bool isHome)
	{
		string gameName = (string)initialState ["gameName"];
		int life = int.Parse (initialState ["life"].ToString ());
		int gp = int.Parse (initialState ["gp"].ToString ());


		if (isHome) {
			SetPlayerInitialState (gameName, life, gp);
		} else {
			SetEnemyInitialState (gameName, life, gp);
		}

	}

	private void SetPlayerInitialState (string gameName, int life, int gp)
	{
		if (GameData.Instance.isHost) {
			BattleController.Instance.InitialPlayerState (life, gameName, gp);
		} else {
			BattleController.Instance.InitialEnemyState (life, gameName);
		}
	}

	private void SetEnemyInitialState (string gameName, int life, int gp)
	{
		if (GameData.Instance.isHost) {
			BattleController.Instance.InitialEnemyState (life, gameName);
		} else {
			BattleController.Instance.InitialPlayerState (life, gameName, gp);
		}
	}
}
