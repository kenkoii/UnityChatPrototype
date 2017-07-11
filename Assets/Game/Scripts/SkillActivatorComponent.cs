using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillActivatorComponent : SingletonMonoBehaviour<SkillActivatorComponent>, IRPCDicObserver
{
	/// <summary>
	/// Animates the skill.
	/// </summary>
	/// <param name="paramName">Parameter name.</param>
	/// <param name="gpEarned">Gp earned.</param>
	public void AnimateSkill (ParamNames paramName)
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

	public void OnNotify (Firebase.Database.DataSnapshot dataSnapShot)
	{
		Dictionary<string, System.Object> rpcReceive = (Dictionary<string, System.Object>)dataSnapShot.Value;
		if (rpcReceive.ContainsKey ("param")) {
			bool userHome = (bool)rpcReceive ["userHome"];
			GameData.Instance.attackerBool = userHome;

			Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcReceive ["param"];
			if (param.ContainsKey ("SkillParam")) {
				string stringParam = param ["SkillParam"].ToString ();
				if (GameData.Instance.attackerBool.Equals (GameData.Instance.isHost)) {
					SetPlayerSkillParameter (stringParam);
				} else {
					SetEnemySkillParameter (stringParam);
				}

			}
			if (param.ContainsKey ("SkillName")) {
				string stringParam = param ["SkillName"].ToString ();
				CheckSkillName (stringParam);
			}
		}
	}

	/// <summary>
	/// Sets the player skill parameter.
	/// </summary>
	/// <param name="skillParameter">Skill parameter.</param>
	public void SetPlayerSkillParameter (string skillParameter)
	{
		SkillParameterList skillResult = JsonUtility.FromJson<SkillParameterList> (skillParameter);

		foreach (SkillParameter skill in skillResult.skillList) {

			if (skill.skillKey == ParamNames.Damage.ToString ()) {
				GameData.Instance.player.playerDamage += skill.skillValue;
			}

			if (skill.skillKey == ParamNames.Recover.ToString ()) {
				BattleView.Instance.PlayerHP += skill.skillValue;
			}
		}
	}

	public void SetEnemySkillParameter (string skillParameter)
	{
		SkillParameterList skillResult = JsonUtility.FromJson<SkillParameterList> (skillParameter);

		foreach (SkillParameter skill in skillResult.skillList) {
			if (skill.skillKey == ParamNames.Recover.ToString ()) {
				BattleView.Instance.EnemyHP += skill.skillValue;
			}
		}
	}

	/// <summary>
	/// Sets the enemy skill parameter.
	/// </summary>
	/// <param name="skillParameter">Skill parameter.</param>


	/// <summary>
	/// Checks the name of the skill and set animation
	/// </summary>
	/// <param name="newParam">New parameter.</param>
	public void CheckSkillName (string skillName)
	{
		if (skillName == ParamNames.AirRender.ToString ()) {
			AnimateSkill (ParamNames.AirRender);
		} else if (skillName == ParamNames.Sunder.ToString ()) {
			AnimateSkill (ParamNames.Sunder);
		} else if (skillName == ParamNames.Rejuvination.ToString ()) {
			AnimateSkill (ParamNames.Rejuvination);
		} else if (skillName == ParamNames.BicPunch.ToString ()) {
			AnimateSkill (ParamNames.BicPunch);
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
