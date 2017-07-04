using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureController : SingletonMonoBehaviour<GestureController>
{
	private bool hasAnswered = false;
	public GameObject gestureButtonContainer;

	private Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();

	public void ShowGestureButtons ()
	{
		gestureButtonContainer.SetActive (true);
	}

	public void HideGestureButton ()
	{
		gestureButtonContainer.SetActive (false);
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

	private void SendGesture (int gestureNumber)
	{
		param [ParamNames.Gesture.ToString ()] = gestureNumber;
		FirebaseDatabaseComponent.Instance.SetAnswerParam (new AnswerModel(JsonConverter.DicToJsonStr (param).ToString()));
	}

	//Hide gesture camera after displaying
	IEnumerator StartTimer (bool isPlayer)
	{
		yield return new WaitForSeconds (1.5f);
		if (!isPlayer) {
			CameraWorksController.Instance.HideGestureCamera ();
		}
	}

	private void ShowGesture (bool isPlayer, string param)
	{
		StartCoroutine (StartTimer (isPlayer));
		CharacterAnimationController.Instance.SetTriggerAnim (isPlayer, param);
		if (!isPlayer) {
			CameraWorksController.Instance.ShowGestureCamera ();
		}
	}
}
