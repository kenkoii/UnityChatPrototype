using System.Collections.Generic;
using UnityEngine;

public class Skill1Controller : EnglishRoyaleElement, ISkill
{
	private int skillCost = 3;

	public void Activate (GameObject entity)
	{

		app.model.battleModel.playerDamage += 10;
		entity.GetComponent<BattleController> ().playerGP -= skillCost;

		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.SkillDamage.ToString ()] = 10;

		app.component.rpcWrapperComponent.RPCWrapSkill ();
	}


}
