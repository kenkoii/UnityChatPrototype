using System.Collections.Generic;
using System;
using UnityEngine;

public class Skill3Controller: EnglishRoyaleElement, ISkill
{
	private int skillCost = 4;
	private string skillName = "Rejuvination";
	private string skillDescription = "Regenerates HP which is highly affected by number of correct answers";


	/// <summary>
	/// activate skill
	/// </summary>
	/// <param name="entity">Entity.</param>
	public void Activate (GameObject entity)
	{
		
		if (app.model.battleModel.gpEarned != 0) {
			app.controller.battleController.playerHP += 2 * app.model.battleModel.gpEarned;
		} else {
			app.controller.battleController.playerHP += 2;
		}

		entity.GetComponent<BattleController> ().playerGP -= skillCost;

		if (app.model.battleModel.modePrototype != ModeEnum.Mode2) {
			app.component.rpcWrapperComponent.RPCWrapSkill ();
		} 
	}

	/// <summary>
	/// set the name and cost of skill in placeholder
	/// </summary>
	/// <param name="skillParam">Skill parameter.</param>
	public void SetSkill(Action<string, int> skillParam){
		skillParam (skillName, skillCost);
	}


}
