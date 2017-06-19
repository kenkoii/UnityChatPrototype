using UnityEngine;

public class EnglishRoyaleElement : MonoBehaviour
{
	// Gives access to the application and all instances.
	public  EnglishRoyaleApplication app { get { return GameObject.FindObjectOfType<EnglishRoyaleApplication> (); } }
}

//Entry Point.
public class EnglishRoyaleApplication : MonoBehaviour
{
	// Reference to the root instances of the MVC.
	public EnglishRoyaleModel model;
	public EnglishRoyaleView view;
	public EnglishRoyaleController controller;
	public EnglishRoyaleComponent component;

	// Init things here
	void Start ()
	{
	}
}
