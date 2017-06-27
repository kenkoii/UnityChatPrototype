public class CameraWorksController : EnglishRoyaleElement {

	public void StartIntroCamera(){
		app.model.cameraWorksModel.introCamera.SetActive (true);
		app.model.cameraWorksModel.winLoseCamera.SetActive (false);
	}

	public void StartWinLoseCamera(){
		app.model.cameraWorksModel.introCamera.SetActive (false);
		app.model.cameraWorksModel.winLoseCamera.SetActive (true);
	}
}
