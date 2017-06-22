using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LobbyView : EnglishRoyaleElement
{
	public GameObject gameRoomUI;
	public GameObject lobbyRoom;
	public GameObject gameRoomAssets;
	public ToggleGroup toggleGroup;

	public void SearchRoom ()
	{
		
		app.controller.lobbyController.SearchRoom ();
	}

	public void ModeOnChange ()
	{
		foreach(Toggle tg in toggleGroup.ActiveToggles())
		{
			app.controller.lobbyController.ModeOnChange(tg.name);
		}
	}

	public void CancelRoomSearch ()
	{
		app.controller.lobbyController.CancelRoomSearch ();
	}
		
}