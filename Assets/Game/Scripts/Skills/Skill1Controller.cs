using System.Collections.Generic;
using System;
using UnityEngine;

public class Skill1Controller: EnglishRoyaleElement, ISkill
{
	private int skillCost = 4;
	private string skillName = "Air Render ";
	private string skillDescription = "Skill Description: Releases a striking wind to the enemy. Answering more correct answers will multiply wind damage";

	/// <summary>
	/// activate skill
	/// </summary>
	/// <param name="entity">Entity.</param>
	public void Activate ()
	{
		
		if (app.model.battleModel.gpEarned != 0) {
			app.model.battleModel.playerDamage += 2 * app.model.battleModel.gpEarned;
		} else {
			app.model.battleModel.playerDamage += 2;
		}

		app.controller.battleController.playerGP -= skillCost;

		if (app.model.battleModel.modePrototype != ModeEnum.Mode2) {
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
