using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System;

/* Handles wrapping and sending of RPC status */
public class RPCWrapperComponent: SingletonMonoBehaviour<RPCWrapperComponent>
{
	/// <summary>
	/// Wraps RPC and send to firebase
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="param">Parameter.</param>
	public void RPCWrapAttack (Dictionary<string, System.Object> param)
	{
		ScreenController.Instance.StartWaitOpponentScreen ();
		FirebaseDatabaseComponent.Instance.AttackPhase (new AttackModel(JsonConverter.DicToJsonStr (param).ToString()));
	}

	public void RPCWrapSkill ()
	{
		ScreenController.Instance.StartWaitOpponentScreen ();
		FirebaseDatabaseComponent.Instance.SkillPhase ();
	
	}
		
	public void RPCWrapAnswer (int receiveTime, int receiveAnswer)
	{
		ScreenController.Instance.StartWaitOpponentScreen ();
		FirebaseDatabaseComponent.Instance.AnswerPhase (receiveTime, receiveAnswer);

		//show skill ui after answer only in mode 1
		if (GameData.Instance.modePrototype == ModeEnum.Mode1) {
			PhaseSkillController.Instance.ShowSkillUI (true,false);
			PhaseSkillController.Instance.ButtonEnable (false);
		}

		/*
		if (GameData.Instance.modePrototype == ModeEnum.Mode1) {
			PhaseSkillController.Instance.ShowSkillUI (true);
			PhaseSkillController.Instance.ButtonEnable (false);
		}*/


	}
		

}

