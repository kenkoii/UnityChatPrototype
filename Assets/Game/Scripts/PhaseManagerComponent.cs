using UnityEngine;

public class PhaseManagerComponent : SingletonMonoBehaviour<PhaseManagerComponent>
{
	public void StartPhase1 ()
	{
		
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (false, true, false);
		} else {

			PhaseActivate (true, false, false);
		}
	}

	public void StartPhase2 ()
	{
		
		if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
			PhaseActivate (true, false, false);
		} else {
			PhaseActivate (false, true, false);
		}
	}

	public void StartPhase3 ()
	{
		
		PhaseActivate (false, false, true);
	}

	public void StopAll ()
	{
		Debug.Log ("Stopped phases");
		PhaseActivate (false, false, false);
	}

	private void PhaseActivate (bool answer, bool skill, bool attack)
	{
		PhaseAnswerController.Instance.gameObject.SetActive (answer);
		PhaseSkillController.Instance.gameObject.SetActive (skill);
		PhaseAttackController.Instance.gameObject.SetActive (attack);
	}

}
