using UnityEngine.UI;

public class GameTimerView : EnglishRoyaleElement {

	public Text gameTimerText;
	public Image gameTimerImage;


	public void ToggleTimer(bool toggleFlag){
		gameTimerText.enabled = toggleFlag;
		gameTimerImage.enabled = toggleFlag;
	}
		

}
