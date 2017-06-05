using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System;

/* Handles wrapping and sending of RPC status */
public class RPCWrapper: SingletonMonoBehaviour<RPCWrapper>
{
	/// <summary>
	/// Wraps RPC and send to firebase
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="param">Parameter.</param>
	public void RPCWrapAttack (Dictionary<string, System.Object> param)
	{
		FirebaseDatabaseFacade.Instance.AttackPhase (GameManager.Instance.playerName,DicToJsonStr (param));

	}

	public void RPCWrapSkill (Dictionary<string, System.Object> param)
	{
		FirebaseDatabaseFacade.Instance.SkillPhase (GameManager.Instance.playerName,DicToJsonStr (param));
		Debug.Log ("RPCWRAPSKILL");

	}

	public void RPCWrapAnswer (Dictionary<string, System.Object> param)
	{
		FirebaseDatabaseFacade.Instance.AnswerPhase (GameManager.Instance.playerName,DicToJsonStr (param));

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

