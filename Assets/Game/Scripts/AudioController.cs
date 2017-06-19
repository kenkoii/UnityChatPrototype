using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : EnglishRoyaleElement
{


	//create and destroy sfx after play to avoid conflict
	public void PlayAudio (AudioEnum audioName)
	{
		AudioSource selectedAudio = null;

		switch (audioName) {
		case AudioEnum.Bgm:
			app.view.audioView.bgm.Play ();
			break;
		case AudioEnum.ClickButton:
			app.view.audioView.clickButton.Play ();
			break;
		case AudioEnum.Attack:
			app.view.audioView.attack.Play ();
			break;
		case AudioEnum.Lose:
			app.view.audioView.lose.Play ();
			break;
		case AudioEnum.Skill:
			app.view.audioView.skill.Play ();
			break;
		case AudioEnum.Win:
			app.view.audioView.win.Play ();
			break;
		case AudioEnum.Hit:
			app.view.audioView.hit.Play ();
			break;

		}

	}
		

}
