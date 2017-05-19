using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCReceiver: SingletonMonoBehaviour<RPCReceiver> {

	BattleManager battleManager;

	void Start ()
	{
		battleManager = FindObjectOfType<BattleManager> ();
	}

	public void ReceiveRPC(Dictionary<string, System.Object> rpcDetails){
		string username = (string)rpcDetails["username"];
		int statusType = int.Parse(rpcDetails["statusType"].ToString());
		StatusType newStatusType = (StatusType)statusType;
		Dictionary<string, System.Object> param = JsonStrToDic((string)rpcDetails["param"]);


		GameManager.Instance.attackerName = username;
	


		switch (newStatusType) {
		case StatusType.attack:
			ExecutionManager.Instance.ExecutePlayerAttack ();
			break;
		}
	}

	public void ReceiveInitialHomeState(Dictionary<string, System.Object> ititialState){
		string username = (string)ititialState["username"];
		int life = int.Parse(ititialState["life"].ToString());
		battleManager.InitialHomeState (life, username);
	}

	public void ReceiveInitialVisitorState(Dictionary<string, System.Object> ititialState){
		string username = (string)ititialState["username"];
		int life = int.Parse(ititialState["life"].ToString());
		battleManager.InitialVisitorState (life, username);
	}

	private Dictionary<string, System.Object> JsonStrToDic (string param)
	{
		Dictionary<string, System.Object> Dic = (Dictionary<string, System.Object>)MiniJSON.Json.Deserialize (param);
		return Dic;
	}
	
}
