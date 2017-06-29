using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Handles receiving of RPC status */
public class RPCReceiverComponent: EnglishRoyaleElement
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
		Dictionary<string, System.Object> param = JsonStrToDic ((string)rpcDetails ["param"]);
	
		app.model.battleModel.attackerBool = userHome;
		app.model.battleModel.attackerParam = param;

		foreach (KeyValuePair<string, System.Object> newParam in param) {
			if (newParam.Key == ParamNames.Damage.ToString ()) {
				thisCurrentParameter.Add (app.model.battleModel.attackerBool, app.model.battleModel.attackerParam);
				if (thisCurrentParameter.Count == 2) {
					app.controller.battleController.SetAttackMode2 (thisCurrentParameter);
					thisCurrentParameter.Clear ();
				} 

			} 

			if (newParam.Key == ParamNames.AnswerCorrect.ToString ()) {
				app.controller.answerController.ValidateAnswer (true, int.Parse (newParam.Value.ToString ()));
			}

			if (newParam.Key == ParamNames.AnswerWrong.ToString ()) {
				app.controller.answerController.ValidateAnswer (false, int.Parse (newParam.Value.ToString ()));
			}

			if (newParam.Key == ParamNames.Gesture.ToString ()) {
				if (!(app.model.battleModel.attackerBool.Equals (app.model.battleModel.isHost))) {
					app.controller.gestureController.SetEnemyGesture (int.Parse(newParam.Value.ToString()));
				}
			}

			if (newParam.Key == ParamNames.AirRender.ToString ()) {
				app.component.skillActivatorComponent.ActivateSkill (ParamNames.AirRender,int.Parse(newParam.Value.ToString()));
				
			}
			if (newParam.Key == ParamNames.Sunder.ToString ()) {
				app.component.skillActivatorComponent.ActivateSkill (ParamNames.Sunder,int.Parse(newParam.Value.ToString()));
			}
			if (newParam.Key == ParamNames.Rejuvination.ToString ()) {
				app.component.skillActivatorComponent.ActivateSkill (ParamNames.Rejuvination,int.Parse(newParam.Value.ToString()));
			}

			if (newParam.Key == ParamNames.BicPunch.ToString ()) {
				app.component.skillActivatorComponent.ActivateSkill (ParamNames.BicPunch,int.Parse(newParam.Value.ToString()));
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

			
			} else {
				//hide skill ui 
				app.controller.phaseSkillController.HideSkillUI ();
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
			} else {
				//hide skill ui 
				app.controller.phaseSkillController.HideSkillUI ();
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
		string gameName = (string)ititialState ["gameName"];
		int life = int.Parse (ititialState ["life"].ToString ());
		int gp = int.Parse (ititialState ["gp"].ToString ());


		if (isHome) {
			if (app.model.battleModel.isHost) {
				app.controller.battleController.InitialPlayerState (life, gameName, gp);
			} else {

				app.controller.battleController.InitialEnemyState (life, gameName);
			}
		} else {
			if (app.model.battleModel.isHost) {

				app.controller.battleController.InitialEnemyState (life, gameName);
			} else {

				app.controller.battleController.InitialPlayerState (life, gameName, gp);
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
