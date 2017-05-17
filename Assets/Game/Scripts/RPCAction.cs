using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCAction {

	public string actor;
	public string type;
	public Dictionary<string, System.Object> param;

	public RPCAction() {
	}

	public RPCAction(string actor, string type, Dictionary<string, System.Object> param){
		this.actor = actor;
		this.type = type;
		this.param = param;
	}
}
