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

//	public void CreateRoom(){
//		GameManager.Instance.userName = userName.text;
//		GameManager.Instance.life = int.Parse(userLife.text);
//		FirebaseDatabaseFacade.Instance.CreateRoom (GameManager.Instance.userName, GameManager.Instance.life);
//		GoToChatRoom();
//	}
//
//	public void JoinRoom(){
//		GameManager.Instance.userName = userName.text;
//		GameManager.Instance.life = int.Parse(userLife.text);
//		FirebaseDatabaseFacade.Instance.JoinRoom (roomkeyName, GameManager.Instance.userName, GameManager.Instance.life);
//		GoToChatRoom();
//	}

	public void SearchRoom(){
		GameManager.Instance.userName = userName.text;
		GameManager.Instance.life = int.Parse(userLife.text);
		FirebaseDatabaseFacade.Instance.SearchRoom (GameManager.Instance.userName, GameManager.Instance.life, delegate(bool result){
			if(result){
				GoToChatRoom();
			}else{
				
			}
			
		});

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
