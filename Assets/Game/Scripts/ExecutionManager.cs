using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Manages the executions of player actions */
public class ExecutionManager : SingletonMonoBehaviour<ExecutionManager>
{

	private PlayerAttack playerAttack;
	// Use this for initialization

	BattleController battleController;

	void Start ()
	{
		battleController = FindObjectOfType<BattleController> ();
	}

	public void ExecutePlayerAttack ()
	{
		playerAttack = new PlayerAttack ();
		battleController.SetExecution (playerAttack);
	}
}
