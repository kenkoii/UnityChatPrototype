using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* UI For searching matches */
public class LobbyController : MonoBehaviour
{

	public GameObject gameRoomUI;
	public GameObject lobbyRoom;
	public GameObject gameRoomAssets;
	public GameObject prototypeModes;
	public InputField userName;
	BattleController battleController;

	void Start(){
		battleController = FindObjectOfType<BattleController> ();
	}

	public void SearchRoom ()
	{
		MyGlobalVariables.Instance.playerName = userName.text;
		MyGlobalVariables.Instance.playerLife = 30;
		MyGlobalVariables.Instance.playerGP = 0;
		MyGlobalVariables.Instance.playerMaxGP = 9;
		MyGlobalVariables.Instance.playerDamage = 5;

		switch (prototypeModes.GetComponent<Dropdown> ().value) {
		case 0:
			Debug.Log ("Mode 1 Chosen");
			MyGlobalVariables.Instance.modePrototype = 1;
			break;
		case 1:
			Debug.Log ("Mode 2 Chosen");
			MyGlobalVariables.Instance.modePrototype = 2;
			break;

		}
		EffectManager.Instance.StartMatchingScreen ();
		FirebaseDatabaseFacade.Instance.SearchRoom (delegate(bool result) {

			if (result) {
					GoToGameRoom ();	
			} else {
				Debug.Log ("Cancelled Search");
			}

			EffectManager.Instance.StopMatchingScreen ();
		});


	}

	public void CancelRoomSearch ()
	{
		FirebaseDatabaseFacade.Instance.CancelRoomSearch ();
	}


	private void GoToGameRoom ()
	{
			lobbyRoom.SetActive (false);
			gameRoomUI.SetActive (true);
			gameRoomAssets.SetActive (true);
			battleController.StartPreTimer ();
			EffectManager.Instance.StopLoadingScreen();
	}
}
