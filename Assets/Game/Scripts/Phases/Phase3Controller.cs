using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase3Controller : MonoBehaviour
{
	BattleController battleController;

	void Start(){
		battleController = FindObjectOfType<BattleController> ();
	}

	public void StartPhase3 ()
	{
		
		Attack ();
	}

	private void Attack ()
	{
		battleController.SendAttackToDatabase ();

	}
		
}
