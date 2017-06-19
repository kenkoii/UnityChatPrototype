using UnityEngine;

public class PhaseManagerComponent : EnglishRoyaleElement
{

	public void StartPhase1(){
		Debug.Log ("Starting phase 1");
		app.controller.phase1Controller.gameObject.SetActive (true);
		app.controller.phase2Controller.gameObject.SetActive (false);
		app.controller.phase3Controller.gameObject.SetActive (false);
	}

	public void StartPhase2(){
		Debug.Log ("Starting phase 2");
		app.controller.phase1Controller.gameObject.SetActive(false);
		app.controller.phase2Controller.gameObject.SetActive(true);
		app.controller.phase3Controller.gameObject.SetActive(false);
	}

	public void StartPhase3(){
		Debug.Log ("Starting phase 3");
		app.controller.phase1Controller.gameObject.SetActive (false);
		app.controller.phase2Controller.gameObject.SetActive (false);
		app.controller.phase3Controller.gameObject.SetActive (true);

	}

	public void StopAll(){
		Debug.Log ("Stopped phases");

		app.controller.phase1Controller.gameObject.SetActive (false);
		app.controller.phase2Controller.gameObject.SetActive (false);
		app.controller.phase3Controller.gameObject.SetActive (false);
	}






		

}
