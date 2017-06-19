using System;
using UnityEngine;

public class ScreenController : EnglishRoyaleElement{

	public void StartLoadingScreen(Action action){
		app.view.screenView.loadingScreen.SetActive (true);
		action ();
	
	}

	public void StopLoadingScreen(){
		app.view.screenView.loadingScreen.SetActive (false);
	
	}


	public void StartMatchingScreen(){
		app.view.screenView.matchingScreen.SetActive (true);

	}

	public void StopMatchingScreen(){
		app.view.screenView.matchingScreen.SetActive (false);
	}


}
