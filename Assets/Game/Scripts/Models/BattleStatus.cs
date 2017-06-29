using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Battle Status Model */
public class BattleStatus
{
	public bool userHome;
	public string param;

	public BattleStatus ()
	{
	}

	public BattleStatus (bool userHome, string param)
	{
		this.userHome = userHome;
		this.param = param;
	}

	public Dictionary<string, System.Object> ToDictionary ()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["userHome"] = userHome;
		result ["param"] = param;

		return result;
	}

}
