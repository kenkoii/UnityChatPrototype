using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Handles receiving of RPC status */
public class RPCReceiver: SingletonMonoBehaviour<RPCReceiver>
{

	BattleController battleController;
	int battleCount;
	string battleState;

	void Start ()
	{
		battleController = FindObjectOfType<BattleController> ();
	}

	/// <summary>
	/// Receives the RPC status.
	/// </summary>
	/// <param name="rpcDetails">Rpc details.</param>
	public void ReceiveRPC (Dictionary<string, System.Object> rpcDetails)
	{
		string username = (string)rpcDetails ["username"];
		Dictionary<string, System.Object> param = JsonStrToDic ((string)rpcDetails ["param"]);
	
		MyGlobalVariables.Instance.attackerName = username;
		MyGlobalVariables.Instance.attackerParam = param;

		foreach (KeyValuePair<string, System.Object> newParam in param) {
			if (MyGlobalVariables.Instance.modePrototype == 2) {
				if (newParam.Key == ParamNames.Damage.ToString ()) {

					Debug.Log (MyGlobalVariables.Instance.attackerName);
					Debug.Log ( MyGlobalVariables.Instance.attackerParam.Count);

					MyGlobalVariables.Instance.currentParameter.Add (MyGlobalVariables.Instance.attackerName, MyGlobalVariables.Instance.attackerParam);

					if (MyGlobalVariables.Instance.currentParameter.Count == 2) {
						battleController.SetAttackMode2 ();
					} 

				} else if (newParam.Key == ParamNames.SkillDamage.ToString ()) {
					//skill here
				}

			} else {

				if (newParam.Key == ParamNames.Damage.ToString ()) {
					battleController.SetAttack ();
					if (battleState.Equals (MyConst.BATTLE_STATUS_ATTACK) && battleCount > 1) {
						battleController.CheckBattleStatus ();
					}
				} else if (newParam.Key == ParamNames.SkillDamage.ToString ()) {
					if (MyGlobalVariables.Instance.attackerName.Equals (MyGlobalVariables.Instance.playerName)) {
						//skill here
					} 
				}
			}


				
		}
			
	}

	public void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		battleState = battleStatusDetails [MyConst.BATTLE_STATUS_STATE].ToString ();
		battleCount = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_COUNT].ToString ());



		switch (battleState) {
		case MyConst.BATTLE_STATUS_ANSWER:
			
			MyGlobalVariables.Instance.hAnswer = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_HANSWER].ToString ());
			MyGlobalVariables.Instance.hTime = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_HTIME].ToString ());
			MyGlobalVariables.Instance.vAnswer = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_VANSWER].ToString ());
			MyGlobalVariables.Instance.vTime = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_VTIME].ToString ());


			if (MyGlobalVariables.Instance.currentParameter.Count == 2) {
				
			
			} else {
				if (battleCount > 1) {
					PhaseManager.Instance.StartPhase2 ();
				}
			}


			break;
		case MyConst.BATTLE_STATUS_SKILL:
			if (battleCount > 1) {
				
				PhaseManager.Instance.StartPhase3 ();
			}
			break;
		case MyConst.BATTLE_STATUS_ATTACK:
			if (battleCount > 1) {

			}
		
			break;
		case MyConst.BATTLE_STATUS_END:
			PhaseManager.Instance.StopAll ();
			break;
		}

		MyGlobalVariables.Instance.battleState = battleState;
		MyGlobalVariables.Instance.battleCount = battleCount;

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
			if (MyGlobalVariables.Instance.isPlayerVisitor) {
				battleController.InitialEnemyState (life, username);
			} else {
				battleController.InitialPlayerState (life, username, gp);
			}
		} else {
			if (MyGlobalVariables.Instance.isPlayerVisitor) {
				battleController.InitialPlayerState (life, username, gp);
			} else {
				battleController.InitialEnemyState (life, username);
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
