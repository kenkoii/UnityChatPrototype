using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Manages the player and enemy status */
public class MyGlobalVariables : SingletonMonoBehaviour<MyGlobalVariables>
{

	public string playerName{ get; set; }

	public int playerLife{ get; set; }

	public int playerGP{ get; set; }

	public int playerMaxGP{ get; set; }

	public float playerDamage{ get; set; }

	public bool isPlayerVisitor{ get; set; }

	public string attackerName{ get; set; }

	public string battleState{ get; set; }

	public int battleCount{ get; set; }

	public int modePrototype { get; set;}
	public Dictionary<string, System.Object> attackerParam{ get; set; }



	public void ResetPlayerStats(){
		playerDamage = 5;
	}
}
