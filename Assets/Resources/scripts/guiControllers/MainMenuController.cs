using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	public void GoToHelpFromMain()
	{
		MainMenuStarter.mainMenu.SetActive(false);
		MainMenuStarter.helpMenu.SetActive(true);
	}
	
	public void GoToPlotFromMain()
	{
		MainMenuStarter.mainMenu.SetActive(false);
		MainMenuStarter.plotMenu.SetActive(true);
	}
	
	public void GoToMainFromHelp()
	{
		MainMenuStarter.mainMenu.SetActive(true);
		MainMenuStarter.helpMenu.SetActive(false);
	}
	
	public void GoToMainFromPlot()
	{
		MainMenuStarter.mainMenu.SetActive(true);
		MainMenuStarter.plotMenu.SetActive(false);
	}
	
	public void GoToCampaignsFromMain()
	{
		MainMenuStarter.mainMenu.SetActive(false);
		MainMenuStarter.campaignsMenu.SetActive(true);
	}
	
	public void GoToMainFromCampaigns()
	{
		MainMenuStarter.campaignsMenu.SetActive(false);
		MainMenuStarter.mainMenu.SetActive(true);
	}
	
	public void GoToCampaignsFromMissions()
	{
		MainMenuStarter.missionsMenu.SetActive(false);
		MainMenuStarter.campaignsMenu.SetActive(true);
	}
}
