using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController : SingletonMonoBehaviour<ScreenController>
{
	public GameObject loadingScreen;
	public GameObject matchingScreen;
	public Transform waitOpponentGroup;
	public RectTransform loadingIndicator;

	public void StartLoadingScreen (Action action)
	{
		loadingScreen.SetActive (true);
		action ();
	}

	public void StopLoadingScreen ()
	{
		loadingScreen.SetActive (false);
	}


	public void StartMatchingScreen ()
	{
		matchingScreen.SetActive (true);
	}

	public void StopMatchingScreen ()
	{
		matchingScreen.SetActive (false);
	}

	public void StartWaitOpponentScreen(){
		TweenController.TweenStartWaitOpponent (0.2f, waitOpponentGroup,loadingIndicator);

	}

	public void StopWaitOpponentScreen(){
		TweenController.TweenStopWaitOpponent (0.2f, waitOpponentGroup);
	}


}
