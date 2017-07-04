using System;
using UnityEngine;
using System.Collections.Generic;

public class SkillManagerComponent : SingletonMonoBehaviour<SkillManagerComponent>
{
	
	private SkillModel[] skill = new SkillModel[3];

	private List<SkillModel> skillList = new List<SkillModel> ();

	//TESTING ONLY!!!!
	void Start ()
	{
		List<SkillParameter> skillData1 = new List<SkillParameter> () {
			new SkillParameter (ParamNames.Damage.ToString (),10)

		};

		string skillParam1 = MiniJSON.Json.Serialize (new
			{
				operations = skillData1
			});
		
		skillList.Add (new SkillModel (ParamNames.BicPunch, 3, "Deals a straight blow to opponent's guts!", skillParam1));
	
		List<SkillParameter> skillData2 = new List<SkillParameter> () {
			new SkillParameter (ParamNames.Damage.ToString (), 15) , new SkillParameter (ParamNames.Recover.ToString (), 10) 
		};

		string skillParam2 = MiniJSON.Json.Serialize (new
			{
				operations = skillData2
			});
					
		skillList.Add (new SkillModel (ParamNames.Sunder, 9, "Deals a considerable amount of damage while absorbing life points at the same time.", skillParam2));

		List<SkillParameter> skillData3 = new List<SkillParameter> () {
			new SkillParameter (ParamNames.Recover.ToString (), 10) 
		};

		string skillParam3 = MiniJSON.Json.Serialize (new
			{
				operations = skillData3
			});
		
		skillList.Add (new SkillModel (ParamNames.Rejuvination, 4, "Regenerates HP which is highly affected by number of correct answers", skillParam3));

	
		

		SetSkill (0, skillList [0]);
		SetSkill (1, skillList [1]);
		SetSkill (2, skillList [2]);
	}

	/// <summary>
	/// Activates the skills.
	/// </summary>
	public void ActivateSkill (int skillNumber)
	{
		Activate (skill [skillNumber + 1]);
	}

	private void Activate (SkillModel skill)
	{
		BattleController.Instance.SetSkill (skill);
	
	}

	/// <summary>
	/// Set the skill to placeholder
	/// </summary>
	/// <param name="skill1">Skill1.</param>

	private void SetSkill (int skillIndex, SkillModel skill)
	{

		this.skill [skillIndex] = skill;
		BattleController.Instance.SetSkillUI (skillIndex + 1, skill.skillName, skill.skillGpCost);
	}

	public SkillModel GetSkill (int skillNumber)
	{
		return skill [skillNumber + 1];
	}


}
