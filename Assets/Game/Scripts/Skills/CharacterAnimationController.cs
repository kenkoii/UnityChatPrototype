using UnityEngine;

public class CharacterAnimationController :  SingletonMonoBehaviour<CharacterAnimationController>
{

	public Animator playerAnim;
	public Animator enemyAnim;


	public void SetTriggerAnim(bool isPLayer, string param){
		if (isPLayer) {
			playerAnim.SetTrigger(param);
		} else {
			enemyAnim.SetTrigger(param);
		}
	}

	public void SetBoolAnim(bool isPLayer, string param, bool value){
		if (isPLayer) {
			playerAnim.SetBool (param, value);
		} else {
			enemyAnim.SetBool (param, value);
		}
	}

	public void SetFloatAnim(bool isPLayer, string param, float value){
		if (isPLayer) {
			playerAnim.SetFloat (param, value);
		} else {
			enemyAnim.SetFloat (param, value);
		}
	}

	public void SetIntAnim(bool isPLayer, string param, int value){
		if (isPLayer) {
			playerAnim.SetInteger (param, value);
		} else {
			enemyAnim.SetInteger (param, value);

		}
	}
		


}
