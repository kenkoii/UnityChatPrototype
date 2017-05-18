using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : IPlayerAction {

	public void Execute (GameObject go, int attackValue){
		go.GetComponent<BattleManager> ().enemyLife -= attackValue;
		
	}
}
