using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillActivatorComponent : SingletonMonoBehaviour<SkillActivatorComponent>
{

	public void ActivateSkill (ParamNames paramName, int gpEarned = 0)
	{
		switch (paramName) {
		case ParamNames.AirRender:
			SetAnimation ("skill1");
			break;
		case ParamNames.Rejuvination:
			SetAnimation ("skill2");
			break;
		case ParamNames.Sunder:
			SetAnimation ("skill3");
			break;

		case ParamNames.BicPunch:

			SetAnimation ("skill4");
			break;

		}
	
	}


	private void SetAnimation (string animationName)
	{
		if (GameData.Instance.attackerBool.Equals (GameData.Instance.isHost)) {
			CharacterAnimationController.Instance.SetTriggerAnim (true, animationName);
		} else {
			CharacterAnimationController.Instance.SetTriggerAnim (false, animationName);
		}
	
	}

}
