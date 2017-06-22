using UnityEngine;

public class PhaseManagerComponent : EnglishRoyaleElement
{
	public void StartPhase1 ()
	{
		Debug.Log ("Starting phase 1");
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (false, true, false);
		} else {

			PhaseActivate (true, false, false);
		}
	}

	public void StartPhase2 ()
	{
		Debug.Log ("Starting phase 2");
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (true, false, false);
		} else {
			PhaseActivate (false, true, false);
		}
	}

	public void StartPhase3 ()
	{
		Debug.Log ("Starting phase 3");
		PhaseActivate (false, false, true);
	}

	public void StopAll ()
	{
		Debug.Log ("Stopped phases");
		PhaseActivate (false, false, false);
	}

	private void PhaseActivate (bool answer, bool skill, bool attack)
	{
		app.controller.phaseAnswerController.gameObject.SetActive (answer);
		app.controller.phaseSkillController.gameObject.SetActive (skill);
		app.controller.phaseAttackController.gameObject.SetActive (attack);
	}

}
