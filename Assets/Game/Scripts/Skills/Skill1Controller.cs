using System.Collections.Generic;
using System;
using UnityEngine;

public class Skill1Controller: EnglishRoyaleElement, ISkill
{
	private int skillCost = 1;
	private string skillName = "Air Render ";
	private string skillDescription = "Skill Description: Releases a striking wind to the enemy. Answering more correct answers will multiply wind damage";
	private Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();

	/// <summary>
	/// activate skill
	/// </summary>
	/// <param name="entity">Entity.</param>
	public void Activate ()
	{
		float damage = 0;
		if (app.model.battleModel.gpEarned != 0) {
			damage = 2 * app.model.battleModel.gpEarned;
		} else {
			damage += 2;
		}

		param [ParamNames.SkillDamage.ToString ()] = damage;

		app.controller.battleController.playerGP -= skillCost;

		app.controller.tweenController.TweenPlayerGPSlider (app.controller.battleController.playerGP, 1, true);

		if (param != null) {
			app.component.firebaseDatabaseComponent.SetParam(app.model.battleModel.playerName, app.component.rpcWrapperComponent.DicToJsonStr (param));
		}

		if (app.model.battleModel.modePrototype == ModeEnum.Mode1) {
			app.component.rpcWrapperComponent.RPCWrapSkill ();
		} 
	}

	/// <summary>
	/// set the name and cost of skill in placeholder
	/// </summary>
	/// <param name="skillParam">Skill parameter.</param>
	public void SetSkill(Action<string,string, int> skillParam){
		skillParam (skillName,skillDescription, skillCost);
	}


}
