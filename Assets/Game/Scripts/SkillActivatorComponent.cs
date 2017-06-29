using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillActivatorComponent : EnglishRoyaleElement
{

	public void ActivateSkill (ParamNames paramName)
	{
		switch (paramName) {
		case ParamNames.AirRender:
			SetSkill (delegate() {
				if (app.model.battleModel.gpEarned != 0) {
					app.model.battleModel.playerDamage += 2 * app.model.battleModel.gpEarned;
				} else {
					app.model.battleModel.playerDamage += 2;
				}
			});
			SetAnimation ("skill1");
			break;
		case ParamNames.Rejuvination:
			SetSkill (delegate() {
				if (app.model.battleModel.gpEarned != 0) {
					app.controller.battleController.playerHP += 2 * app.model.battleModel.gpEarned;
				} else {
					app.model.battleModel.playerDamage += 2;
				}
			}, delegate() {
				if (app.model.battleModel.gpEarned != 0) {
					app.controller.battleController.enemyHP += 2 * app.model.battleModel.gpEarned;
				} else {
					app.model.battleModel.playerDamage += 2;
				}
			});
			SetAnimation ("skill2");
			break;
		case ParamNames.Sunder:
			SetSkill (delegate() {
				app.controller.battleController.playerHP += 10;
				app.model.battleModel.playerDamage += 15;
			}, delegate() {
				app.controller.battleController.enemyHP += 10;
			});
			SetAnimation ("skill3");
			break;

		}
	}


	private void SetAnimation (string animationName)
	{
		if (app.model.battleModel.attackerBool.Equals (app.model.battleModel.isHost)) {
			app.controller.characterAnimationController.SetTriggerAnim (true, animationName);
		} else {
			app.controller.characterAnimationController.SetTriggerAnim (false, animationName);
		}
	
	}

	private void SetSkill (Action playerAction = null, Action enemyAction = null)
	{
		if (app.model.battleModel.attackerBool.Equals (app.model.battleModel.isHost)) {
			playerAction ();
		} else {
			enemyAction ();
		}
	}


}
