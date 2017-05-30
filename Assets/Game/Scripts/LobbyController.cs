using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* UI For searching matches */
public class LobbyController : MonoBehaviour
{

	public GameObject gameRoom;
	public GameObject lobbyRoom;
	public InputField userName;
	public InputField userLife;
	public InputField playerGP;


	public void SearchRoom ()
	{
		GameManager.Instance.userName = userName.text;
		GameManager.Instance.life = int.Parse (userLife.text);
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
		gameRoom.SetActive (true);
		EffectManager.Instance.StopLoadingScreen();
	}
}
