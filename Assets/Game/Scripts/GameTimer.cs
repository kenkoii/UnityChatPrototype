using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : SingletonMonoBehaviour<GameTimer> {

	public Text gameTimerText;
	private Image gameTimerImage;

	void Awake(){
		gameTimerText = this.transform.GetChild (0).GetComponent<Text> ();
		gameTimerImage = this.GetComponent<Image> ();
	}


	public void ToggleTimer(bool toggleFlag){
		gameTimerText.enabled = toggleFlag;
		gameTimerImage.enabled = toggleFlag;
	}
		

}
