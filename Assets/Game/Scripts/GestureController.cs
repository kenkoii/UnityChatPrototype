using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureController : EnglishRoyaleElement
{

	private Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();

	public void ShowGestureButtons ()
	{
		app.model.gestureModel.gestureButtonContainer.SetActive (true);
	}

	public void HideGestureButton ()
	{
		app.model.gestureModel.gestureButtonContainer.SetActive (false);
	}


	public void HidePlayerGestureContainer ()
	{
		app.model.gestureModel.playerGestureContainer.SetActive (false);
	}

	public void HideEnemyGestureContainer ()
	{
		app.model.gestureModel.enemyGestureContainer.SetActive (false);
	}

	public void ShowPlayerGesture1 ()
	{
		ShowGesture (true, "Gesture1");
		SendGesture (1);
	}

	public void ShowPlayerGesture2 ()
	{
		ShowGesture (true, "Gesture2");
		SendGesture (2);
	}

	public void ShowPlayerGesture3 ()
	{
		ShowGesture (true, "Gesture3");
		SendGesture (3);
	}

	public void ShowPlayerGesture4 ()
	{
		ShowGesture (true, "Gesture4");
		SendGesture (4);
	}


	public void SetEnemyGesture (int gestureNumber)
	{
		switch (gestureNumber) {
		case 1:
			ShowGesture (false, "Gesture1");
			break;
		case 2:
			ShowGesture (false, "Gesture2");
			break;
		case 3:
			ShowGesture (false, "Gesture3");
			break;
		case 4:
			ShowGesture (false, "Gesture4");
			break;
		}
	}

	IEnumerator StartTimer (bool isPLayer)
	{
		yield return new WaitForSeconds (1);
		if (isPLayer) {
			HidePlayerGestureContainer ();
		} else {
			HideEnemyGestureContainer ();
		}

	}

	private void SendGesture(int gestureNumber){
		param [ParamNames.Gesture.ToString ()] = gestureNumber;
		app.component.firebaseDatabaseComponent.SetParam(app.model.battleModel.isHost, app.component.rpcWrapperComponent.DicToJsonStr (param));
	}

	private void ShowGesture (bool isPlayer, string param)
	{
		StartCoroutine (StartTimer (isPlayer));
		if (isPlayer) {
			app.model.gestureModel.playerGestureContainer.SetActive (true);
		} else {
			app.model.gestureModel.enemyGestureContainer.SetActive (true);
		}
		app.controller.characterAnimationController.SetTriggerGestureAnim (isPlayer, param);

	}
}
