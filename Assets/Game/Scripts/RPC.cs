using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RPC : SingletonMonoBehaviour<RPC> {
	public void SendRPC(Dictionary<string, System.Object> rpcParam){
		FDController.Instance.SendRPC (rpcParam);
	}

	public void ReceiveDBConnection(bool isConnectedDB){
		RPCBoolObserver.Notify (isConnectedDB);
	}

	public void ReceiveRPC(Dictionary<string, System.Object> rpcReceive){
		RPCDicObserver.Notify (rpcReceive);
	}

}
