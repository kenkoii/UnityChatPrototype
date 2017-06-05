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
	public InputField userName;
	public InputField userLife;
	public InputField playerGP;

	BattleController battleController;

	void Start(){
		battleController = FindObjectOfType<BattleController> ();
	}

	public void SearchRoom ()
	{
		GameManager.Instance.playerName = userName.text;
		GameManager.Instance.playerLife = int.Parse (userLife.text);
		GameManager.Instance.playerGP = int.Parse (playerGP.text);
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
