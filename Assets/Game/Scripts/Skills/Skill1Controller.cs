using System.Collections.Generic;
using UnityEngine;

public class Skill1Controller : EnglishRoyaleElement, ISkill
{
	private int skillCost = 3;

	public void Activate (GameObject entity)
	{
		Debug.Log("activate skill");
		app.model.battleModel.playerDamage += 10;
		entity.GetComponent<BattleController> ().playerGP -= skillCost;

		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.SkillDamage.ToString ()] = 10;

		if (app.model.battleModel.modePrototype != ModeEnum.Mode2) {
			app.component.rpcWrapperComponent.RPCWrapSkill ();
		}
	}


}
