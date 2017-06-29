using UnityEngine.UI;
using UnityEngine;

public class GestureView : EnglishRoyaleElement
{

	public void ShowGestureButtons ()
	{
		app.controller.gestureController.ShowGestureButtons ();
	}

	public void HideGestureButton ()
	{
		app.controller.gestureController.HideGestureButton ();
	}

	public void ShowGesture1 ()
	{
		app.controller.gestureController.ShowPlayerGesture1 ();
	}

	public void ShowGesture2 ()
	{
		app.controller.gestureController.ShowPlayerGesture2 ();
	}

	public void ShowGesture3 ()
	{
		app.controller.gestureController.ShowPlayerGesture3 ();
	}

	public void ShowGesture4 ()
	{
		app.controller.gestureController.ShowPlayerGesture4 ();
	}

}
