using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Battle Status Model */
public class BattleStatus
{
	public string username;
	public int statusType;
	public string param;

	public BattleStatus ()
	{
	}

	public BattleStatus (string username, int statusType, string param)
	{
		this.username = username;
		this.statusType = statusType;
		this.param = param;
	}

	public Dictionary<string, System.Object> ToDictionary ()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["username"] = username;
		result ["statusType"] = statusType;
		result ["param"] = param;

		return result;
	}

}
