using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPC: SingletonMonoBehaviour<RPC> {

	public void Reducer(RPCAction action){
		switch(action.type){
		case "sendMessage":
			FirebaseDatabaseFacade.Instance.WriteNewMessage (action.param["userName"].ToString(), action.param["message"].ToString(), (long)action.param["timeStamp"]);
			break;
		}
	}
}
