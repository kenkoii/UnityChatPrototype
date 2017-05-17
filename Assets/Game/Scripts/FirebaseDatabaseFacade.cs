using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class FirebaseDatabaseFacade : SingletonMonoBehaviour<FirebaseDatabaseFacade> {

	void Start() {
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://chatprototype-39807.firebaseio.com");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
	}


}
