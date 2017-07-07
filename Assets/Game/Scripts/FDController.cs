using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;
//using Firebase;

public class FDController : SingletonMonoBehaviour<FDController>
{
	DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

	void Start(){
		SetReference ();
	}

	private void SetReference ()
	{
		FDFacade.Instance.SetUnityEditorReference (MyConst.URL_FIREBASE_DATABASE);
	}

	public void SendRPC(Dictionary<string, System.Object> rpcParam){
//		string directory = "/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_RPC + "/" + rpcKey;
//		FDFacade.Instance.CreateTableChildrenAsync (directory, rpcParam);
	}


		


}
