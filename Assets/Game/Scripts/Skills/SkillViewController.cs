using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SkillViewController : MonoBehaviour {
	public GameObject skillView;
	public GameObject skillOverViewScreen;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnSkillButtonClick(Button b){
		TweenController.TweenMoveTo(skillView.transform, new Vector2(0,0), 0.5f);
	}

	public void OnClickSkillItem(){
		skillOverViewScreen.SetActive (true);
		TweenController.TweenScaleToLarge (skillOverViewScreen.transform,new Vector3(1,1,1),0.2f);
		skillOverViewScreen.transform.GetChild(1).GetChild(0).GetComponentInChildren<Image>().sprite = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Image> ().sprite;
	}
	public void OnCloseSkillOverView(){
		TweenController.TweenTextScale (skillOverViewScreen.transform,new Vector3(1,1,1),0.5f);
		skillOverViewScreen.SetActive (false);
	}
}
