using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using UnityEngine.Events;



public class FDFacade : SingletonMonoBehaviour<FDFacade>
{
	
	private Dictionary<string, DatabaseReference> subscriberReference = new Dictionary<string, DatabaseReference>();
	private Dictionary<string, Query> subscriberQuery = new Dictionary<string, Query>();



	//Check if there is connection to firebase
	public void CheckDBConnection (DatabaseReference dbReference)
	{
		dbReference.ValueChanged += HandleDatabaseConnection;
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
	public void CreateTableValueChangedListener (string subscriberName, DatabaseReference reference)
	{
		if (subscriberReference.ContainsKey (subscriberName)) {
			return;
		}
		subscriberReference.Add (subscriberName, reference);
		subscriberReference[subscriberName].ValueChanged+= HandleTableValueChanged;
	}

	public void RemoveReference (string subscriberName)
	{
		subscriberReference.Remove (subscriberName);
	}

	private void HandleTableValueChanged (object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		RPC.Instance.ReceiveRPC (args.Snapshot);
	}

	public void QueryTable (string subscriberName, Query query)
	{
		if (subscriberQuery.ContainsKey (subscriberName)) {
			return;
		}
		subscriberQuery.Add (subscriberName, query);
		subscriberQuery[subscriberName].ValueChanged+= HandleQuery;
	}

	public void RemoveQuery (string subscriberName)
	{
		subscriberQuery.Remove (subscriberName);
	}


	private void HandleQuery (object sender, ValueChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		RPC.Instance.ReceiveRPCQuery (args.Snapshot);
	}


	//Add listener to a table ChildAdded
	public void CreateTableChildAddedListener (string subscriberName, DatabaseReference reference)
	{
		if (subscriberReference.ContainsKey (subscriberName)) {
			return;
		}
		subscriberReference.Add (subscriberName, reference);
		subscriberReference[subscriberName].ChildAdded+= HandleTableChildAdded;
	}

	private void HandleTableChildAdded (object sender, ChildChangedEventArgs args)
	{
		if (args.DatabaseError != null) {
			Debug.LogError (args.DatabaseError.Message);
			return;
		}
		RPC.Instance.ReceiveRPC (args.Snapshot);
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
	public void GetTableValueAsync (DatabaseReference reference, Action<DataSnapshot> dataSnapshot)
	{
		reference.GetValueAsync ().ContinueWith (task => {

			if (task.IsFaulted || task.IsCanceled) {
			} else {
				dataSnapshot(task.Result);
			}
		});

	}

	//Set table once
	public void SetTableValueAsync (DatabaseReference reference, object objectValue)
	{
		reference.SetValueAsync (objectValue);
	}

	public void RemoveTableValueAsync (DatabaseReference reference)
	{
		reference.RemoveValueAsync ();
	}

	//Create a key from table
	public string CreateKey (DatabaseReference reference)
	{
		return reference.Push ().Key;
	}
}
