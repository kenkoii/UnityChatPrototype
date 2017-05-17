﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour {

	public GameObject chatRoom;
	public GameObject lobbyRoom;
	public InputField userName;
	public Dropdown roomListDropDown;
	private string roomkeyName;

	void Start(){
//		roomListDropDown.onValueChanged.AddListener(delegate {
//			RoomListDropDownValueChangedHandler(roomListDropDown);
//		});
	}


	public void CreateRoom(){
		FirebaseDatabaseFacade.Instance.CreateRoom ();
		GameManager.Instance.userName = userName.text;
		GoToChatRoom();
	}

	public void JoinRoom(){
		FirebaseDatabaseFacade.Instance.JoinRoom (roomkeyName);
		GameManager.Instance.userName = userName.text;
		GoToChatRoom();
	}

	public void RoomListDropDownValueChangedHandler() {

		List<Dropdown.OptionData> menuOptions = roomListDropDown.options;
		roomkeyName = menuOptions [roomListDropDown.value].text;
		Debug.Log (roomkeyName);
	}

	public void ShowRoom(List<string> roomList){
		roomListDropDown.options.Clear ();
		for (int i = 0; i < roomList.Count; i++) {
			roomListDropDown.options.Add(new Dropdown.OptionData(roomList[i]));
		}
		roomkeyName = roomList [0];
		roomListDropDown.transform.Find("Label").GetComponent<Text>().text = "Select Room";
		roomListDropDown.RefreshShownValue();
	}

	private void GoToChatRoom (){
		lobbyRoom.SetActive (false);
		chatRoom.SetActive (true);
	}
}
