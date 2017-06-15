using UnityEngine;

public class CharacterAnimationController :  SingletonMonoBehaviour<CharacterAnimationController>
{

	public Animator playerAnim;
	public Animator enemyAnim;

	public void Attack(bool isPLayer){
		if (isPLayer) {
			playerAnim.SetBool ("attack", true);
		} else {
			enemyAnim.SetBool ("attack", true);
		}
	}


}
