using UnityEngine;
using UnityEngine.UI;

public class PhaseAttackController : EnglishRoyaleElement
{
	private BattleController battleController;
	public GameObject[] battleUI;
	private bool stoptimer = false;
	private int timeLeft;

	void OnEnable ()
	{
		app.view.gameTimerView.ToggleTimer (false);
		battleController = FindObjectOfType<BattleController> ();
		stoptimer = true;
		timeLeft = 10;
		InvokeRepeating ("StartTimer", 0, 1);

		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (false);
		}
		battleController.SendAttackToDatabase ();
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

			app.component.firebaseDatabaseComponent.CheckAttackPhase();

			stoptimer = false;

		}
	}
		
}
