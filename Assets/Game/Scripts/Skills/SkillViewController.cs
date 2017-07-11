using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SkillViewController : MonoBehaviour {
	public GameObject skillView;
	public GameObject skillOverViewScreen;
	public GameObject skillOverViewDesc;
	public GameObject skillDamage;
	public GameObject skillHeal;
	public GameObject skillName;
	public GameObject[] skillArray = new GameObject[3];
	private SkillModel skill1;
	private SkillModel skill2;
	private SkillModel skill3;

	// Use this for initialization
	void Start () {
		skill1 = SkillManagerComponent.Instance.GetSkill (1);
		skill2 = SkillManagerComponent.Instance.GetSkill (2);
		skill3 = SkillManagerComponent.Instance.GetSkill (3);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClickSkillItem(){
		skillOverViewScreen.SetActive (true);
		TweenController.TweenScaleToLarge (skillOverViewScreen.transform,new Vector3(1,1,1),0.2f);
		skillOverViewScreen.transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Image> ().sprite;
		switch (EventSystem.current.currentSelectedGameObject.name) {
		case "BicPunch":
			skillOverViewDesc.GetComponentInChildren<Text> ().text = SkillManagerComponent.Instance.skillList[0].skillDescription;
			skillDamage.GetComponentInChildren<Text> ().text = "9 HP";
			skillHeal.GetComponentInChildren<Text> ().text = "0 HP";
			skillName.GetComponentInChildren<Text> ().text = SkillManagerComponent.Instance.skillList[0].skillName.ToString();
			break;
		case "Sunder":
			skillOverViewDesc.GetComponentInChildren<Text> ().text = SkillManagerComponent.Instance.skillList[1].skillDescription;
			skillDamage.GetComponentInChildren<Text> ().text = "10 HP";
			skillHeal.GetComponentInChildren<Text> ().text = "5 HP";
			skillName.GetComponentInChildren<Text> ().text = SkillManagerComponent.Instance.skillList[1].skillName.ToString();
			break;
		case "Rejuvenation":
			skillOverViewDesc.GetComponentInChildren<Text> ().text = SkillManagerComponent.Instance.skillList[2].skillDescription;
			skillDamage.GetComponentInChildren<Text> ().text = "0 HP";
			skillHeal.GetComponentInChildren<Text> ().text = "8 HP";
			skillName.GetComponentInChildren<Text> ().text = SkillManagerComponent.Instance.skillList[2].skillName.ToString();
			break;
		}
	}
	public void OnCloseSkillOverView(){
		TweenController.TweenTextScale (skillOverViewScreen.transform,new Vector3(1,1,1),0.5f);
		skillOverViewScreen.SetActive (false);
	}


}
