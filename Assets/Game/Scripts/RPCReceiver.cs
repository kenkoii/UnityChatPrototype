using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Handles receiving of RPC status */
public class RPCReceiver: SingletonMonoBehaviour<RPCReceiver>
{

	BattleController battleController;

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
	
		StatusManager.Instance.attackerName = username;
		StatusManager.Instance.attackerParam = param;
		Debug.Log ("receive RPC");
		battleController.SetAttack ();
	

	}

	public void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		string battleState = battleStatusDetails [MyConst.BATTLE_STATUS_STATE].ToString ();
		int battleCount = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_COUNT].ToString ());



		switch (battleState) {
		case MyConst.BATTLE_STATUS_ANSWER:
			if (battleCount > 1) {
				
				PhaseManager.Instance.StartPhase2 ();
			}
			break;
		case MyConst.BATTLE_STATUS_SKILL:
			if (battleCount > 1) {
				
				PhaseManager.Instance.StartPhase3 ();
			}
			break;
		case MyConst.BATTLE_STATUS_ATTACK:
			if (battleCount > 1) {
				battleController.CheckBattleStatus ();
			}
		
			break;
		case MyConst.BATTLE_STATUS_END:
			PhaseManager.Instance.StopAll ();
			break;
		}

		StatusManager.Instance.battleState = battleState;
		StatusManager.Instance.battleCount = battleCount;

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
			if (StatusManager.Instance.isPlayerVisitor) {
				battleController.InitialEnemyState (life, username);
			} else {
				battleController.InitialPlayerState (life, username, gp);
			}
		} else {
			if (StatusManager.Instance.isPlayerVisitor) {
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
