using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : ISkill {
	private int skillCost = 1;

	public void Activate(GameObject entity){
		PlayerAnimationController.Instance.Skill1Animate ();
		entity.GetComponent<BattleController> ().playerGP -= skillCost;

		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.SkillDamage.ToString ()] = 10;

		RPCWrapper.Instance.RPCWrapSkill (param);
	}
}
