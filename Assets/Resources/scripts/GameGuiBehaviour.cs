using UnityEngine;
using System.Collections;

public class GameGuiBehaviour : MonoBehaviour {
	
	public GameObject pausePanel;
	public GameObject mainPanel;
	
	public GameObject missionNameLabel,missionDescLabel;
	
	public void pauseButtonCallback()
	{
		States.inPauseMenu=true;
		mainPanel.SetActive(false);
		pausePanel.SetActive(true);
		missionNameLabel.GetComponent<UnityEngine.UI.Text>().text="Mission "+(States.selectedLevel+1)+": "+Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).levelName;
		missionDescLabel.GetComponent<UnityEngine.UI.Text>().text=Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).description;
	}
	
	public void zalpButtonBehaviour()
	{
		GameStorage.getInstance().setRocketsAndThorpeds();
	}
	
	public void arrowPrevButtonCallback()
	{
		States.nextToFocus=GameStorage.getInstance().getPrevFriendlyToFocus();
	}
	
	public void arrowNextButtonCallback()
	{
		States.nextToFocus=GameStorage.getInstance().getNextFriendlyToFocus();
	}
	
	public void zoomInButtonCallback()
	{
		States.zoomAction=1;
	}
	
	public void zoomOutButtonCallback()
	{
		States.zoomAction=-1;
	}
	
	public void continueButtonCallback()
	{
		States.inPauseMenu=false;
		mainPanel.SetActive(true);
		pausePanel.SetActive(false);
	}
	
	public void nextButtonCallback()
	{
		States.retries=0;
		GamePhaseController.showLoading();
		pausePanel.SetActive(false);
		States.selectedLevel++;
		GameStorage.getInstance().loadLevel(States.selectedLevel);
	}
	
	public void restartButtonCallback()
	{
		States.retries++;
		mainPanel.SetActive(false);
		pausePanel.SetActive(false);
		GamePhaseController.showLoading();
		GameStorage.getInstance().loadLevel(States.selectedLevel);
		States.inPauseMenu=false;
	}
	
	public void mainButtonCallback()
	{
		States.retries=0;
		mainPanel.SetActive(false);
		pausePanel.SetActive(false);
		GamePhaseController.showLoading();
		UnityEngine.SceneManagement.SceneManager.LoadScene(0,UnityEngine.SceneManagement.LoadSceneMode.Single);
		States.inPauseMenu=false;
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
