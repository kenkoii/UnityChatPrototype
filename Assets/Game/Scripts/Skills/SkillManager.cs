using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletonMonoBehaviour<SkillManager>
{

	private ISkill skill1;
	private ISkill skill2;
	private ISkill skill3;

	BattleController battleController;
	// Use this for initialization

	void Start ()
	{
		battleController = FindObjectOfType<BattleController> ();

		//test set skill
		SetSkill1(new Skill1());
	}

	/// <summary>
	/// Activates the skill1.
	/// </summary>
	public void ActivateSkill1 ()
	{
		battleController.SetSkill (skill1);
	}


	//to easily set skill in future
	public void SetSkill1(ISkill skill1){
		this.skill1 = skill1;
	}

	public void SetSkill2(ISkill skill2){
		this.skill2 = skill2;
	}

	public void SetSkill3(ISkill skill3){
		this.skill3 = skill3;
	}
}
