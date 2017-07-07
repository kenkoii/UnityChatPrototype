using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
		TweenController.TweenTextScale (skillOverViewScreen.transform,new Vector3(8,8,8),0.5f);
	}
	public void OnCloseSkillOverView(){
		TweenController.TweenTextScale (skillOverViewScreen.transform,new Vector3(1,1,1),0.5f);
		skillOverViewScreen.SetActive (false);
	}
}
