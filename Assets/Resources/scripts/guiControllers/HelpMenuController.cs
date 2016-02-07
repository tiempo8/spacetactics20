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
		//GamePhaseController.showLoading();
		Debug.Log("Selected lev: "+States.currentCampaign.levels[States.selectedLevel]);
		States.gamePhaseGuiEnabled=true;
		BulletPoolManager.getInstance().initialize();
		GameStorage.getInstance().clearStorage();
		UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		StaticBatchingUtility.Combine(GameObject.Find("bounds"));
		Resources.UnloadUnusedAssets();
	}
}
