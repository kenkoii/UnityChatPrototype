using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionManager : SingletonMonoBehaviour<ExecutionManager> {

	private PlayerAttack playerAttack;
	// Use this for initialization

	BattleManager battleManager;

	void Start ()
	{
		battleManager = FindObjectOfType<BattleManager> ();
	}

	public void ExecutePlayerAttack(){
		playerAttack = new PlayerAttack ();
		battleManager.SetExecution (playerAttack);
	}
}
