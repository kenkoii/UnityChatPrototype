﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class SkillManagerComponent : SingletonMonoBehaviour<SkillManagerComponent>
{
	
	private SkillModel[] skill = new SkillModel[3];

	private List<SkillModel> skillList = new List<SkillModel> ();

	//TESTING ONLY!!!!
	void Start ()
	{
		//test skill 1
		List<SkillParameter> skillData1 = new List<SkillParameter> ();
		skillData1.Add (new SkillParameter (ParamNames.Damage.ToString(), 10));
		SkillParameterList splist1 = new SkillParameterList ();
		splist1.skillList = skillData1;
		string skillParam1 = JsonUtility.ToJson (splist1);
		skillList.Add (new SkillModel (ParamNames.BicPunch, 3, "Deals a straight blow to opponent's guts!", skillParam1));
	
		//test skill 2
		List<SkillParameter> skillData2 = new List<SkillParameter> ();
		skillData2.Add (new SkillParameter (ParamNames.Damage.ToString(), 15));
		skillData2.Add (new SkillParameter (ParamNames.Recover.ToString(), 10));
		SkillParameterList splist2 = new SkillParameterList ();
		splist2.skillList = skillData2;
		string skillParam2 = JsonUtility.ToJson (splist2);
		skillList.Add (new SkillModel (ParamNames.Sunder, 9, "Deals a considerable amount of damage while absorbing life points at the same time.", skillParam2));

		//test skill 3
		List<SkillParameter> skillData3 = new List<SkillParameter> ();
		skillData3.Add (new SkillParameter (ParamNames.Recover.ToString(), 10));
		SkillParameterList splist3 = new SkillParameterList ();
		splist3.skillList = skillData3;
		string skillParam3 = JsonUtility.ToJson (splist3);
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
		StartSkill (skill [skillNumber - 1]);
	}

	public void StartSkill (SkillModel skill)
	{
		StartCoroutine (StartSkillDeductDelay(skill));
		FDController.Instance.SetSkillParam (skill);
		if (GameData.Instance.modePrototype == ModeEnum.Mode1) {
			RPCWrapperComponent.Instance.RPCWrapSkill ();
		} 
	}

	IEnumerator StartSkillDeductDelay(SkillModel skill){
		yield return new WaitForSeconds (0.5f);
		BattleView.Instance.PlayerGP -= skill.skillGpCost;
	}

	/// <summary>
	/// Set the skill to placeholder UI
	/// </summary>
	/// <param name="skill1">Skill1.</param>

	private void SetSkill (int skillIndex, SkillModel skill)
	{
		this.skill [skillIndex] = skill;
		BattleView.Instance.SetSkillUI (skillIndex + 1, skill.skillName, skill.skillGpCost);
	}

	public SkillModel GetSkill (int skillNumber)
	{
		return skill [skillNumber -1];
	}


}
