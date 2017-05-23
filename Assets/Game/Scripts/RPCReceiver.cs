using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCReceiver: SingletonMonoBehaviour<RPCReceiver> {

	BattleController battleController;

	void Start ()
	{
		battleController = FindObjectOfType<BattleController> ();
	}

	/// <summary>
	/// Receives the RPC status.
	/// </summary>
	/// <param name="rpcDetails">Rpc details.</param>
	public void ReceiveRPC(Dictionary<string, System.Object> rpcDetails){
		string username = (string)rpcDetails["username"];
		int statusType = int.Parse(rpcDetails["statusType"].ToString());
		StatusType newStatusType = (StatusType)statusType;
		Dictionary<string, System.Object> param = JsonStrToDic((string)rpcDetails["param"]);
	
		GameManager.Instance.attackerName = username;
		GameManager.Instance.attackerParam = param;

		switch (newStatusType) {
		case StatusType.Attack:
			ExecutionManager.Instance.ExecutePlayerAttack ();
			break;
		}
	}

	/// <summary>
	/// Receives the initial state of the home.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	public void ReceiveInitialHomeState(Dictionary<string, System.Object> ititialState){
		ReceivInitialState (ititialState, true);
	}
	/// <summary>
	/// Receives the initial state of the visitor.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	public void ReceiveInitialVisitorState(Dictionary<string, System.Object> ititialState){
		ReceivInitialState (ititialState, false);
	}

	/// <summary>
	/// Receivs the initial state.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	/// <param name="isHome">If set to <c>true</c> is home.</param>
	private void ReceivInitialState(Dictionary<string, System.Object> ititialState,bool isHome){
		string username = (string)ititialState["username"];
		int life = int.Parse(ititialState["life"].ToString());

		if (isHome) {
			battleController.InitialHomeState (life, username);
		} else {
			battleController.InitialVisitorState (life, username);
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
