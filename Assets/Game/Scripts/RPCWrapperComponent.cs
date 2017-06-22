using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System;

/* Handles wrapping and sending of RPC status */
public class RPCWrapperComponent: EnglishRoyaleElement
{
	/// <summary>
	/// Wraps RPC and send to firebase
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="param">Parameter.</param>
	public void RPCWrapAttack (Dictionary<string, System.Object> param)
	{
		app.component.firebaseDatabaseComponent.AttackPhase (app.model.battleModel.playerName,DicToJsonStr (param));

	}

	public void RPCWrapSkill ()
	{
		app.component.firebaseDatabaseComponent.SkillPhase ();
	
	}
		
	public void RPCWrapAnswer (int receiveTime, int receiveAnswer)
	{
		app.component.firebaseDatabaseComponent.AnswerPhase (receiveTime, receiveAnswer);

	}



	/// <summary>
	/// Converts Dictionary<string, System.Object> to string
	/// </summary>
	/// <returns>The to json string.</returns>
	/// <param name="param">Parameter.</param>
	private string DicToJsonStr (Dictionary<string, System.Object> param)
	{
		string jsonStr = MiniJSON.Json.Serialize (param);
		return jsonStr;
	}
}

