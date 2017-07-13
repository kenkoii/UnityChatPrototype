using UnityEngine;
using UnityEngine.UI;

public class ViewGroupController : MonoBehaviour {
	public GameObject skillView;
	public GameObject viewGroup;
	public GameObject matchView;
	public GameObject menu;
	public GameObject profileView;
	public GameObject skillButton;
	public GameObject matchButton;
	public GameObject homeButton;
	public GameObject lobbyView;
	public GameObject menuIndicator;
	public void LerpProfileView(Button start){
		TweenController.TweenMoveTo(viewGroup.transform, profileView.transform.localPosition, 0.3f);
		menu.SetActive (true);
		lobbyView.SetActive (false);
	}
	public void LerpSkillView(Button skill){
		TweenController.TweenMoveTo (viewGroup.transform, -skillView.transform.localPosition, 0.5f);
		TweenController.TweenMoveTo (menuIndicator.transform, skillButton.transform.localPosition, 0.3f);
			colorToDefault ();
			skill.gameObject.GetComponentInChildren<Image> ().color = new Color32 (66, 135, 199, 255);
		TweenController.TweenScaleToLarge(skill.transform.GetChild(0).transform, new Vector3(1.3f,1.3f,1.3f), 0.2f);
	}
	public void LerpBackToHome(Button home){
		TweenController.TweenMoveTo(viewGroup.transform, new Vector2(0,0), 0.5f);
		TweenController.TweenMoveTo (menuIndicator.transform, homeButton.transform.localPosition, 0.3f);
		colorToDefault ();
		home.gameObject.GetComponentInChildren<Image> ().color = new Color32 (66, 135, 199, 255);
		TweenController.TweenScaleToLarge(home.transform.GetChild(0).transform, new Vector3(1.3f,1.3f,1.3f), 0.2f);
	}
	public void LerpToMatch(Button match){
		TweenController.TweenMoveTo(viewGroup.transform, -matchView.transform.localPosition, 0.5f);
		TweenController.TweenMoveTo (menuIndicator.transform, matchButton.transform.localPosition, 0.3f);
		colorToDefault ();
		match.gameObject.GetComponentInChildren<Image> ().color = new Color32 (66, 135, 199, 255);
		TweenController.TweenScaleToLarge(match.transform.GetChild(0).transform, new Vector3(1.3f,1.3f,1.3f), 0.2f);
	}
	public void colorToDefault(){
		skillButton.gameObject.GetComponentInChildren<Image> ().color =  new Color32 (20, 65, 96, 255);
		matchButton.gameObject.GetComponentInChildren<Image> ().color =  new Color32 (20, 65, 96, 255);
		homeButton.gameObject.GetComponentInChildren<Image> ().color =  new Color32 (20, 65, 96, 255);
		TweenController.TweenTextScale(skillButton.transform.GetChild(0).transform, Vector3.one, 0.2f);
		TweenController.TweenTextScale(matchButton.transform.GetChild(0).transform, Vector3.one, 0.2f);
		TweenController.TweenTextScale(homeButton.transform.GetChild(0).transform, Vector3.one, 0.2f);
	}
}
