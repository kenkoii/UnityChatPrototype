using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2: ISkill
{
	private int skillCost = 3;

	public void Activate (GameObject entity)
	{

		if (MyGlobalVariables.Instance.gpEarned != 0) {
			MyGlobalVariables.Instance.playerDamage += 2 * MyGlobalVariables.Instance.gpEarned;
		}

		PlayerAnimationController.Instance.Skill1Animate ();
		entity.GetComponent<BattleController> ().playerGP -= skillCost;

		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.SkillDamage.ToString ()] = 10;

		RPCWrapper.Instance.RPCWrapSkill ();
	}


}
