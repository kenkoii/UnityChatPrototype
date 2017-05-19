﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System;

public class RPCWrapper: SingletonMonoBehaviour<RPCWrapper>
{
	//test attack
	public void SumbitSignal(){
		Dictionary<string, System.Object> submit = new Dictionary<string, System.Object>();
		submit["damage"] = 10;

		RPCWrap ((int)StatusType.attack, submit);
	}

	public void RPCWrap (StatusType type, Dictionary<string, System.Object> param)
	{
		switch (type) {
		case StatusType.attack:
			FirebaseDatabaseFacade.Instance.AttackPlayer (GameManager.Instance.userName, type, DicToJsonStr (param));
			break;
		}
	}


	private string DicToJsonStr (Dictionary<string, System.Object> param)
	{
		string jsonStr = MiniJSON.Json.Serialize (param);
		return jsonStr;
	}
}

public enum StatusType
{
	attack
}