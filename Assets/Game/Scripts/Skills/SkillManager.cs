using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SingletonMonoBehaviour<SkillManager>
{

	private Skill1 skill1;
	BattleController battleController;
	// Use this for initialization

	void Start ()
	{
		battleController = FindObjectOfType<BattleController> ();
	}

	public void ActivateSkill1 ()
	{
		skill1 = new Skill1 ();
		battleController.SetSkill (skill1);
	}
}
