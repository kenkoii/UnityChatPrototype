using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenController : EnglishRoyaleElement {

	public void TweenPlayerGPSlider(float endValue, float duration, bool snapping){
		app.model.tweenModel.playerGpSlider.DOValue (endValue, duration, snapping);
	}

	public void TweenPlayerHPSlider(float endValue, float duration, bool snapping){
		app.model.tweenModel.playerHpSlider.DOValue (endValue, duration, snapping);
	}

	public void TweenEnemyHPSlider(float endValue, float duration, bool snapping){
		app.model.tweenModel.enemyHpSlider.DOValue (endValue, duration, snapping);
	}

	public void TestButton(){
		TweenPlayerHPSlider (10, 1, true);
	}
}
