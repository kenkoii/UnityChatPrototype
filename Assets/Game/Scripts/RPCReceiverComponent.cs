using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Handles receiving of RPC status */
public class RPCReceiverComponent: SingletonMonoBehaviour<RPCReceiverComponent>
{
	private string rpcAttackParameter;
	private string rpcAnswerIndicatorParameter;
	private string rpcGestureParameter;
	private string rpcSkillNameParameter;
	private string rpcSkillParameter;

	private Dictionary<string, System.Object> rpcBattleStatusParameter;

	private Dictionary<string, System.Object> rpcInitialHomeStateParameter;
	private Dictionary<string, System.Object> rpcInitialVisitorStateParameter;

	/// <summary>
	/// Receives the RPC status.
	/// </summary>
	/// <param name="rpcDetails">Rpc details.</param>
	public void ReceiveRPC (Dictionary<string, System.Object> rpcDetails)
	{
		bool userHome = (bool)rpcDetails ["userHome"];
		Dictionary<string, System.Object> param = (Dictionary<string, System.Object>)rpcDetails ["param"];

		GameData.Instance.attackerBool = userHome;

		foreach (KeyValuePair<string, System.Object> newParam in param) {

			//NORMAL ATTACK
			if (newParam.Key == "Attack") {
				rpcAttackParameter = newParam.Value.ToString ();
			} 

			//ANSWER INDICATORS

			if (newParam.Key == "AnswerIndicator") {
				rpcAnswerIndicatorParameter = newParam.Value.ToString ();
			}
				

			// GESTURE

			if (newParam.Key == "Gesture") {
				rpcAnswerIndicatorParameter = newParam.Value.ToString ();
			}

			//SKILL PARAMETERS

			if (newParam.Key == "SkillName") {
				rpcAnswerIndicatorParameter = newParam.Value.ToString ();
			}

			if (newParam.Key == "SkillParam") {
				rpcAnswerIndicatorParameter = newParam.Value.ToString ();

			}

		}
			
	}

	/// <summary>
	/// Receives the battle status.
	/// </summary>
	/// <param name="battleStatusDetails">Battle status details.</param>
	public void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		rpcBattleStatusParameter = battleStatusDetails;
	}

	/// <summary>
	/// Receives the initial state of the home.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	public void ReceiveInitialHomeState (Dictionary<string, System.Object> initialState)
	{
		rpcInitialHomeStateParameter = initialState;
	}

	/// <summary>
	/// Receives the initial state of the visitor.
	/// </summary>
	/// <param name="ititialState">Ititial state.</param>
	public void ReceiveInitialVisitorState (Dictionary<string, System.Object> initialState)
	{
		rpcInitialVisitorStateParameter = initialState;
	}


	//Getters

	public string GetAttackParameter ()
	{
		return rpcAttackParameter;
	}

	public string GetAnswerIndicatorParameter ()
	{
		return rpcAnswerIndicatorParameter;
	}

	public string GetGestureParameter ()
	{
		return rpcGestureParameter;
	}

	public string GetSkillNameParameter ()
	{
		return rpcSkillNameParameter;
	}

	public string GetSkillParameter ()
	{
		return rpcSkillNameParameter;
	}

	public Dictionary<string, System.Object> GetBattleStatus ()
	{
		return rpcBattleStatusParameter;
	}

	public Dictionary<string, System.Object> GetHomeState ()
	{
		return rpcInitialHomeStateParameter;
	}

	public Dictionary<string, System.Object> GetVisitorState ()
	{
		return rpcInitialVisitorStateParameter;
	}

}
