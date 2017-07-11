﻿using UnityEngine;
using UnityEngine.UI;

public class ConnectionStatus : MonoBehaviour, IRPCBoolObserver {

	public Image connectionIndicator;

	void Start(){
		RPCBoolObserver.AddObserver (this);
	}

	public void OnNotify (bool isConnectedDB)
	{
		if (!isConnectedDB) {
			connectionIndicator.enabled = true;
		} else {
			connectionIndicator.enabled = false;
		}

	}

}
