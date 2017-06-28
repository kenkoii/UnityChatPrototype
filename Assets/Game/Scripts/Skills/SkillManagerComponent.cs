using System;
using UnityEngine;

public class SkillManagerComponent : EnglishRoyaleElement
{

	private ISkill skill1;
	private ISkill skill2;
	private ISkill skill3;

	//test only... call before battle in future where player can select which skill
	void Start(){
		SetSkill1 (app.controller.skill1Controller);
		SetSkill2 (app.controller.skill2Controller);
		SetSkill3 (app.controller.skill3Controller);
	}

	/// <summary>
	/// Activates the skills.
	/// </summary>
	public void ActivateSkill1 ()
	{
		app.controller.battleController.SetSkill (skill1);
	}

	public void ActivateSkill2 ()
	{
		app.controller.battleController.SetSkill (skill2);
	}

	public void ActivateSkill3 ()
	{
		app.controller.battleController.SetSkill (skill3);
	}


	/// <summary>
	/// Set the skill to placeholder
	/// </summary>
	/// <param name="skill1">Skill1.</param>
	public void SetSkill1 (ISkill skill1)
	{
		this.skill1 = skill1;
		this.skill1.SetSkill (delegate(string skillName, string skillDescription , int gpCost) {
			app.model.battleModel.Skill1GPCost = gpCost;
			app.model.battleModel.Skill1Name = skillName;
			app.model.battleModel.skill2Description = skillDescription;

			app.controller.battleController.skill1Name.text = skillName;
			app.controller.battleController.skill1GpCost.text = "" +gpCost;
		});
	}

	public void SetSkill2 (ISkill skill2)
	{
		this.skill2 = skill2;
		this.skill2.SetSkill (delegate(string skillName, string skillDescription, int gpCost) {
			app.model.battleModel.Skill2GPCost = gpCost;
			app.model.battleModel.Skill2Name = skillName;
			app.model.battleModel.skill2Description = skillDescription;

			app.controller.battleController.skill2Name.text = skillName;
			app.controller.battleController.skill2GpCost.text = "" +gpCost;

		});
	}

	public void SetSkill3 (ISkill skill3)
	{
		this.skill3 = skill3;
		this.skill3.SetSkill (delegate(string skillName, string skillDescription, int gpCost) {
			app.model.battleModel.Skill3GPCost = gpCost;
			app.model.battleModel.Skill3Name = skillName;
			app.model.battleModel.skill2Description = skillDescription;

			app.controller.battleController.skill3Name.text = skillName;
			app.controller.battleController.skill3GpCost.text = "" +gpCost;

		});
	}

}
