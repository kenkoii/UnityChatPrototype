using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* UI For searching matches */
public class LobbyController : MonoBehaviour
{

	public GameObject gameRoomUI;
	public GameObject lobbyRoom;
	public GameObject gameRoomAssets;
	public GameObject prototypeModes;
	public GameObject toggleGroup;
	BattleController battleController;

	void Start ()
	{
		battleController = FindObjectOfType<BattleController> ();
	}

	public void SearchRoom ()
	{
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

	public void ModeOnChange ()
	{
		switch(EventSystem.current.currentSelectedGameObject.GetComponent<Toggle> ().name){
		case "Mode1":
			MyGlobalVariables.Instance.modePrototype = ModeEnum.Mode1;
			break;
		case "Mode2":
			MyGlobalVariables.Instance.modePrototype = ModeEnum.Mode2;
			break;
		case "Mode3":
			MyGlobalVariables.Instance.modePrototype = ModeEnum.Mode3;
			break;
		case "Mode4":
			MyGlobalVariables.Instance.modePrototype = ModeEnum.Mode4;
			break;
		}

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
		EffectManager.Instance.StopLoadingScreen ();
	}
}
