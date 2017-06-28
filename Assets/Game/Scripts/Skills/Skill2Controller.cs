using System.Collections.Generic;
using System;
using UnityEngine;

public class Skill2Controller: EnglishRoyaleElement, ISkill
{
	private int skillCost = 1;
	private string skillName = "Sunder";
	private string skillDescription = "Deals a considerable amount of damage while absorbing life points at the same time.";
	public Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();

	/// <summary>
	/// activate skill
	/// </summary>
	/// <param name="entity">Entity.</param>
	public void Activate ()
	{
		param [ParamNames.SkillDamage.ToString ()] = 15;
		param [ParamNames.SkillHeal.ToString ()] = 10;
			
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
