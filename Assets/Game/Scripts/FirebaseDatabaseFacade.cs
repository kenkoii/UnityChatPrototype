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
	DatabaseReference roomReference;
	string gameRoomKey = null;
	Dictionary<string, System.Object> receiveMessage;
	private bool isHasGameRooms = false;
	private bool searchingRoom = false;

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

		roomReference = reference.Child ("GameRoom");

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

	void HandleChatroomChildAdded (object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
	
		var gameRoomList = args.Snapshot.Children;
		if (searchingRoom) {
			
			isHasGameRooms = args.Snapshot.HasChildren;

			if (isHasGameRooms) {

				Debug.Log ("has game rooms");
				foreach (DataSnapshot dataSnapshot in gameRoomList) {
					if (dataSnapshot.Child ("RoomStatus").Value.ToString ().Equals ("Open")) {
						gameRoomKey = dataSnapshot.Key.ToString ();
						JoinRoom ();
						searchingRoom = false;
						return;
					}

				}

				CreateRoom ();
				searchingRoom = false;

			} else {
				Debug.Log ("has NO game rooms");
				CreateRoom ();
				searchingRoom = false;
			}

		}



	}


	private Action<bool> onSuccessMatchMake;


	public void SearchRoom (Action<bool> onResult)
	{
		//Order first to search fast
		roomReference.OrderByChild("RoomStatus").EqualTo("Open").ValueChanged += HandleChatroomChildAdded;
		searchingRoom = true;
		onSuccessMatchMake = onResult;

	}

	public void CancelRoomSearch ()
	{
		onSuccessMatchMake (false);
		searchingRoom = false;
	}

	public void CreateRoom ()
	{
		onSuccessMatchMake (true);
		gameRoomKey = reference.Child ("GameRoom").Push ().Key;
		MessageListener ();
		User user = new User (GameManager.Instance.userName, GameManager.Instance.life);
		Dictionary<string, System.Object> entryValues = user.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/GameRoom/" + gameRoomKey + "/InitialState/Home/param/"] = entryValues;
		reference.UpdateChildrenAsync (childUpdates);

		//set room status to open when create
		reference.Child ("GameRoom").Child (gameRoomKey).Child ("RoomStatus").SetValueAsync ("Open");

		InitialStateListener ();
		GameManager.Instance.isPlayerVisitor = false;
	}

	public void JoinRoom ()
	{
		onSuccessMatchMake (true);
		MessageListener ();

		User user = new User (GameManager.Instance.userName, GameManager.Instance.life);
		Dictionary<string, System.Object> entryValues = user.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/GameRoom/" + gameRoomKey + "/InitialState/Visitor/param/"] = entryValues;
		reference.UpdateChildrenAsync (childUpdates);

		//set room status to full when join
		reference.Child ("GameRoom").Child (gameRoomKey).Child ("RoomStatus").SetValueAsync ("Full");

		InitialStateListener ();
		GameManager.Instance.isPlayerVisitor = true;
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
			reference.Child ("GameRoom").Child (gameRoomKey).Child ("InitialState").Child ("Home").ChildAdded += HandleInitialHomeChildAdded;
			reference.Child ("GameRoom").Child (gameRoomKey).Child ("InitialState").Child ("Visitor").ChildAdded += HandleInitialVisitorChildAdded;
		}
	}

	public void AttackPlayer (string name, StatusType statusType, string param)
	{
		string	rpcKey = reference.Child ("GameRoom").Child (gameRoomKey).Child ("RPC").Push ().Key;

		BattleStatus battleStatus = new BattleStatus (name, (int)statusType, param);
		Dictionary<string, System.Object> entryValues = battleStatus.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/GameRoom/" + gameRoomKey + "/RPC/" + rpcKey] = entryValues;

		reference.UpdateChildrenAsync (childUpdates);
	}



}
