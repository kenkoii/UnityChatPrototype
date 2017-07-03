using System;
using UnityEngine;

public class ScreenController : SingletonMonoBehaviour<ScreenController>
{
	public GameObject loadingScreen;
	public GameObject matchingScreen;

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


}
