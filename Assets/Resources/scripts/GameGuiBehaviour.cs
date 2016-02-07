using UnityEngine;
using System.Collections;

public class GameGuiBehaviour : MonoBehaviour {
	
	public void pauseButtonCallback()
	{
		GamePhaseController.showLoading();
		UnityEngine.SceneManagement.SceneManager.LoadScene(0,UnityEngine.SceneManagement.LoadSceneMode.Single);
	}
	
	public void runButtonCallback()
	{
		if(!States.WorldRunning)
		{
			//DebugConsole.getInstance().Log("FA");
			GameStorage.getInstance().launchStep();
		}
	}
}
