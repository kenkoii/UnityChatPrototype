using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenController : EnglishRoyaleElement {

	//HP GP Sliders
	public void TweenPlayerGPSlider(float endValue, float duration, bool snapping){
		TweenSlider (app.model.tweenModel.playerGpSlider, endValue, duration, snapping);
	}

	public void TweenPlayerHPSlider(float endValue, float duration, bool snapping){
		TweenSlider (app.model.tweenModel.enemyHpSlider, endValue, duration, snapping);
	}

	public void TweenEnemyHPSlider(float endValue, float duration, bool snapping){
		TweenSlider (app.model.tweenModel.enemyHpSlider, endValue, duration, snapping);
	}

	private void TweenSlider(Slider slider,float endValue, float duration, bool snapping){
		slider.DOValue (endValue, duration, snapping);
	}

	//HP GP Group
	public void TweenPlayerGPGroup(Vector3 scale, float duration){
		TweenGroup (app.model.tweenModel.playerGPGroup, scale, duration);
	}

	public void TweenPlayerHPGroup(Vector3 scale, float duration){
		TweenGroup (app.model.tweenModel.playerHPGroup, scale, duration);
	}

	public void TweenEnemyHPGroup(Vector3 scale, float duration){
		TweenGroup (app.model.tweenModel.enemyHPGroup, scale, duration);
	}


	private void TweenGroup(Transform tweenGroup,Vector3 scale, float duration){
		Sequence mySequence = DOTween.Sequence ();
		mySequence.Append (tweenGroup.DOScale (scale, duration));
		mySequence.Append (tweenGroup.DOScale (new Vector3(1,1,1), duration));
	}

	//WaitOpponent

	public void TweenStartWaitOpponent(float duration){
		app.model.tweenModel.waitOpponentGroup.DOScale(new Vector3(1,1,1), duration);
		TweenRotateForever (app.model.tweenModel.loadingIndicator);
	}

	public void TweenStopWaitOpponent(float duration){
		app.model.tweenModel.waitOpponentGroup.DOScale(new Vector3(1,0,0), duration);
	}

	public void TweenRotateForever(Transform rotateObject){
		
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(rotateObject.DORotate(new Vector3(0, 0, -360), 5, RotateMode.FastBeyond360).SetEase(Ease.Linear)).SetLoops(-1);
	}


	//for test tween
	public void TestButton(){
		TweenStartWaitOpponent (0.2f);
	}
}
