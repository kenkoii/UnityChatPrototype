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
public class FirebaseDatabaseComponent : SingletonMonoBehaviour<FirebaseDatabaseComponent>
{

	DatabaseReference reference = null;
	DatabaseReference roomReference;
	DatabaseReference connectionReference;

	string gameRoomKey = null;
	string	battleStatusKey = null;
	string receiveBattleKey = null;
	Dictionary<string, System.Object> receiveMessage;
	Dictionary<string, System.Object> receiveBattleStatus = null;
	private bool isHasGameRooms = false;
	private bool searchingRoom = false;
	public bool isHost = false;
	private bool isOnline = false;


	[System.Serializable] public class MessageBroadcast : UnityEvent<Dictionary<string, System.Object>>
	{

	};

	public MessageBroadcast sendMessage;
	public MessageBroadcast sendBattleStatus;
	public MessageBroadcast sendInitialHomeState;
	public MessageBroadcast sendInitialVisitorState;
	private Action<bool> onSuccessMatchMake;
	private bool isMatchMakeSuccess = false;

	DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;



	public Image dcImage;


	/// <summary>
	/// Start this instance. Check for dependencies if none
	/// </summary>
	void Start ()
	{
		
		ScreenController.Instance.StartLoadingScreen (delegate() {
			dependencyStatus = FirebaseApp.CheckDependencies ();
			if (dependencyStatus != DependencyStatus.Available) {
				FirebaseApp.FixDependenciesAsync ().ContinueWith (task => {
					dependencyStatus = FirebaseApp.CheckDependencies ();
					if (dependencyStatus == DependencyStatus.Available) {
						InitializeFirebase ();
					} else {
						ScreenController.Instance.StopLoadingScreen ();
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
	/// Initializes the firebase database.
	/// </summary>
	void InitializeFirebase ()
	{

		#if UNITY_EDITOR
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl (MyConst.URL_FIREBASE_DATABASE);
		#endif

	
		reference = FirebaseDatabase.DefaultInstance.RootReference;

		if (reference != null) {
			ScreenController.Instance.StopLoadingScreen ();
		}

		roomReference = reference.Child (MyConst.GAMEROOM_NAME);
		ConnectInternet ();
	}

	/// <summary>
	/// Connects to the firebase database.
	/// </summary>
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
	/// Handles the battle status added.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleBattleStatusAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		receiveBattleKey = args.Snapshot.Key;
	}

	/// <summary>
	/// Handles the battle status value changed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="args">Arguments.</param>
	void HandleBattleStatusValueChanged (object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
	
		receiveBattleStatus = (Dictionary<string, System.Object>)args.Snapshot.Child (receiveBattleKey).Value;
		if (receiveBattleStatus != null) {
			sendBattleStatus.Invoke (receiveBattleStatus);
		}

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
		ScreenController.Instance.StartLoadingScreen (delegate() {
			if (args.DatabaseError != null) {
				Debug.LogError (args.DatabaseError.Message);
				return;
			}

			receiveMessage = (Dictionary<string, System.Object>)args.Snapshot.Value;
			sendInitialVisitorState.Invoke (receiveMessage);
			isMatchMakeSuccess = true;
			onSuccessMatchMake (true);
			Debug.Log ("matching success: " + isMatchMakeSuccess);
		});
	
	}

	/// <summary>
	/// Handles the game room value changed. Also handles matchmaking
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
					//get prototype mode type from host
					GameData.Instance.modePrototype = (ModeEnum)int.Parse (dataSnapshot.Child (MyConst.GAMEROOM_PROTOTYPE_MODE).Value.ToString ());

					GameController.Instance.UpdateGame ();

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
		GameController.Instance.UpdateGame ();
		gameRoomKey = reference.Child (MyConst.GAMEROOM_NAME).Push ().Key;
		RoomCreateJoin (true, MyConst.GAMEROOM_HOME, MyConst.GAMEROOM_OPEN);

		//set prototype mode type
		reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_PROTOTYPE_MODE).SetValueAsync ("" + (int)GameData.Instance.modePrototype);

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
		GameData.Instance.isHost = isHost;
		MessageListener ();
		User user = new User (GameData.Instance.player.playerName,GameData.Instance.player.playerLife, GameData.Instance.player.playerGP);
		Dictionary<string, System.Object> entryValues = user.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_INITITAL_STATE + "/" + userPlace + "/param/"] = entryValues;
		reference.UpdateChildrenAsync (childUpdates);

		//set room status to open when create room
		reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_STATUS).SetValueAsync ("" + roomStatus);

		//set battle status to answer when start of game
		CheckInitialPhase ();

		InitialStateListener ();
	}

	public void UpdateBattleStatus (string stateName, int stateCount)
	{
		battleStatusKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Push ().Key;
		Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object> ();
		entryValues.Add (MyConst.BATTLE_STATUS_STATE, stateName);
		entryValues.Add (MyConst.BATTLE_STATUS_COUNT, "" + stateCount);
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_BATTLE_STATUS + "/" + battleStatusKey + "/"] = entryValues;
		reference.UpdateChildrenAsync (childUpdates);
	}

	//for mode 2
	public void UpdateAnswerBattleStatus (string stateName, int stateCount, int hTime, int hAnswer, int vTime, int vAnswer)
	{
		battleStatusKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Push ().Key;
		Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object> ();
		entryValues.Add (MyConst.BATTLE_STATUS_STATE, stateName);
		entryValues.Add (MyConst.BATTLE_STATUS_COUNT, "" + stateCount);
		entryValues.Add (MyConst.BATTLE_STATUS_HTIME, "" + hTime);
		entryValues.Add (MyConst.BATTLE_STATUS_HANSWER, "" + hAnswer);
		entryValues.Add (MyConst.BATTLE_STATUS_VTIME, "" + vTime);
		entryValues.Add (MyConst.BATTLE_STATUS_VANSWER, "" + vAnswer);
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_BATTLE_STATUS + "/" + battleStatusKey + "/"] = entryValues;
		reference.UpdateChildrenAsync (childUpdates);
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
	/// Sets the parameter to be sent to RPC table
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="param">Parameter.</param>
	public void SetParam (string param)
	{
		string	rpcKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC).Push ().Key;

		BattleStatus battleStatus = new BattleStatus (GameData.Instance.isHost, param);
		Dictionary<string, System.Object> entryValues = battleStatus.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_RPC + "/" + rpcKey] = entryValues;

		reference.UpdateChildrenAsync (childUpdates);
	}

	public void SetSkillParam (SkillDAO skill)
	{
		string	rpcKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC).Push ().Key;

		Dictionary<string, System.Object> entryValues = skill.ToDictionary ();
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates ["/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_RPC + "/" + rpcKey] = entryValues;

		reference.UpdateChildrenAsync (childUpdates);
	}


		
	/// <summary>
	/// Checks the initial battle phase.
	/// </summary>
	public void CheckInitialPhase ()
	{
		reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).RunTransaction (mutableData => {

			//get the battlekey, create if host
			if (mutableData.Value == null) {
				if (GameData.Instance.modePrototype == ModeEnum.Mode1) {
					UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
				} else if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
					UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);
				}
			} else {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)mutableData.Value;

				foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
					battleStatusKey = battleKey.Key;
				}

			}

			reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).ChildAdded += HandleBattleStatusAdded;
			reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).ValueChanged += HandleBattleStatusValueChanged;

			return TransactionResult.Success (mutableData);
		});
	}

	/// <summary>
	/// Answer Phase
	/// </summary>
	/// <param name="receiveTime">Receive time.</param>
	/// <param name="receiveAnswer">Receive answer.</param>
	public void AnswerPhase (int receiveTime, int receiveAnswer)
	{
		int modulusNum = 1;
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			modulusNum = 2;
		} else {
			modulusNum = 1;
		}


		GetLatestKey (modulusNum, delegate(string resultString) {

			Debug.Log (resultString);
			reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString).RunTransaction (mutableData => {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)mutableData.Value;

				string battleState = battleStatus [MyConst.BATTLE_STATUS_STATE].ToString ();
				int battleCount = int.Parse (battleStatus [MyConst.BATTLE_STATUS_COUNT].ToString ());


				if (battleState.Equals (MyConst.BATTLE_STATUS_ANSWER) && battleCount < 2) {
					battleCount++;
					battleStatus [MyConst.BATTLE_STATUS_COUNT] = battleCount.ToString ();

					if (isHost) {
						battleStatus [MyConst.BATTLE_STATUS_HANSWER] = receiveAnswer.ToString ();
						battleStatus [MyConst.BATTLE_STATUS_HTIME] = receiveTime.ToString ();
					} else {
						battleStatus [MyConst.BATTLE_STATUS_VANSWER] = receiveAnswer.ToString ();
						battleStatus [MyConst.BATTLE_STATUS_VTIME] = receiveTime.ToString ();
					}

					if (battleCount == 2) {
						if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
							UpdateBattleStatus (MyConst.BATTLE_STATUS_ATTACK, 0);
						} else {
							UpdateBattleStatus (MyConst.BATTLE_STATUS_SKILL, 0);
						}
					} 
				} 

				mutableData.Value = battleStatus;
				return TransactionResult.Success (mutableData);
			});


		});
	}

	/// <summary>
	/// Skills Phase. Increment skill count in battle status table
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="param">Parameter.</param>
	public void SkillPhase ()
	{
		int modulusNum = 2;
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			modulusNum = 1;
		} else {
			modulusNum = 2;
		}

		GetLatestKey (modulusNum, delegate(string resultString) {

			Debug.Log (resultString);
			reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString).RunTransaction (mutableData => {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)mutableData.Value;

				string battleState = battleStatus [MyConst.BATTLE_STATUS_STATE].ToString ();
				int battleCount = int.Parse (battleStatus [MyConst.BATTLE_STATUS_COUNT].ToString ());


				if (battleState.Equals (MyConst.BATTLE_STATUS_SKILL) && battleCount < 2) {
					battleCount++;
					battleStatus [MyConst.BATTLE_STATUS_COUNT] = battleCount.ToString ();
				
					if (battleCount == 2) {
						if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
							UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
						} else {
							UpdateBattleStatus (MyConst.BATTLE_STATUS_ATTACK, 0);
						}
					} 
				} 

				mutableData.Value = battleStatus;
				return TransactionResult.Success (mutableData);
			});
		});


	}

	/// <summary>
	/// Skill Phase. Increment attack count in battle status table
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="param">Parameter.</param>
	public void AttackPhase (string param)
	{
		GetLatestKey (3, delegate(string resultString) {
			
			Debug.Log (resultString);
			reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString).RunTransaction (mutableData => {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)mutableData.Value;

				string battleState = battleStatus [MyConst.BATTLE_STATUS_STATE].ToString ();
				int battleCount = int.Parse (battleStatus [MyConst.BATTLE_STATUS_COUNT].ToString ());


				if (battleState.Equals (MyConst.BATTLE_STATUS_ATTACK) && battleCount < 2) {
					SetParam (param);
					battleCount++;
					battleStatus [MyConst.BATTLE_STATUS_COUNT] = battleCount.ToString ();
				} 

				mutableData.Value = battleStatus;
				return TransactionResult.Success (mutableData);
			});


		});
			

	}


	/// <summary>
	/// Gets the latest key in battle status.
	/// </summary>
	/// <param name="numberMod">Number mod.</param>
	/// <param name="action">Action.</param>
	private void GetLatestKey (int numberMod, Action<string> action)
	{
		reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).GetValueAsync ().ContinueWith (task => {

			if (task.IsFaulted || task.IsFaulted) {

			} else {
				DataSnapshot snapshot = task.Result;
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)snapshot.Value;

				string latestKey = "";

				if (numberMod == 3) {
					if ((float)battleStatus.Count % 3 == 0) {

						foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
							latestKey = battleKey.Key;
						}
						action (latestKey);
					} else {
						GetLatestKey (numberMod, action);
					}
				}
				if (numberMod == 2) {
					if ((float)battleStatus.Count % 3 == 2) {

						foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
							latestKey = battleKey.Key;
						}
						action (latestKey);
					} else {
						GetLatestKey (numberMod, action);
					}
				}

				if (numberMod == 1) {
					if ((float)battleStatus.Count % 3 == 1) {

						foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
							latestKey = battleKey.Key;
						}
						action (latestKey);
					} else {
						GetLatestKey (numberMod, action);
					}
				}


			}
		});

	}



}
