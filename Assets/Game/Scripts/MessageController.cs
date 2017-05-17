using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MessageController : MonoBehaviour {

	public InputField message;
	public Text messageBox;

	void Start(){
		messageBox.text = "";
	}

	public void SendChatMessage ()
	{
		long timeStamp = (long)DateTime.Now.Ticks;
		FirebaseDatabaseFacade.Instance.WriteNewMessage (GameManager.Instance.userName, message.text, timeStamp);
	}

	public void GetChatMessage(Dictionary<string, System.Object> messageDetails){
		
		string username = (string)messageDetails["username"];
		string message = (string)messageDetails["message"];
//		long timeStamp = (long)messageDetails["timestamp"];

		messageBox.text += "" + username + ": " + message + "\n";
	}
}
