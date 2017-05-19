using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCReceiver: SingletonMonoBehaviour<RPCReceiver> {

	BattleController battleController;

	void Start ()
	{
		battleController = FindObjectOfType<BattleController> ();
	}

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

	public void ReceiveInitialHomeState(Dictionary<string, System.Object> ititialState){
		string username = (string)ititialState["username"];
		int life = int.Parse(ititialState["life"].ToString());
		battleController.InitialHomeState (life, username);
	}

	public void ReceiveInitialVisitorState(Dictionary<string, System.Object> ititialState){
		string username = (string)ititialState["username"];
		int life = int.Parse(ititialState["life"].ToString());
		battleController.InitialVisitorState (life, username);
	}

	private Dictionary<string, System.Object> JsonStrToDic (string param)
	{
		Dictionary<string, System.Object> Dic = (Dictionary<string, System.Object>)MiniJSON.Json.Deserialize (param);
		return Dic;
	}
	
}
