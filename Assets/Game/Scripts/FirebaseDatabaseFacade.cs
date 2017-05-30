using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
using Firebase.Unity.Editor;
#endif

using System;

/* Facade for Firebase Database */
public class FirebaseDatabaseFacade : SingletonMonoBehaviour<FirebaseDatabaseFacade>
{

	DatabaseReference reference = null;
	DatabaseReference roomReference;
	DatabaseReference connectionReference;

	string gameRoomKey = null;
	Dictionary<string, System.Object> receiveMessage;
	private bool isHasGameRooms = false;
	private bool searchingRoom = false;
	private bool isHost = false;
	private bool isOnline = false;

	[System.Serializable] public class MessageBroadcast : UnityEvent<Dictionary<string, System.Object>>
	{

	};

	public MessageBroadcast sendMessage;
	public MessageBroadcast sendInitialHomeState;
	public MessageBroadcast sendInitialVisitorState;
	private Action<bool> onSuccessMatchMake;
	private bool isMatchMakeSuccess = false;

	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;


	public Image dcImage;


	void Start ()
	{
		EffectManager.Instance.StartLoadingScreen (delegate() {
			dependencyStatus = FirebaseApp.CheckDependencies ();
			if (dependencyStatus != DependencyStatus.Available) {
				FirebaseApp.FixDependenciesAsync ().ContinueWith (task => {
					dependencyStatus = FirebaseApp.CheckDependencies ();
					if (dependencyStatus == DependencyStatus.Available) {
						InitializeFirebase ();
					} else {
						EffectManager.Instance.StopLoadingScreen ();
						Debug.LogError (
							"Could not resolve all Firebase dependencies: " + dependencyStatus);
					}
				});
			} else {
				
				InitializeFirebase ();
			}
		});

	}

	/// <summary>
	/// Initializes the firebase.
	/// </summary>
	void InitializeFirebase ()
	{


		#if UNITY_EDITOR
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl (MyConst.URL_FIREBASE_DATABASE);
		#endif

	
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		if (reference != null) {
			EffectManager.Instance.StopLoadingScreen();
		}

		roomReference = reference.Child (MyConst.GAMEROOM_NAME);
		ConnectInternet ();
	}

	public void ConnectInternet ()
	{
		connectionReference = FirebaseDatabase.DefaultInstance.GetReferenceFromUrl (MyConst.URL_FIREBASE_DATABASE_CONNECTION);
		connectionReference.ValueChanged += HandleDatabaseConnection;
	}

	/// <summary>
	/// Handles the database connection.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleDatabaseConnection (object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}

		bool connected = (bool)args.Snapshot.Value;
		if (connected) {
			isOnline = true;
		} else {
			isOnline = false;
		}

		Debug.Log ("DEVICE IS ONLINE?: " + isOnline);

		dcImage.enabled = !isOnline;
	}


	/// <summary>
	/// Handles the RPC added.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleRPCAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		receiveMessage = (Dictionary<string, System.Object>)args.Snapshot.Value;
		sendMessage.Invoke (receiveMessage);

	}

	/// <summary>
	/// Handles the initial home child added.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleInitialHomeChildAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		receiveMessage = (Dictionary<string, System.Object>)args.Snapshot.Value;
		sendInitialHomeState.Invoke (receiveMessage);

	}

	/// <summary>
	/// Handles the initial visitor child added.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleInitialVisitorChildAdded (object sender, ChildChangedEventArgs args)
	{
		EffectManager.Instance.StartLoadingScreen (delegate() {
			if (args.DatabaseError != null) {
				Debug.LogError (args.DatabaseError.Message);
				return;
			}

			receiveMessage = (Dictionary<string, System.Object>)args.Snapshot.Value;
			sendInitialVisitorState.Invoke (receiveMessage);
			isMatchMakeSuccess = true;
			onSuccessMatchMake (isMatchMakeSuccess);
		});
	
	}

	/// <summary>
	/// Handles the game room value changed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleGameRoomValueChanged (object sender, ValueChangedEventArgs args)
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
					if (dataSnapshot.Child (MyConst.GAMEROOM_STATUS).Value.ToString ().Equals (MyConst.GAMEROOM_OPEN)) {

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

	/// <summary>
	/// Searchs the room.
	/// </summary>
	/// <param name="onResult">On result.</param>
	public void SearchRoom (Action<bool> onResult)
	{
		//Order first to search fast
		roomReference.OrderByChild (MyConst.GAMEROOM_STATUS).EqualTo (MyConst.GAMEROOM_OPEN).ValueChanged += HandleGameRoomValueChanged;
		searchingRoom = true;
		onSuccessMatchMake = onResult;
	}

	/// <summary>
	/// Cancels the searching of room
	/// </summary>
	/// <returns><c>true</c> if this instance cancel room search; otherwise, <c>false</c>.</returns>

	public void CancelRoomSearch ()
	{
		if (!isMatchMakeSuccess) {
			
			if (isHost) {
				DeleteRoom ();
			} 
		}
	}

	/// <summary>
	/// Deletes the room.
	/// </summary>
	private void DeleteRoom ()
	{
		reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).RemoveValueAsync ();
		gameRoomKey = null;
		searchingRoom = false;
		onSuccessMatchMake (false);
	}

	/// <summary>
	/// Creates the room.
	/// </summary>
	private void CreateRoom ()
	{
		gameRoomKey = reference.Child (MyConst.GAMEROOM_NAME).Push ().Key;
		RoomCreateJoin (true, MyConst.GAMEROOM_HOME, MyConst.GAMEROOM_OPEN);
	}

	/// <summary>
	/// Joins the room.
	/// </summary>
	private void JoinRoom ()
	{
		RoomCreateJoin (false, MyConst.GAMEROOM_VISITOR, MyConst.GAMEROOM_FULL);
	}

	/// <summary>
	/// Rooms the create join.
	/// </summary>
	/// <param name="isHost">If set to <c>true</c> is host.</param>
	/// <param name="userPlace">User place.</param>
	/// <param name="roomStatus">Room status.</param>
	private void RoomCreateJoin (bool isHost, string userPlace, string roomStatus)
	{
		this.isHost = isHost;
		MessageListener ();
		User user = new User (GameManager.Instance.userName, GameManager.Instance.life);
		Dictionary<string, System.Object> entryValues = user.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_INITITAL_STATE + "/" + userPlace + "/param/"] = entryValues;
		reference.UpdateChildrenAsync (childUpdates);

		//set room status to open when create room
		reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_STATUS).SetValueAsync ("" + roomStatus);

		//set battle status to preparation when start of game
		reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).SetValueAsync ("preparation");


		InitialStateListener ();
		GameManager.Instance.isPlayerVisitor = !isHost;
	}

	/// <summary>
	/// listens when there is activity inside the RPC table in the same room
	/// </summary>
	private void MessageListener ()
	{
		if (gameRoomKey != null) {
			reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC).ChildAdded += HandleRPCAdded;
		}
	}

	/// <summary>
	/// Listens to initial states for home and visitor in the same room
	/// </summary>
	private void InitialStateListener ()
	{
		if (gameRoomKey != null) {
			reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_INITITAL_STATE).Child (MyConst.GAMEROOM_HOME).ChildAdded += HandleInitialHomeChildAdded;
			reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_INITITAL_STATE).Child (MyConst.GAMEROOM_VISITOR).ChildAdded += HandleInitialVisitorChildAdded;
		}
	}

	/// <summary>
	/// Attacks the player. Sends info to firebase database
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="statusType">Status type.</param>
	/// <param name="param">Parameter.</param>
	public void AttackPlayer (string name, string param)
	{
		//only attack if online in multiplayer
		if (isOnline) {
			string	rpcKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC).Push ().Key;

			BattleStatus battleStatus = new BattleStatus (name, param);
			Dictionary<string, System.Object> entryValues = battleStatus.ToDictionary ();
			Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
			childUpdates ["/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_RPC + "/" + rpcKey] = entryValues;

			reference.UpdateChildrenAsync (childUpdates);
		}
	}

	//	public void AttackPlayer (string name, StatusType statusType, string param){
	//
	//		string	rpcKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC).Push ().Key;
	//		Debug.Log (rpcKey);
	//		Debug.Log ("hello");
	//		reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC ).Child (rpcKey).RunTransaction(mutableData => {
	//			List<object> leaders = mutableData.Value as List<object>;
	////				if (leaders == null) {
	////					leaders = new List<object>();
	////				} else if (mutableData.ChildrenCount >= MaxScores) {
	////					long minScore = long.MaxValue;
	////					object minVal = null;
	////					foreach (var child in leaders) {
	////						if (!(child is Dictionary<string, object>)) continue;
	////						long childScore = (long)
	////							((Dictionary<string, object>)child)["score"];
	////						if (childScore < minScore) {
	////							minScore = childScore;
	////							minVal = child;
	////						}
	////					}
	////					if (minScore > score) {
	////						// The new score is lower than the existing 5 scores, abort.
	////						return TransactionResult.Abort();
	////					}
	////
	////					// Remove the lowest score.
	////					leaders.Remove(minVal);
	////				}
	//
	//					// Add the new high score.
	//
	//
	//			BattleStatus battleStatus = new BattleStatus (name, (int)statusType, param);
	//			Dictionary<string, System.Object> entryValues = battleStatus.ToDictionary ();
	//
	//			leaders.Add(entryValues);
	//			mutableData.Value = leaders;
	//			return TransactionResult.Success(mutableData);
	//		});
	//	}

}
