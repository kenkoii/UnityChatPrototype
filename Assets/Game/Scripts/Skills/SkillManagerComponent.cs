using System;
using UnityEngine;
using System.Collections.Generic;

public class SkillManagerComponent : SingletonMonoBehaviour<SkillManagerComponent>
{

	private SkillDAO[] skill;
	private List<SkillDAO> skillList = new List<SkillDAO> ();

	//TESTING ONLY
	void Start(){
		Dictionary<string, System.Object> param1 = new Dictionary<string, System.Object> ();
		param1 [ParamNames.Damage.ToString ()] = 10;
		skillList.Add (new SkillDAO(ParamNames.BicPunch,3,"Deals a straight blow to opponent's guts!",JsonConverter.DicToJsonStr (param1)));

		Dictionary<string, System.Object> param2 = new Dictionary<string, System.Object> ();
		param2 [ParamNames.Damage.ToString ()] = 15;
		param2 [ParamNames.Recover.ToString ()] = 10;
		skillList.Add (new SkillDAO(ParamNames.Sunder,9,"Deals a considerable amount of damage while absorbing life points at the same time.",JsonConverter.DicToJsonStr (param2)));

		Dictionary<string, System.Object> param3 = new Dictionary<string, System.Object> ();
		param3 [ParamNames.Recover.ToString ()] = 10;
		skillList.Add (new SkillDAO(ParamNames.Rejuvination,4,"Regenerates HP which is highly affected by number of correct answers",JsonConverter.DicToJsonStr (param3)));


		SetSkill (0,skillList[0]);
		SetSkill (1,skillList[1]);
		SetSkill (2,skillList[2]);
	}

	/// <summary>
	/// Activates the skills.
	/// </summary>
	public void ActivateSkill (int skillNumber)
	{
		Activate(skill[skillNumber + 1]);
	}

	private void Activate(SkillDAO skill){
		BattleController.Instance.SetSkill (skill);
	
	}

	/// <summary>
	/// Set the skill to placeholder
	/// </summary>
	/// <param name="skill1">Skill1.</param>

	private void SetSkill(int skillIndex,SkillDAO skill){
		this.skill[skillIndex] = skill;
		BattleController.Instance.SetSkillUI (skillIndex+1, skill.skillName, skill.skillGpCost);
	}

	public SkillDAO GetSkill(int skillNumber){
		return skill[skillNumber + 1];
	}


}
