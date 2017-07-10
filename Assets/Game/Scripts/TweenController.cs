using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class TweenController{

	//HP GP Sliders
	public static void TweenPlayerGPSlider(float endValue, float duration, bool snapping, Slider playerGpSlider){
		TweenSlider (playerGpSlider, endValue, duration, snapping);
	}

	public static void TweenPlayerHPSlider(float endValue, float duration, bool snapping, Slider playerHpSlider){
		TweenSlider (playerHpSlider, endValue, duration, snapping);
	}

	public static void TweenEnemyHPSlider(float endValue, float duration, bool snapping, Slider enemyHpSlider){
		TweenSlider (enemyHpSlider, endValue, duration, snapping);
	}

	private static void TweenSlider(Slider slider,float endValue, float duration, bool snapping){
		slider.DOValue (endValue, duration, snapping).SetEase(Ease.InOutCirc);
	}
		

	//WaitOpponent

	public static void TweenStartWaitOpponent(float duration, Transform waitOpponentGroup,RectTransform loadingIndicator){
		waitOpponentGroup.DOScale(new Vector3(1,1,1), duration);
		TweenRotateForever (loadingIndicator);
	}

	public static void TweenStopWaitOpponent(float duration, Transform waitOpponentGroup){
		waitOpponentGroup.DOScale(new Vector3(1,0,0), duration);
	}

	public static void TweenRotateForever(RectTransform rotateObject){
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(rotateObject.DOLocalRotate(new Vector3(0, 0, -360), 5, RotateMode.FastBeyond360).SetEase(Ease.Linear)).SetLoops(-1);
	}

	public static void TweenTextScale(Transform text, Vector3 endValue, float duration){
		text.transform.DOScale (endValue,duration).SetEase(Ease.InElastic,30,1);
	}

	public static void TweenScaleToLarge(Transform text, Vector3 endValue, float duration){
		text.transform.DOScale (new Vector3(0.1f,0.1f,0.1f),0.05f);
		text.transform.DOScale (endValue, duration).SetEase (Ease.OutElastic, 10, 1);
	}

	public static void TweenShakePosition(Transform obj,float duration, float strength, int vibrato, float randomness){
		obj.transform.DOShakePosition(duration, strength, vibrato,randomness, true);
	}

	public static void TweenJumpTo(Transform obj, Vector3 endValue, float jumpPower,int numJumps,float duration){
		obj.transform.DOLocalJump (endValue,jumpPower,numJumps,1,true);
	}

	public static void TweenMoveTo(Transform obj, Vector3 endValue,float duration){
		obj.transform.DOLocalMove (endValue,duration,true);
	}

}
