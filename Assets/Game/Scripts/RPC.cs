using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RPC : SingletonMonoBehaviour<RPC> {

	public void ReceiveDBConnection(bool isConnectedDB){
		RPCBoolObserver.Notify (isConnectedDB);
	}

	public void ReceiveRPC(Firebase.Database.DataSnapshot dataSnapShot){
		RPCDicObserver.Notify (dataSnapShot);
	}

	public void ReceiveRPCQuery(Firebase.Database.DataSnapshot dataSnapShot){
		RPCQueryObserver.NotifyQuery (dataSnapShot);
	}

}
