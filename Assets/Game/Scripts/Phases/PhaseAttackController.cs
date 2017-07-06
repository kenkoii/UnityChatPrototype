using UnityEngine;
using UnityEngine.UI;

public class PhaseAttackController : SingletonMonoBehaviour<PhaseAttackController>, IPhase
{
	
	private bool stoptimer = false;
	private int timeLeft;

	public void OnStartPhase ()
	{
		AnswerController.Instance.ResetAnswer ();
		Debug.Log ("Starting attack phase");
		GameTimerView.Instance.ToggleTimer (false);
		stoptimer = true;
		timeLeft = 20;
		InvokeRepeating ("StartTimer", 0, 1);
		BattleController.Instance.SendAttackToDatabase ();
	}

	public void OnEndPhase(){
		CancelInvoke ("StartTimer");
		//reset effects done by skill
		GameController.Instance.ResetPlayerDamage ();
		Debug.Log ("player damage reset! now damage is: " + GameData.Instance.player.playerDamage);
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
