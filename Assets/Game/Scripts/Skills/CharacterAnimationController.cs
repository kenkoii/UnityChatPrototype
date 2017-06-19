using UnityEngine;

public class CharacterAnimationController :  EnglishRoyaleElement
{
	public void SetTriggerAnim(bool isPLayer, string param){
		if (isPLayer) {
			
			app.view.characterAnimationView.playerAnim.SetTrigger(param);
		} else {
			app.view.characterAnimationView.enemyAnim.SetTrigger(param);
		}
	}

}
