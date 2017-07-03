using System.Collections.Generic;

public class SkillDAO{

	public bool userHome;
	public ParamNames skillName;
	public int skillGpCost;
	public string skillDescription;
	public string skillParam;

	public SkillDAO(){
	}

	public SkillDAO(ParamNames skillName, int skillGpCost, string skillDescription, string skillParam){
		this.skillName = skillName;
		this.skillGpCost = skillGpCost;
		this.skillDescription = skillDescription;
		this.skillParam = skillParam;
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result ["userHome"] = GameData.Instance.isHost;
		result ["SkillName"] = skillName;
		result ["SkillGPCost"] = skillGpCost;
		result ["SkillDescription"] = skillDescription;
		result ["SkillParam"] = skillParam;

		return result;
	}

}
