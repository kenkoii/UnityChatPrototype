using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager> {

	public string userName{ get; set;}
	public int life{ get; set;}
	public bool isPlayerVisitor{ get; set;}

	public string attackerName{ get; set; }
	public Dictionary<string, System.Object> attackerParam{ get; set; }
}
