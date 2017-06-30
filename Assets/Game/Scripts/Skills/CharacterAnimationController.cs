using UnityEngine;

public class CharacterAnimationController :  EnglishRoyaleElement
{
	public void SetTriggerAnim(bool isPLayer, string param){
		if (isPLayer) {
			app.model.characterAnimationModel.playerAnim.SetTrigger(param);
		} else {
			app.model.characterAnimationModel.enemyAnim.SetTrigger(param);
		}
	}
}
