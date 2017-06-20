using UnityEngine;

public class PhaseManagerComponent : EnglishRoyaleElement
{
	public void StartPhase1 ()
	{
		Debug.Log ("Starting phase 1");
		if (app.model.battleModel.modePrototype == ModeEnum.Mode4) {
			app.controller.phaseSkillController.gameObject.SetActive (true);
			app.controller.phaseAnswerController.gameObject.SetActive (false);
			app.controller.phaseAttackController.gameObject.SetActive (false);
		} else {

			app.controller.phaseAnswerController.gameObject.SetActive (true);
			app.controller.phaseSkillController.gameObject.SetActive (false);
			app.controller.phaseAttackController.gameObject.SetActive (false);
		}
	}

	public void StartPhase2 ()
	{
		Debug.Log ("Starting phase 2");
		if (app.model.battleModel.modePrototype == ModeEnum.Mode4) {
			app.controller.phaseSkillController.gameObject.SetActive (false);
			app.controller.phaseAnswerController.gameObject.SetActive (true);
			app.controller.phaseAttackController.gameObject.SetActive (false);
		} else {
			app.controller.phaseAnswerController.gameObject.SetActive (false);
			app.controller.phaseSkillController.gameObject.SetActive (true);
			app.controller.phaseAttackController.gameObject.SetActive (false);
		}
	}

	public void StartPhase3 ()
	{
		Debug.Log ("Starting phase 3");
			app.controller.phaseSkillController.gameObject.SetActive (false);
			app.controller.phaseAnswerController.gameObject.SetActive (false);
			app.controller.phaseAttackController.gameObject.SetActive (true);

	}

	public void StopAll ()
	{
		Debug.Log ("Stopped phases");
		app.controller.phaseAnswerController.gameObject.SetActive (false);
		app.controller.phaseSkillController.gameObject.SetActive (false);
		app.controller.phaseAttackController.gameObject.SetActive (false);
	}






		

}
