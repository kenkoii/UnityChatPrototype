using UnityEngine;
using UnityEngine.UI;

public class PhaseAttackController : EnglishRoyaleElement
{
	
	private bool stoptimer = false;
	private int timeLeft;

	void OnEnable ()
	{
		app.controller.answerController.ResetAnswer ();
		Debug.Log ("Starting attack phase");
		app.view.gameTimerView.ToggleTimer (false);
		stoptimer = true;
		timeLeft = 20;
		InvokeRepeating ("StartTimer", 0, 1);
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
