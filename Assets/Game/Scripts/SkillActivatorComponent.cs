using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillActivatorComponent : SingletonMonoBehaviour<SkillActivatorComponent>
{
	/// <summary>
	/// Animates the skill.
	/// </summary>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="gpEarned">Gp earned.</param>
	public void AnimateSkill (ParamNames paramName, int gpEarned = 0)
	{
		switch (paramName) {
		case ParamNames.AirRender:
			SetAnimation ("skill1");
			break;
		case ParamNames.Rejuvination:
			SetAnimation ("skill2");
			break;
		case ParamNames.Sunder:
			SetAnimation ("skill3");
			break;

		case ParamNames.BicPunch:
			SetAnimation ("skill4");
			break;

		}
	
	}

	/// <summary>
	/// Sets the player skill parameter.
	/// </summary>
	/// <param name="skillParameter">Skill parameter.</param>
	public void SetPlayerSkillParameter (Dictionary<string,System.Object> skillParameter)
	{

		foreach (KeyValuePair<string, System.Object> newParam in skillParameter) {
			if (newParam.Key == ParamNames.Damage.ToString ()) {
				GameData.Instance.player.playerDamage += int.Parse (newParam.Value.ToString ());
			}

			if (newParam.Key == ParamNames.Recover.ToString ()) {
				BattleController.Instance.playerHP += int.Parse (newParam.Value.ToString ());
			}
			CheckSkillName (newParam);
		
		}
	}

	/// <summary>
	/// Sets the enemy skill parameter.
	/// </summary>
	/// <param name="skillParameter">Skill parameter.</param>
	public void SetEnemySkillParameter (Dictionary<string,System.Object> skillParameter)
	{
		foreach (KeyValuePair<string, System.Object> newParam in skillParameter) {
			if (newParam.Key == ParamNames.Recover.ToString ()) {
				BattleController.Instance.enemyHP += int.Parse (newParam.Value.ToString ());
			}

			CheckSkillName (newParam);
		}
	}

	/// <summary>
	/// Checks the name of the skill and set animation
	/// </summary>
	/// <param name="newParam">New parameter.</param>
	private void CheckSkillName (KeyValuePair<string, System.Object> newParam)
	{
		if(newParam.Key == ParamNames.AirRender.ToString()){
			AnimateSkill (ParamNames.AirRender, int.Parse (newParam.Value.ToString ()));
		}

		else if(newParam.Key == ParamNames.Sunder.ToString()){
			AnimateSkill (ParamNames.Sunder, int.Parse (newParam.Value.ToString ()));
		}

		else if(newParam.Key == ParamNames.Rejuvination.ToString()){
			AnimateSkill (ParamNames.Rejuvination, int.Parse (newParam.Value.ToString ()));
		}

		else if(newParam.Key == ParamNames.BicPunch.ToString()){
			AnimateSkill (ParamNames.BicPunch, int.Parse (newParam.Value.ToString ()));
		}
			
	}


	/// <summary>
	/// Sets the animation.
	/// </summary>
	/// <param name="animationName">Animation name.</param>
	private void SetAnimation (string animationName)
	{
		if (GameData.Instance.attackerBool.Equals (GameData.Instance.isHost)) {
			CharacterAnimationController.Instance.SetTriggerAnim (true, animationName);
		} else {
			CharacterAnimationController.Instance.SetTriggerAnim (false, animationName);
		}
	
	}

}
