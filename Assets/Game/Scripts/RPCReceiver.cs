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
		StatusType statusType = (StatusType)rpcDetails["statusType"];
		Dictionary<string, System.Object> param = JsonStrToDic((string)rpcDetails["param"]);
	}

	public void ReceiveInitialHomeState(Dictionary<string, System.Object> ititialState){
		string username = (string)ititialState["username"];
		int life = (int)ititialState["life"];
		battleManager.InitialHomeState (life, username);
	}

	public void ReceiveInitialVisitorState(Dictionary<string, System.Object> ititialState){
		string username = (string)ititialState["username"];
		int life = (int)ititialState["life"];
		battleManager.InitialVisitorState (life, username);
	}

	private Dictionary<string, System.Object> JsonStrToDic (string param)
	{
		Dictionary<string, System.Object> Dic = (Dictionary<string, System.Object>)MiniJSON.Json.Deserialize (param);
		return Dic;
	}
	
}
