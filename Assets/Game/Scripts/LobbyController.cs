using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* UI For searching matches */
public class LobbyController : EnglishRoyaleElement
{
	public void SearchRoom ()
	{
		app.controller.audioController.PlayAudio (AudioEnum.ClickButton);
		app.controller.screenController.StartMatchingScreen ();
		app.component.firebaseDatabaseComponent.SearchRoom (delegate(bool result) {

			if (result) {
				GoToGameRoom ();	
			} else {
				Debug.Log ("Cancelled Search");
			}

			app.controller.screenController.StopMatchingScreen ();
		});


	}

	public void ModeOnChange (string modeName)
	{
		switch(EventSystem.current.currentSelectedGameObject.GetComponent<Toggle> ().name){
		case "Mode1":
			app.model.battleModel.modePrototype = ModeEnum.Mode1;
			break;
		case "Mode2":
			app.model.battleModel.modePrototype = ModeEnum.Mode2;
			break;
		case "Mode3":
			app.model.battleModel.modePrototype = ModeEnum.Mode3;
			break;
		case "Mode4":
			app.model.battleModel.modePrototype = ModeEnum.Mode4;
			break;
		}

	}

	public void CancelRoomSearch ()
	{
		app.component.firebaseDatabaseComponent.CancelRoomSearch ();
		app.controller.audioController.PlayAudio (AudioEnum.ClickButton);
	}


	private void GoToGameRoom ()
	{
		app.controller.audioController.PlayAudio (AudioEnum.Bgm);
		app.view.lobbyView.lobbyRoom.SetActive (false);
		app.view.lobbyView.gameRoomUI.SetActive (true);
		app.view.lobbyView.gameRoomAssets.SetActive (true);
		app.controller.battleController.StartPreTimer ();
		app.controller.screenController.StopLoadingScreen ();
	}


}
