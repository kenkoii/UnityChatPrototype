using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Handles receiving of RPC status */
public class RPCReceiverComponent: SingletonMonoBehaviour<RPCReceiverComponent>
{
	Dictionary<bool, Dictionary<string, object>> thisCurrentParameter = new Dictionary<bool, Dictionary<string, object>> ();
	int battleCount;
	string battleState;


	/// <summary>
	/// Receives the RPC status.
	/// </summary>
	/// <param name="rpcDetails">Rpc details.</param>
	public void ReceiveRPC (Dictionary<string, System.Object> rpcDetails)
	{
		bool userHome = (bool)rpcDetails ["userHome"];
		Dictionary<string, System.Object> param = JsonConverter.JsonStrToDic ((string)rpcDetails ["param"]);
	
		GameData.Instance.attackerBool = userHome;
		GameData.Instance.attackerParam = param;

		foreach (KeyValuePair<string, System.Object> newParam in param) {
			if (newParam.Key == ParamNames.Damage.ToString ()) {
				thisCurrentParameter.Add (GameData.Instance.attackerBool, GameData.Instance.attackerParam);
				if (thisCurrentParameter.Count == 2) {
					BattleController.Instance.SetAttack (thisCurrentParameter);
					thisCurrentParameter.Clear ();
				} 

			} 

			if (newParam.Key == ParamNames.AnswerCorrect.ToString ()) {
				AnswerController.Instance.ValidateAnswer (true, int.Parse (newParam.Value.ToString ()));
			}

			if (newParam.Key == ParamNames.AnswerWrong.ToString ()) {
				AnswerController.Instance.ValidateAnswer (false, int.Parse (newParam.Value.ToString ()));
			}

			if (newParam.Key == ParamNames.Gesture.ToString ()) {
				if (!(GameData.Instance.attackerBool.Equals (GameData.Instance.isHost))) {
					GestureController.Instance.SetEnemyGesture (int.Parse (newParam.Value.ToString ()));
				}
			}

			if (newParam.Key == ParamNames.AirRender.ToString ()) {
				SkillActivatorComponent.Instance.ActivateSkill (ParamNames.AirRender, int.Parse (newParam.Value.ToString ()));
				
			}
			if (newParam.Key == ParamNames.Sunder.ToString ()) {
				SkillActivatorComponent.Instance.ActivateSkill (ParamNames.Sunder, int.Parse (newParam.Value.ToString ()));
			}
			if (newParam.Key == ParamNames.Rejuvination.ToString ()) {
				SkillActivatorComponent.Instance.ActivateSkill (ParamNames.Rejuvination, int.Parse (newParam.Value.ToString ()));
			}

			if (newParam.Key == ParamNames.BicPunch.ToString ()) {
				SkillActivatorComponent.Instance.ActivateSkill (ParamNames.BicPunch, int.Parse (newParam.Value.ToString ()));
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
//				app.controller.tweenController.TweenStopWaitOpponent (0.2f);

			
			} else {
				//hide skill ui 
				PhaseSkillController.Instance.HideSkillUI ();
			}
			break;

		case MyConst.BATTLE_STATUS_SKILL:
			if (battleCount > 1) {
				if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
					PhaseManagerComponent.Instance.StartPhase2 ();
				} else {
					PhaseManagerComponent.Instance.StartPhase3 ();
				}
//				app.controller.tweenController.TweenStopWaitOpponent (0.2f);
			}
			break;

		case MyConst.BATTLE_STATUS_ATTACK:
			if (battleCount > 1) {
//				app.controller.tweenController.TweenStopWaitOpponent (0.2f);
			} else {
				//hide skill ui 
				PhaseSkillController.Instance.HideSkillUI ();
			}
		
			break;

		}

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
		string gameName = (string)ititialState ["gameName"];
		int life = int.Parse (ititialState ["life"].ToString ());
		int gp = int.Parse (ititialState ["gp"].ToString ());


		if (isHome) {
			if (GameData.Instance.isHost) {
				BattleController.Instance.InitialPlayerState (life, gameName, gp);
			} else {

				BattleController.Instance.InitialEnemyState (life, gameName);
			}
		} else {
			if (GameData.Instance.isHost) {

				BattleController.Instance.InitialEnemyState (life, gameName);
			} else {

				BattleController.Instance.InitialPlayerState (life, gameName, gp);
			}
		}

	}
}
