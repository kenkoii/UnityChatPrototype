using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Controls the battle */
public class BattleController : MonoBehaviour
{

	IPlayerAction playerAction;
	public int homeLife = 10;
	public int visitorLife = 10;
	public string homeName = "Anonymous1";
	public string visitorName = "Anonymous2";

	public Text homeNameText;
	public Text homeLifeText;
	public Text visitorNameText;
	public Text visitorLifeText;

	public Text battleResultText;


	void Update ()
	{
		homeNameText.text = "" + homeName;
		homeLifeText.text = "" + homeLife;
		visitorNameText.text = "" + visitorName;
		visitorLifeText.text = "" + visitorLife;
	}

	public void Execute ()
	{
		playerAction.Execute (this.gameObject);
	}

	public void InitialHomeState (int playerLife, string playerName)
	{
		this.homeLife = playerLife;
		this.homeName = playerName;
	}

	public void InitialVisitorState (int enemyLife, string enemyName)
	{
		this.visitorLife = enemyLife;
		this.visitorName = enemyName;
	}

	/// <summary>
	/// Sets the execution of playerAction and execute. 
	/// </summary>
	/// <param name="playerAction">Player action.</param>
	public void SetExecution (IPlayerAction playerAction)
	{
		if (visitorLife > 0 && homeLife > 0) {
			this.playerAction = playerAction;
			Execute ();
		} 
		if (visitorLife > 0 && homeLife <= 0) {
			if (GameManager.Instance.isPlayerVisitor) {
				battleResultText.text = "WIN";
			} else {
				battleResultText.text = "LOSE";
			}

		} 
		else if (homeLife > 0 && visitorLife <= 0) {
			if (GameManager.Instance.isPlayerVisitor) {
				battleResultText.text = "LOSE";
			} else {
				battleResultText.text = "WIN";
			}

		} 
		battleResultText.enabled = true;

	}

	//test attack
	public void SendAttack ()
	{
		Dictionary<string, System.Object> param = new Dictionary<string, System.Object> ();
		param [ParamNames.Damage.ToString ()] = 10;
		RPCWrapper.Instance.RPCWrap (StatusType.Attack, param);
	}
}
