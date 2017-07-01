﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TweenController : EnglishRoyaleElement {

	//HP GP Sliders
	public void TweenPlayerGPSlider(float endValue, float duration, bool snapping){
		TweenSlider (app.model.tweenModel.playerGpSlider, endValue, duration, snapping);
		TweenPlayerGPGroup (new Vector3(1.2F,1.2F,1.2F), 1);
	}

	public void TweenPlayerHPSlider(float endValue, float duration, bool snapping){
		TweenSlider (app.model.tweenModel.playerHpSlider, endValue, duration, snapping);
		TweenPlayerHPGroup (new Vector3(1.2F,1.2F,1.2F), 1);
	}

	public void TweenEnemyHPSlider(float endValue, float duration, bool snapping){
		TweenSlider (app.model.tweenModel.enemyHpSlider, endValue, duration, snapping);
		TweenEnemyHPGroup (new Vector3(1.2F,1.2F,1.2F), 1);
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

	public void TweenRotateForever(RectTransform rotateObject){
		
		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(rotateObject.DOLocalRotate(new Vector3(0, 0, -360), 5, RotateMode.FastBeyond360).SetEase(Ease.Linear)).SetLoops(-1);
	}

	public static void TweenTextScale(Transform text, Vector3 endValue, float duration){
		text.transform.DOScale (endValue,duration);
	}

	public static void TweenShakePosition(Transform obj,float duration, float strength, int vibrato, float randomness){
	//	obj.transform.DOShakePosition(1.0f, 30.0f, 50,90);
		obj.transform.DOShakePosition(duration, strength, vibrato,randomness, true);
	}
	//for test tween
	public void TestButton(){
		TweenStartWaitOpponent (0.2f);
	}
	public static void TweenJumpTo(Transform obj, Vector3 endValue, float jumpPower,int numJumps,float duration){
		//obj.DOJump ();
	}
}
