using UnityEngine;

public abstract class AbstractPhase : MonoBehaviour {
	protected bool stoptimer = false;
	protected int timeLeft;

	public abstract void OnStartPhase ();

	public abstract void OnEndPhase();

}
