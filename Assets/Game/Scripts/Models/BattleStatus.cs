using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Battle Status Model */
public class BattleStatus
{
	public string param;

	public BattleStatus ()
	{
	}

	public BattleStatus (string param)
	{
		this.param = param;
	}

	public Dictionary<string, System.Object> ToDictionary ()
	{
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object> ();
		result ["AnswerIndicator"] = param;

		return result;
	}

}
