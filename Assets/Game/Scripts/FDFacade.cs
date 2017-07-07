using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using Firebase.Unity.Editor;
#endif

public class FDFacade : SingletonMonoBehaviour<FDFacade>
{
	private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
	private DatabaseReference connectionReference;
	private string unityEditorReference;

	//set unity reference
	public void SetUnityEditorReference (string reference)
	{
		unityEditorReference = reference;
	}

	public void StartFirebase ()
	{
		//search dependency
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
	}
	//
	private void InitializeFirebase ()
	{

		#if UNITY_EDITOR
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl (unityEditorReference);
		#endif


		CheckDBConnection ();
	}

	//Check if there is connection to firebase
	private void CheckDBConnection ()
	{
		connectionReference = FirebaseDatabase.DefaultInstance.GetReferenceFromUrl (MyConst.URL_FIREBASE_DATABASE_CONNECTION);
		connectionReference.ValueChanged += HandleDatabaseConnection;
	}

	private void HandleDatabaseConnection (object sender, ValueChangedEventArgs args)
	{

		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		RPC.Instance.ReceiveDBConnection ((bool)args.Snapshot.Value);
	}

	//Create table using childrenasync
	public void CreateTableChildrenAsync (string directory,DatabaseReference reference, Dictionary<string, System.Object> entryValues)
	{
		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object> ();
		childUpdates [directory] = entryValues;
		//example
		//childUpdates ["/" + MyConst.GAMEROOM_NAME + "/" + gameRoomKey + "/" + MyConst.GAMEROOM_INITITAL_STATE + "/" + userPlace + "/param/"] = entryValues;
		reference.UpdateChildrenAsync (childUpdates);
	}

	//Add listener to a table ValueChange
	public void CreateTableValueChangedListener (DatabaseReference reference)
	{
		reference.ValueChanged += HandleTableValueChanged;
	}

	void HandleTableValueChanged (object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		RPC.Instance.ReceiveRPC ((Dictionary<string, System.Object>)args.Snapshot.Value);
	}


	//Add listener to a table ChildAdded
	private void CreateTableChildAddedListener (DatabaseReference reference)
	{
		reference.ChildAdded += HandleTableChildAdded;
	}

	void HandleTableChildAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		RPC.Instance.ReceiveRPC ((Dictionary<string, System.Object>)args.Snapshot.Value);
	}
		
	//Run a transaction
	public void RunTransaction (DatabaseReference reference, Action<MutableData> action)
	{
		reference.RunTransaction (mutableData => {

			action (mutableData);

			return TransactionResult.Success (mutableData);
		});
	}

	//Read from table once
	public Dictionary<string, System.Object> GetTableValueAsync (DatabaseReference reference, Action<string> action)
	{
		DataSnapshot snapshot = null;
		reference.GetValueAsync ().ContinueWith (task => {

			if (task.IsFaulted || task.IsCanceled) {
			} else {
				snapshot = task.Result;
			}
		});

		return (Dictionary<string, System.Object>)snapshot.Value;
	}
	//Create a key from table
	public string CreateKey (DatabaseReference reference)
	{
		return reference.Push ().Key;
	}
}
