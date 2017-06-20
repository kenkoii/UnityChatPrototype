using UnityEngine;
using UnityEngine.UI;

public class PhaseAttackController : EnglishRoyaleElement
{
	public GameObject[] battleUI;
	private bool stoptimer = false;
	private int timeLeft;

	void OnEnable ()
	{
		app.view.gameTimerView.ToggleTimer (false);
		stoptimer = true;
		timeLeft = 20;
		InvokeRepeating ("StartTimer", 0, 1);

		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (false);
		}
		app.controller.battleController.SendAttackToDatabase ();
	}



	void OnDisable(){
		CancelInvoke ("StartTimer");

	}

	private void StartTimer ()
	{
		if (stoptimer) {
			if (timeLeft > 0) {
				timeLeft--;
				return;
			} 

//			app.component.firebaseDatabaseComponent.CheckAttackPhase();

			stoptimer = false;

		}
	}
		
}
