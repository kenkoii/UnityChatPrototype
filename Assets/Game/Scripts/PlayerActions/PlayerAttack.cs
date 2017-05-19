using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : IPlayerAction
{

	public void Execute (GameObject go)
	{
		if (GameManager.Instance.attackerParam [ParamNames.Damage.ToString()] != null) {
			int damage = int.Parse(GameManager.Instance.attackerParam [ParamNames.Damage.ToString()].ToString());

			if (GameManager.Instance.attackerName.Equals (GameManager.Instance.userName)) {
				if (GameManager.Instance.isPlayerVisitor) {
					go.GetComponent<BattleController> ().homeLife -= damage;
				} else {
					go.GetComponent<BattleController> ().visitorLife -= damage;
				}
			} else {
				if (GameManager.Instance.isPlayerVisitor) {
					go.GetComponent<BattleController> ().visitorLife -= damage;
				} else {
					go.GetComponent<BattleController> ().homeLife -= damage;
				}
			}
		}
		
	}
}
