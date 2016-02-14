using UnityEngine;
using System.Collections;

public class HelpMenuController : MonoBehaviour {
	
	public void OnEv()
	{
		MainMenuStarter.descriptionPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[0].text=States.currentCampaign.name;
		MainMenuStarter.descriptionPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text=States.currentCampaign.desc;
		States.selectedLevel=-1;
		MainMenuStarter.start_but.SetActive(false);
	}
	
	public void loadLevel()
	{
		GamePhaseController.showLoading();
		GameStorage.getInstance().loadLevel(States.selectedLevel);
	}
	
	public void CLEAR()
	{
		PlayerPrefs.DeleteAll();
	}
}
