using System.Collections.Generic;
using UnityEngine;

public class BattleStatusManager : SingletonMonoBehaviour<BattleStatusManager>, IRPCDicObserver
{

	public void OnNotify (Dictionary<string, System.Object> rpcReceive)
	{
//		ReceiveBattleStatus (RPCReceiverComponent.Instance.GetBattleStatus ());

	}

	public void ReceiveBattleStatus (Dictionary<string, System.Object> battleStatusDetails)
	{
		string battleState = battleStatusDetails [MyConst.BATTLE_STATUS_STATE].ToString ();
		int battleCount = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_COUNT].ToString ());
		
		switch (battleState) {
		case MyConst.BATTLE_STATUS_ANSWER:
		
			GameData.Instance.hAnswer = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_HANSWER].ToString ());
			GameData.Instance.hTime = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_HTIME].ToString ());
			GameData.Instance.vAnswer = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_VANSWER].ToString ());
			GameData.Instance.vTime = int.Parse (battleStatusDetails [MyConst.BATTLE_STATUS_VTIME].ToString ());
		
					 
			if (battleCount > 1) {
				if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
					PhaseManagerComponent.Instance.StartPhase3 ();
				} else {
					PhaseManagerComponent.Instance.StartPhase2 ();
				}
				ScreenController.Instance.StopWaitOpponentScreen ();
					
			} 
		
			break;
		
		case MyConst.BATTLE_STATUS_SKILL:
			if (battleCount > 1) {
						
				if (GameData.Instance.modePrototype == ModeEnum.Mode2) {
					PhaseManagerComponent.Instance.StartPhase2 ();
				} else {
					PhaseManagerComponent.Instance.StartPhase3 ();
				}
				ScreenController.Instance.StopWaitOpponentScreen ();
			}
			break;
		
		case MyConst.BATTLE_STATUS_ATTACK:
			if (battleCount > 1) {
				ScreenController.Instance.StopWaitOpponentScreen ();
			} else {
				//hide skill ui 
				FindObjectOfType<PhaseSkillController> ().ShowSkillUI (false);
			}
				
			break;
		
		}
	}
}
