using System.Collections.Generic;
using UnityEngine;

public class Skill2Controller: EnglishRoyaleElement, ISkill
{
	private int skillCost = 3;


	public void Activate (GameObject entity)
	{
		Debug.Log("activate skill");
		if (app.model.battleModel.gpEarned != 0) {
			app.model.battleModel.playerDamage += 2 * app.model.battleModel.gpEarned;
		}

		entity.GetComponent<BattleController> ().playerGP -= skillCost;

		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.SkillDamage.ToString ()] = 10;

		if (app.model.battleModel.modePrototype != ModeEnum.Mode4) {
			app.component.rpcWrapperComponent.RPCWrapSkill ();
		} 
	}


}
