using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
/* UI For searching matches */
public class LobbyController : SingletonMonoBehaviour<LobbyController>
{
	public GameObject gameRoomUI;
	public GameObject lobbyRoom;
	public GameObject gameRoomAssets;
	public ToggleGroup toggleGroup;
	private int timeLeft = 3;
	private bool stoptimer = true;

	void Start(){

		List<string> list = new List<string> ();
		list = CSVParser.ParseCSV ("selectTyping");
		Debug.Log (list [50] [0]);
		}
	public void SearchRoom ()
	{
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
		ScreenController.Instance.StartMatchingScreen ();
		FirebaseDatabaseComponent.Instance.SearchRoom (delegate(bool result) {
			if (result) {
				GoToGameRoom ();	
			} else {
				Debug.Log ("Cancelled Search");
			}
			ScreenController.Instance.StopMatchingScreen ();
		});
	}

	public void OnModeChange ()
	{
		foreach (Toggle tg in toggleGroup.ActiveToggles()) {

			ModeEnum modeChosen = ModeEnum.Mode1;
			switch (tg.name) {

			case "Mode1":
				modeChosen = ModeEnum.Mode1;
				break;
			case "Mode2":
				modeChosen = ModeEnum.Mode2;
				break;
			}

			GameData.Instance.modePrototype = modeChosen;

		}



	}

	public void CancelRoomSearch ()
	{
		FirebaseDatabaseComponent.Instance.CancelRoomSearch ();
		AudioController.Instance.PlayAudio (AudioEnum.ClickButton);
	}


	private void GoToGameRoom ()
	{
		AudioController.Instance.PlayAudio (AudioEnum.Bgm);
		lobbyRoom.SetActive (false);
		gameRoomUI.SetActive (true);
		gameRoomAssets.SetActive (true);
		ScreenController.Instance.StopLoadingScreen ();
		StartPreTimer ();
		CameraWorksController.Instance.StartIntroCamera ();
		RPCDicObserver.AddObserver (GestureController.Instance);
		RPCDicObserver.AddObserver (BattleStatusManager.Instance);
		RPCDicObserver.AddObserver(BattleView.Instance);
	}

	/// <summary>
	/// Delay before start of battle
	/// </summary>
	public void StartPreTimer ()
	{
		timeLeft = 3;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			GameTimerView.Instance.ToggleTimer (true);
			if (timeLeft > 0) {
				GameTimerView.Instance.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			PhaseManagerComponent.Instance.StartPhase1 ();
			GameTimerView.Instance.ToggleTimer (false);
			stoptimer = false;
			CancelInvoke ("StartTimer");
		}
	}

}
