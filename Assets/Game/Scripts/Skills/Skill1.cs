using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : ISkill
{
	private int skillCost = 3;

	public void Activate (GameObject entity)
	{

		MyGlobalVariables.Instance.playerDamage += 10;
		PlayerAnimationController.Instance.Skill1Animate ();
		entity.GetComponent<BattleController> ().playerGP -= skillCost;

		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.SkillDamage.ToString ()] = 10;

		RPCWrapper.Instance.RPCWrapSkill ();
	}


}
