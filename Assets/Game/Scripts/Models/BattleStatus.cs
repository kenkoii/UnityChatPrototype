using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Battle Status Model */
public class BattleStatus
{
	public string username;
	public string param;

	public BattleStatus ()
	{
	}

	public BattleStatus (string username, string param)
	{
		this.username = username;
		this.param = param;
	}

	public Dictionary<string, System.Object> ToDictionary ()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["username"] = username;
		result ["param"] = param;

		return result;
	}

}
