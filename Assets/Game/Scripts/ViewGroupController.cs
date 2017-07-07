using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewGroupController : MonoBehaviour {
	public GameObject skillView;
	public GameObject viewGroup;

	public void LerpProfileView(Button start){
		TweenController.TweenMoveTo(viewGroup.transform, new Vector2(0,viewGroup.transform.localPosition.y +1280f), 0.3f);
	}
	public void LerpSkillView(Button skill){
		TweenController.TweenMoveTo(viewGroup.transform, new Vector2(viewGroup.transform.localPosition.x - 845f, viewGroup.transform.localPosition.y), 0.5f);
	}
	public void LerpBackToHome(Button home){
		TweenController.TweenMoveTo(viewGroup.transform, new Vector2(0,1280f), 0.5f);
	}
	public void LerpToMatch(Button match){
		TweenController.TweenMoveTo(viewGroup.transform, new Vector2(viewGroup.transform.localPosition.x + 870f, viewGroup.transform.localPosition.y), 0.5f);
	}
}
