using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioController : SingletonMonoBehaviour<AudioController> {

	private AudioSource bgm;

	void Start(){
		bgm = GetComponent<AudioSource>();
	}


	public void PlayBGM(string bgmName){
		
		bgm.clip = Resources.Load ("Audio/" + bgmName) as AudioClip;
		bgm.Play ();
		
	}

	//create and destroy sfx after play to avoid conflict
	public void PlaySFX(string sfxName){

		AudioSource newAudio = gameObject.AddComponent<AudioSource> ();
		newAudio.clip = Resources.Load ("Audio/" + sfxName) as AudioClip;
		newAudio.Play ();

		float audioLength = newAudio.clip.length;
		StartCoroutine (DestroyBGM(audioLength, delegate() {
			Destroy(newAudio);
		}));
	}

	IEnumerator DestroyBGM(float time,Action action){
		yield return new WaitForSeconds (time);
		action();
	}

}
