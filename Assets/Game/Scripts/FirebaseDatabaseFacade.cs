using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.Events;

#if UNITY_EDITOR
using Firebase.Unity.Editor;
#endif

using System;

public class FirebaseDatabaseFacade : SingletonMonoBehaviour<FirebaseDatabaseFacade>
{

	DatabaseReference reference;
	string chatRoomKey = null;
	Dictionary<string, System.Object> receiveMessage;
	[System.Serializable] public class MessageBroadcast : UnityEvent<Dictionary<string, System.Object>>{};
	public MessageBroadcast sendMessage;

	[System.Serializable] public class RoomListBroadcast : UnityEvent<List<string>>{};
	public RoomListBroadcast sendRoomList;
	private List<string> roomList = new List<string>();

	void Start ()
	{
		#if UNITY_EDITOR
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://chatprototype-39807.firebaseio.com");
		#endif
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		DatabaseReference roomReference = reference.Child("chatroom");
		roomReference.ValueChanged += HandleRoomValueChanged;


	}

	void HandleRoomValueChanged(object sender, ValueChangedEventArgs args) {
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}


		foreach (DataSnapshot name in args.Snapshot.Children) {
			roomList.Add (name.Key.ToString());
		}

		sendRoomList.Invoke (roomList);

	}
		
	void HandleChildAdded(object sender, ChildChangedEventArgs args) {
		if (args.DatabaseError != null) {
			Debug.LogError(args.DatabaseError.Message);
			return;
		}
		receiveMessage = (Dictionary<string, System.Object>)args.Snapshot.Value;
		sendMessage.Invoke (receiveMessage);
		Debug.Log ("callTImes");

	}

	public void CreateRoom(){
			chatRoomKey = reference.Child ("chatroom").Push ().Key;
		if (chatRoomKey != null) {
			reference.Child ("chatroom").Child (chatRoomKey).Child ("messages").ChildAdded += HandleChildAdded;
		}
	}

	public void JoinRoom(string roomKey){
		chatRoomKey = roomKey;
		if (chatRoomKey != null) {
			reference.Child ("chatroom").Child (chatRoomKey).Child ("messages").ChildAdded += HandleChildAdded;
		}
	}


	public void WriteNewMessage (string name, string message, long timeStamp)
	{

//		if (String.IsNullOrEmpty(chatRoomKey)) {
//			chatRoomKey = reference.Child ("chatroom").Push ().Key;
//		}

		string	messageKey = reference.Child ("chatroom").Child (chatRoomKey).Child ("messages").Push ().Key;

		User user = new User (name, message, timeStamp);
		Dictionary<string, System.Object> entryValues = user.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/chatroom/" + chatRoomKey + "/messages/" + messageKey] = entryValues;
	
		reference.UpdateChildrenAsync (childUpdates);
	}

	public void ReceiveMessage ()
	{
		
	}





}
