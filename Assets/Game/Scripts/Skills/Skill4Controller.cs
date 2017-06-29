using System.Collections.Generic;
using System;
using UnityEngine;

public class Skill4Controller: EnglishRoyaleElement, ISkill
{
	private int skillCost = 3;
	private string skillName = "Enemy HP - 9";
	private string skillDescription = "Deals a straight blow to opponent's guts!";
	private Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();

	/// <summary>
	/// activate skill
	/// </summary>
	/// <param name="entity">Entity.</param>
	public void Activate ()
	{
		param [ParamNames.BicPunch.ToString ()] = 0;

		app.controller.battleController.playerGP -= skillCost;

		app.controller.tweenController.TweenPlayerGPSlider (app.controller.battleController.playerGP, 1, true);

		if (param != null) {
			app.component.firebaseDatabaseComponent.SetParam(app.model.battleModel.isHost, app.component.rpcWrapperComponent.DicToJsonStr (param));
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
