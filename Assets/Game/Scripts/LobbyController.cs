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
		MyGlobalVariables.Instance.modePrototype = (ModeEnum)prototypeModes.GetComponent<Dropdown> ().value;
		GameController.Instance.UpdateGame ();
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
