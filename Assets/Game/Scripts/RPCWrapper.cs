using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System;

public class RPCWrapper: SingletonMonoBehaviour<RPCWrapper>
{
	/// <summary>
	/// Wraps RPC and send to firebase
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="param">Parameter.</param>
	public void RPCWrap (StatusType type, Dictionary<string, System.Object> param)
	{
		switch (type) {
		case StatusType.Attack:
			FirebaseDatabaseFacade.Instance.AttackPlayer (GameManager.Instance.userName, type, DicToJsonStr (param));
			break;
		}
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

