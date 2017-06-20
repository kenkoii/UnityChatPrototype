using UnityEngine;

public class EnglishRoyaleElement : MonoBehaviour
{
	private EnglishRoyaleApplication englishRoyalApp;

	public  EnglishRoyaleApplication app {
		get { 
			return englishRoyalApp;
		}
	}

	void Awake(){
		englishRoyalApp = GameObject.FindObjectOfType<EnglishRoyaleApplication> ();
	}
}
