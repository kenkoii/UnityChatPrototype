using UnityEngine;
using UnityEngine.UI;

public class Phase2Controller : EnglishRoyaleElement
{
	public GameObject[] battleUI;
	public Text skillTimerText;
	public int skill1GPCost = 3;
	public int skill2GPCost = 3;
	public int skill3GPCost = 3;
	public Button skillButton1;
	public Button skillButton2;
	public Button skillButton3;
	private bool stoptimer = false;
	private int timeLeft;
	private BattleController battleController;
	public Button attackButton;


	public void OnEnable ()
	{
		
		battleController = FindObjectOfType<BattleController> ();

		if (skill1GPCost > battleController.playerGP) {
			skillButton1.interactable = false;
		} else {
			skillButton1.interactable = true;
		}

		if (skill2GPCost > battleController.playerGP) {
			skillButton2.interactable = false;
		} else {
			skillButton2.interactable = true;
		}

		if (skill3GPCost > battleController.playerGP) {
			skillButton3.interactable = false;
		} else {
			skillButton3.interactable = true;
		}

		if (app.model.battleModel.modePrototype == ModeEnum.Mode2 || app.model.battleModel.modePrototype == ModeEnum.Mode3) {
			attackButton.interactable = true;
			attackButton.gameObject.SetActive (true);
		}

		for (int i = 0; i < battleUI.Length; i++) {
			battleUI [i].SetActive (true);
		}


		timeLeft = 5;
		stoptimer = true;
		InvokeRepeating ("StartTimer", 0, 1);

	
	}
		
	void OnDisable ()
	{
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2 ||app.model.battleModel.modePrototype == ModeEnum.Mode3) {
			attackButton.gameObject.SetActive (false);
		}
		CancelInvoke ("StartTimer");
	}

	public void AttackButton(){
		ButtonEnable (false);
		app.view.gameTimerView.ToggleTimer (false);
		app.component.rpcWrapperComponent.RPCWrapSkill ();
		stoptimer = false;
	}

	private void ButtonEnable (bool buttonEnable)
	{
		skillButton1.interactable = buttonEnable;
		skillButton2.interactable = buttonEnable;
		skillButton3.interactable = buttonEnable;
		attackButton.interactable = buttonEnable;
	}

	public void SelectSkill1 ()
	{
		app.component.skillManagerComponent.ActivateSkill1 ();
		ButtonEnable (false);
		app.view.gameTimerView.ToggleTimer (false);
		stoptimer = false;

	}

	public void SelectSkill2 ()
	{
		app.component.skillManagerComponent.ActivateSkill1 ();
		ButtonEnable (false);
		app.view.gameTimerView.ToggleTimer (false);
		stoptimer = false;
	}

	public void SelectSkill3 ()
	{
		app.component.skillManagerComponent.ActivateSkill1 ();
		ButtonEnable (false);
		app.view.gameTimerView.ToggleTimer (false);
		stoptimer = false;
	}

	private void StartTimer ()
	{
		if (stoptimer) {
			app.view.gameTimerView.ToggleTimer (true);
			if (timeLeft > 0) {
				app.view.gameTimerView.gameTimerText.text = "" + timeLeft;
				timeLeft--;
				return;
			} 
			ButtonEnable (false);
			app.view.gameTimerView.ToggleTimer (false);
				
			app.component.rpcWrapperComponent.RPCWrapSkill ();
			Debug.Log ("stopped phase2 timer");
			stoptimer = false;

		}
	}
		

}
