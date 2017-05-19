using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

	public GameObject chatRoom;
	public GameObject lobbyRoom;
	public InputField userName;
	public InputField userLife;
	public Dropdown roomListDropDown;
	private string roomkeyName;


	public void SearchRoom(){
		GameManager.Instance.userName = userName.text;
		GameManager.Instance.life = int.Parse(userLife.text);
		FirebaseDatabaseFacade.Instance.SearchRoom (GameManager.Instance.userName, GameManager.Instance.life, delegate(bool result){
			if(result){
				GoToChatRoom();
			}else{
				Debug.Log("Cancelled Search");
			}
			
		});

	}

	public void CancelRoomSearch(){
		FirebaseDatabaseFacade.Instance.CancelRoomSearch ();
	}

	public void RoomListDropDownValueChangedHandler() {

		List<Dropdown.OptionData> menuOptions = roomListDropDown.options;
		roomkeyName = menuOptions [roomListDropDown.value].text;
		Debug.Log (roomkeyName);
	}
		

	private void GoToChatRoom (){
		lobbyRoom.SetActive (false);
		chatRoom.SetActive (true);
	}
}
