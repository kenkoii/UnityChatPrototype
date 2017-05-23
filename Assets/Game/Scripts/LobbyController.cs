using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

	public GameObject loadingScreen;
	public GameObject gameRoom;
	public GameObject lobbyRoom;
	public InputField userName;
	public InputField userLife;


	public void SearchRoom(){
		GameManager.Instance.userName = userName.text;
		GameManager.Instance.life = int.Parse(userLife.text);
		loadingScreen.SetActive (true);
		FirebaseDatabaseFacade.Instance.SearchRoom (delegate(bool result){

			if(result){
				GoToGameRoom();
			}else{
				Debug.Log("Cancelled Search");
			}

			loadingScreen.SetActive(false);
		});
	}

	public void CancelRoomSearch(){
		FirebaseDatabaseFacade.Instance.CancelRoomSearch ();
	}
		

	private void GoToGameRoom (){
		lobbyRoom.SetActive (false);
		gameRoom.SetActive (true);
	}
}
