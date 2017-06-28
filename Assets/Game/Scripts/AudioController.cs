﻿public class AudioController : EnglishRoyaleElement
{
	//create and destroy sfx after play to avoid conflict
	public void PlayAudio (AudioEnum audioName)
	{
		switch (audioName) {
		case AudioEnum.Bgm:
			app.model.audioModel.bgm.Play ();
			break;
		case AudioEnum.ClickButton:
			app.model.audioModel.clickButton.Play ();
			break;
		case AudioEnum.Attack:
			app.model.audioModel.attack.Play ();
			break;
		case AudioEnum.Lose:
			app.model.audioModel.lose.Play ();
			break;
		case AudioEnum.Skill:
			app.model.audioModel.skill.Play ();
			break;
		case AudioEnum.Win:
			app.model.audioModel.win.Play ();
			break;
		case AudioEnum.Hit:
			app.model.audioModel.hit.Play ();
			break;
		case AudioEnum.Correct:
			app.model.audioModel.correct.Play ();
			break;
		case AudioEnum.Mistake:
			app.model.audioModel.mistake.Play ();
			break;
		}

	}
}
