using System;
using UnityEngine;

public class SkillManagerComponent : EnglishRoyaleElement
{

	private ISkill skill1;
	private ISkill skill2;
	private ISkill skill3;

	/// <summary>
	/// Activates the skill1.
	/// </summary>
	public void ActivateSkill1 ()
	{
		//test skill only remove action later
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			SetSkill1 (app.controller.skill2Controller, delegate() {
				app.controller.battleController.SetSkill (skill1);
			});
		} else {
			SetSkill1 (app.controller.skill1Controller, delegate() {
				app.controller.battleController.SetSkill (skill1);
			});
		}
	}

	public void ActivateSkill2 ()
	{
		//test skill only remove action later
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			SetSkill2 (app.controller.skill2Controller, delegate() {
				app.controller.battleController.SetSkill (skill2);
			});
		} else {
			SetSkill2 (app.controller.skill1Controller, delegate() {
				app.controller.battleController.SetSkill (skill2);
			});
		}
	}

	public void ActivateSkill3 ()
	{
		//test skill only remove action later
		if (app.model.battleModel.modePrototype == ModeEnum.Mode2) {
			SetSkill3 (app.controller.skill2Controller, delegate() {
				app.controller.battleController.SetSkill (skill3);
			});
		} else {
			SetSkill3 (app.controller.skill1Controller, delegate() {
				app.controller.battleController.SetSkill (skill3);
			});
		}
	}


	//test only remove action later
	public void SetSkill1 (ISkill skill1, Action action)
	{
		this.skill1 = skill1;
		action ();
	}

	public void SetSkill2 (ISkill skill2, Action action)
	{
		this.skill2 = skill2;
		action ();
	}

	public void SetSkill3 (ISkill skill3, Action action)
	{
		this.skill3 = skill3;
		action ();
	}

}
