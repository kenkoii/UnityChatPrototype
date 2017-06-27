using System.Collections.Generic;
using System;

public class BattleModel : EnglishRoyaleElement
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

	public ModeEnum modePrototype { get; set; }

	public Dictionary<string, System.Object> attackerParam{ get; set; }

	public Action playerSkillChosen{ get; set; }

	public int skillChosenCost{ get; set; }

	public int hAnswer{ get; set; }

	public int hTime{ get; set; }

	public int vAnswer{ get; set; }

	public int vTime{ get; set; }

	public int answerQuestionTime{ get; set; }

	public int gpEarned{ get; set; }

	public int skill1GPCost {
		get{ return skill1GPCost; } 
		set {
			skill1GPCost = value;
			app.controller.battleController.skill1GpCost.text = "" + skill1GPCost;
		}
	}

	public int skill2GPCost {
		get{ return skill2GPCost; } 
		set {
			skill2GPCost = value;
			app.controller.battleController.skill2GpCost.text = "" + skill2GPCost;
		}
	}

	public int skill3GPCost {
		get{ return skill3GPCost; }  
		set {
			skill3GPCost = value;
			app.controller.battleController.skill3GpCost.text = "" + skill3GPCost;
		}
	}

	public string skill1Name {
		get{ return skill1Name; } 
		set {
			skill1Name = value;
			app.controller.battleController.skill1Name.text = "" + skill1Name;
		}
	}

	public string skill2Name {
		get{ return skill2Name; }  
		set {
			skill2Name = value;
			app.controller.battleController.skill2Name.text = "" + skill2Name;
		}
	}

	public string skill3Name {
		get{ return skill3Name; } 
		set {
			skill3Name = value;
			app.controller.battleController.skill3Name.text = "" + skill3Name;
		}
	}



}
