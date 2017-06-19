using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LobbyView : EnglishRoyaleElement
{
	public GameObject gameRoomUI;
	public GameObject lobbyRoom;
	public GameObject gameRoomAssets;

	public void SearchRoom ()
	{
		
		app.controller.lobbyController.SearchRoom (lobbyRoom, gameRoomUI, gameRoomAssets);
	}

	public void ModeOnChange ()
	{
		app.controller.lobbyController.ModeOnChange(EventSystem.current.currentSelectedGameObject.GetComponent<Toggle> ().name);

	}

	public void CancelRoomSearch ()
	{
		app.controller.lobbyController.CancelRoomSearch ();
	}
		
}
