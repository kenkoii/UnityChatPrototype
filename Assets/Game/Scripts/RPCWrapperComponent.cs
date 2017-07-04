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
//		app.controller.tweenController.TweenStartWaitOpponent (0.2f);
		FirebaseDatabaseComponent.Instance.AttackPhase (new AttackModel(JsonConverter.DicToJsonStr (param).ToString()));
	}

	public void RPCWrapSkill ()
	{
//		app.controller.tweenController.TweenStartWaitOpponent (0.2f);
		FirebaseDatabaseComponent.Instance.SkillPhase ();
	
	}
		
	public void RPCWrapAnswer (int receiveTime, int receiveAnswer)
	{
//		app.controller.tweenController.TweenStartWaitOpponent (0.2f);
		FirebaseDatabaseComponent.Instance.AnswerPhase (receiveTime, receiveAnswer);

	}
		

}

