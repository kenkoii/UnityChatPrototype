using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

//using Firebase;

public class FDController : SingletonMonoBehaviour<FDController>,IRPCDicObserver
{
	DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	DatabaseReference roomReference;
	private bool searchingRoom = false;
	private Action<bool> onSuccessMatchMake;
	private int joinCounter = 0;
	string gameRoomKey = null;
	string battleStatusKey = null;

	void Start ()
	{
		SetReference ();
	}

	private void SetReference ()
	{
		FDFacade.Instance.SetUnityEditorReference (MyConst.URL_FIREBASE_DATABASE);
	}

	//Do something when recives rpc from facade
	public void OnNotify (Dictionary<string, System.Object> rpcReceive)
	{

	}

	public void SearchRoom (Action<bool> onResult)
	{
		RPCDicObserver.AddObserver (this);
		//Order first to search fast
		FDFacade.Instance.CreateTableValueChangedListener ("SearchRoom", roomReference.OrderByChild (MyConst.GAMEROOM_STATUS).EqualTo ("0"));
		searchingRoom = true;
		onSuccessMatchMake = onResult;
	}

	private void CreateRoom ()
	{
		GameController.Instance.UpdateGame ();
		gameRoomKey = FDFacade.Instance.CreateKey (reference.Child (MyConst.GAMEROOM_NAME));
		RoomCreateJoin (true, MyConst.GAMEROOM_HOME);

		//set prototype mode type
		FDFacade.Instance.SetTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_PROTOTYPE_MODE), "" + (int)GameData.Instance.modePrototype);
	}

	public void JoinRoom ()
	{
		FDFacade.Instance.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_STATUS), delegate(MutableData mutableData) {
			int playerCount = int.Parse (mutableData.Value.ToString ());

			playerCount++;
			joinCounter++;

			mutableData.Value = playerCount.ToString ();
		});
			
		StartCoroutine (StartJoinDelay ());
	}

	IEnumerator StartJoinDelay ()
	{
		yield return new WaitForSeconds (5);
		Debug.Log ("JoinCounter " + joinCounter);
		if (joinCounter < 2) {
			RoomCreateJoin (false, MyConst.GAMEROOM_VISITOR);
		} else {
			LobbyController.Instance.SearchRoom ();
		}

		joinCounter = 0;
	}

	private void RoomCreateJoin (bool isHost, string userPlace)
	{
		GameData.Instance.isHost = isHost;
		RPCListener ();

		Dictionary<string, System.Object> entryValues = GameData.Instance.player.ToDictionary ();

		string directory = "/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_INITITAL_STATE + "/" + userPlace + "/param/";
		FDFacade.Instance.CreateTableChildrenAsync (directory, reference, entryValues);


		//set room status to open when create room
		if (isHost) {
			FDFacade.Instance.SetTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_STATUS), "0");
		}
		//set battle status to answer when start of game
		CheckInitialPhase ();

		InitialStateListener ();
	}

	private void RPCListener ()
	{
		if (gameRoomKey != null) {
			FDFacade.Instance.CreateTableChildAddedListener ("RPCListener", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC));
		}
	}

	private void CheckInitialPhase ()
	{
		FDFacade.Instance.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS), delegate(MutableData mutableData) {
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

			FDFacade.Instance.CreateTableChildAddedListener ("BattleStatusChildAdded", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS));
			FDFacade.Instance.CreateTableValueChangedListener ("BattleStatusValueChanged", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS));
		});
	}

	private void InitialStateListener ()
	{
		if (gameRoomKey != null) {
			FDFacade.Instance.CreateTableChildAddedListener ("HomeListener", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_INITITAL_STATE).Child (MyConst.GAMEROOM_HOME));
			FDFacade.Instance.CreateTableChildAddedListener ("VisitorListener", reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_INITITAL_STATE).Child (MyConst.GAMEROOM_VISITOR));
		}
	}

	public void UpdateBattleStatus (string stateName, int stateCount)
	{
		battleStatusKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Push ().Key;
		Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object> ();
		entryValues.Add (MyConst.BATTLE_STATUS_STATE, stateName);
		entryValues.Add (MyConst.BATTLE_STATUS_COUNT, "" + stateCount);

		string directory = "/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_BATTLE_STATUS + "/" + battleStatusKey + "/";
		FDFacade.Instance.CreateTableChildrenAsync (directory, reference, entryValues);
	}

	//for mode 2
	public void UpdateAnswerBattleStatus (string stateName, int stateCount, int hTime, int hAnswer, int vTime, int vAnswer)
	{
		battleStatusKey = FDFacade.Instance.CreateKey (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS));
		Dictionary<string, System.Object> entryValues = new Dictionary<string, System.Object> ();
		entryValues.Add (MyConst.BATTLE_STATUS_STATE, stateName);
		entryValues.Add (MyConst.BATTLE_STATUS_COUNT, "" + stateCount);
		entryValues.Add (MyConst.BATTLE_STATUS_HTIME, "" + hTime);
		entryValues.Add (MyConst.BATTLE_STATUS_HANSWER, "" + hAnswer);
		entryValues.Add (MyConst.BATTLE_STATUS_VTIME, "" + vTime);
		entryValues.Add (MyConst.BATTLE_STATUS_VANSWER, "" + vAnswer);

		string directory = "/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_BATTLE_STATUS + "/" + battleStatusKey + "/";
		FDFacade.Instance.CreateTableChildrenAsync (directory, reference, entryValues);
	}

	private void DeleteRoom ()
	{
		FDFacade.Instance.RemoveTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey));
		gameRoomKey = null;
		searchingRoom = false;
		onSuccessMatchMake (false);
	}

	public void SetAttackParam (AttackModel attack)
	{
		SetParam (attack.ToDictionary ());
	}

	public void SetAnswerParam (AnswerModel answer)
	{
		SetParam (answer.ToDictionary ());
	}

	public void SetGestureParam (GestureModel gesture)
	{
		SetParam (gesture.ToDictionary ());
	}

	public void SetSkillParam (SkillModel skill)
	{
		SetParam (skill.ToDictionary ());
	}

	private void SetParam (Dictionary<string, System.Object> toDictionary)
	{
		string	rpcKey = reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_RPC).Push ().Key;

		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["userHome"] = GameData.Instance.isHost;
		result ["param"] = toDictionary;

		string directory = "/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_RPC + "/" + rpcKey;
		FDFacade.Instance.CreateTableChildrenAsync (directory, reference, result);
	}

	public void AnswerPhase (int receiveTime, int receiveAnswer)
	{
		int modulusNum = 1;
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			modulusNum = 2;
		} else {
			modulusNum = 1;
		}
		GetLatestKey (modulusNum, delegate(string resultString) {
			FDFacade.Instance.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString), delegate(MutableData mutableData) {

				mutableData.Value = PhaseMutate (mutableData, MyConst.BATTLE_STATUS_ANSWER, delegate(Dictionary<string, System.Object> battleStatus, int battleCount) {
					if (GameData.Instance.isHost) {
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
				});
			});
		});
	}


	public void SkillPhase ()
	{
		int modulusNum = 2;
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			modulusNum = 1;
		} else {
			modulusNum = 2;
		}

		GetLatestKey (modulusNum, delegate(string resultString) {
			FDFacade.Instance.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString), delegate(MutableData mutableData) {


				mutableData.Value = PhaseMutate (mutableData, MyConst.BATTLE_STATUS_SKILL, delegate(Dictionary<string, System.Object> battleStatus, int battleCount) {
					if (battleCount == 2) {
						if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
							UpdateAnswerBattleStatus (MyConst.BATTLE_STATUS_ANSWER, 0, 0, 0, 0, 0);
						} else {
							UpdateBattleStatus (MyConst.BATTLE_STATUS_ATTACK, 0);
						}
					} 
				});
			});
		});
	}


	public void AttackPhase (AttackModel param)
	{
		GetLatestKey (3, delegate(string resultString) {
			FDFacade.Instance.RunTransaction (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS).Child (resultString), delegate(MutableData mutableData) {
				mutableData.Value = PhaseMutate (mutableData, MyConst.BATTLE_STATUS_ATTACK, delegate(Dictionary<string, System.Object> battleStatus, int battleCount) {
					SetAttackParam (param);
				});
			});
		});
	}

	private Dictionary<string, System.Object> PhaseMutate (MutableData mutableData, string battleStatusName, Action<Dictionary<string, System.Object>,int> action)
	{
		Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)mutableData.Value;
		string battleState = battleStatus [MyConst.BATTLE_STATUS_STATE].ToString ();
		int battleCount = int.Parse (battleStatus [MyConst.BATTLE_STATUS_COUNT].ToString ());

		if (battleState.Equals (battleStatusName) && battleCount < 2) {

			battleCount++;
			battleStatus [MyConst.BATTLE_STATUS_COUNT] = battleCount.ToString ();
			action (battleStatus, battleCount);
		} 
			
		return battleStatus;
	}


	private void GetLatestKey (int numMod, Action<string> action)
	{
		FDFacade.Instance.GetTableValueAsync (reference.Child (MyConst.GAMEROOM_NAME).Child (gameRoomKey).Child (MyConst.GAMEROOM_BATTLE_STATUS), delegate(DataSnapshot dataSnapshot) {
		

			if (dataSnapshot != null) {
				Dictionary<string, System.Object> battleStatus = (Dictionary<string, System.Object>)dataSnapshot.Value;
				switch (numMod) {
				case 1:
					LatestKeyCompute (battleStatus, 1, 1, action);
					break;
				case 2:
					LatestKeyCompute (battleStatus, 2, 2, action);
					break;
				case 3:
					LatestKeyCompute (battleStatus, 3, 0, action);
					break;
				}
			}

		});

	}

	private void LatestKeyCompute (Dictionary<string, System.Object> battleStatus, int numMod, int numCompare, Action<string> action)
	{
		string latestKey = "";

		if ((float)battleStatus.Count % 3 == numCompare) {
			foreach (KeyValuePair<string , System.Object> battleKey in battleStatus) {
				latestKey = battleKey.Key;
			}
			action (latestKey);
		} else {
			GetLatestKey (numMod, action);
		}
	}

}
