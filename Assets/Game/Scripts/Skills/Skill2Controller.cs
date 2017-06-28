using System.Collections.Generic;
using System;
using UnityEngine;

public class Skill2Controller: EnglishRoyaleElement, ISkill
{
	private int skillCost = 7;
	private string skillName = "Sunder";
	private string skillDescription = "Deals a considerable amount of damage while absorbing life points at the same time.";


	/// <summary>
	/// activate skill
	/// </summary>
	/// <param name="entity">Entity.</param>
	public void Activate ()
	{
		Debug.Log ("activate skill");

		app.model.battleModel.playerDamage += 15;
		app.controller.battleController.playerHP += 10;
			
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
