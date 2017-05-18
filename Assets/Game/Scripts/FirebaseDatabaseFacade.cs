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
	string gameRoomKey = null;
	Dictionary<string, System.Object> receiveMessage;

	[System.Serializable] public class MessageBroadcast : UnityEvent<Dictionary<string, System.Object>>
	{

	};

	public MessageBroadcast sendMessage;
	public MessageBroadcast sendInitialHomeState;
	public MessageBroadcast sendInitialVisitorState;

	[System.Serializable] public class RoomListBroadcast : UnityEvent<List<string>>
	{

	};

	public RoomListBroadcast sendRoomList;


	void Start ()
	{
		#if UNITY_EDITOR
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://chatprototype-39807.firebaseio.com");
		#endif
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		DatabaseReference roomReference = reference;
		roomReference.ChildAdded += HandleChatroomChildAdded;
		roomReference.ChildRemoved += HandleChatroomChildAdded;

	}

	void HandleChildAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		receiveMessage = (Dictionary<string, System.Object>)args.Snapshot.Value;
		sendMessage.Invoke (receiveMessage);

	}

	void HandleInitialHomeChildAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		receiveMessage = (Dictionary<string, System.Object>)args.Snapshot.Value;
		sendInitialHomeState.Invoke (receiveMessage);

	}

	void HandleInitialVisitorChildAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		receiveMessage = (Dictionary<string, System.Object>)args.Snapshot.Value;
		sendInitialVisitorState.Invoke (receiveMessage);

	}

	void HandleChatroomChildAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
	
		var keyDictionary = args.Snapshot.Value;
		Debug.Log (keyDictionary);
		Dictionary<string, System.Object> newKeyDictionary = (Dictionary<string, System.Object>)keyDictionary;
		List<string> keyList = new List<string> (newKeyDictionary.Keys);

		sendRoomList.Invoke (keyList);

	}


	public void CreateRoom (string name, int life)
	{
		gameRoomKey = reference.Child ("GameRoom").Push ().Key;
		MessageListener ();

		User user = new User (name, life);
		Dictionary<string, System.Object> entryValues = user.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/GameRoom/" + gameRoomKey + "/InitialState/Home/"] = entryValues;
	}

	public void JoinRoom (string roomKey, string name, int life)
	{
		gameRoomKey = roomKey;
		MessageListener ();

		User user = new User (name, life);
		Dictionary<string, System.Object> entryValues = user.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/GameRoom/" + gameRoomKey + "/InitialState/Visitor/"] = entryValues;
		InitialStateListener ();

	}

	private void MessageListener ()
	{
		//so that only people who joined in the room will be broadcasted the message
		if (gameRoomKey != null) {
			reference.Child ("GameRoom").Child (gameRoomKey).Child ("RPC").ChildAdded += HandleChildAdded;
		}
	}

	private void InitialStateListener ()
	{
		//so that only people who joined in the room will be broadcasted the message
		if (gameRoomKey != null) {
			reference.Child ("GameRoom").Child (gameRoomKey).Child("InitialState").Child("Home").ChildAdded += HandleInitialHomeChildAdded;
			reference.Child ("GameRoom").Child (gameRoomKey).Child("InitialState").Child("Visitor").ChildAdded += HandleInitialVisitorChildAdded;
		}
	}

	public void AttackPlayer (string name, StatusType statusType, string param)
	{
		string	rpcKey = reference.Child ("GameRoom").Child (gameRoomKey).Child ("RPC").Push ().Key;

		BattleStatus battleStatus = new BattleStatus (name, statusType, param);
		Dictionary<string, System.Object> entryValues = battleStatus.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/GameRoom/" + gameRoomKey + "/RPC/" + rpcKey] = entryValues;

		reference.UpdateChildrenAsync (childUpdates);
	}



}
