using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RPC : SingletonMonoBehaviour<RPC> {

	public void ReceiveDBConnection(bool isConnectedDB){
		RPCBoolObserver.Notify (isConnectedDB);
	}

	public void ReceiveRPC(Dictionary<string, System.Object> rpcReceive){
		RPCDicObserver.Notify (rpcReceive);
	}

	public void ReceiveRPCQuery(Firebase.Database.DataSnapshot dataSnapShot){
		RPCQueryObserver.NotifyQuery (dataSnapShot);
	}

}
