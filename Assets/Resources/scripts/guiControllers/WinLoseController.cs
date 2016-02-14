using UnityEngine;
using System.Collections;

public class WinLoseController : MonoBehaviour {

	public GameObject gamePanel;
	public GameObject winlosePanel;
	public GameObject pausePanel;
	public GameObject okLabel,failLabel;
	public GameObject failHint;
	public GameObject[] stars;
	public GameObject nextButton;
	
	public GameObject reachRankText,reachRankImg;
	
	void Start () {
		States.winLoseController=this;
	}
	
	public void ShowWinlose()
	{
		States.inPauseMenu=true;
		pausePanel.SetActive(false);
		gamePanel.SetActive(false);
		winlosePanel.SetActive(true);
		if(States.winloseResult==-1)
		{
			reachRankImg.SetActive(false);
			reachRankText.SetActive(false);
			failLabel.SetActive(true);
			nextButton.GetComponent<UnityEngine.UI.Button>().interactable=false;
			okLabel.SetActive(false);
			foreach(GameObject obj in stars)
				obj.SetActive(false);
		
			if(States.retries>=States.WinLoseSettings.triesToHint)
			{
				failHint.SetActive(true);
				failHint.GetComponent<UnityEngine.UI.Text>().text="Hint: "+Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).hint;
			}
			else
				failHint.SetActive(false);
		}
		else if(States.winloseResult>=1)
		{
			if(Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).rankReached!=-1)
			{
				if(PlayerPrefs.GetInt("rank"+States.currentCampaign.id+"Campaign",-1)==-1)
				{
					reachRankText.SetActive(true);
					reachRankImg.SetActive(true);
					reachRankText.GetComponent<UnityEngine.UI.Text>().text="New rank reached:\n"+Templates.getInstance().getRank(Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).rankReached).name;
					reachRankImg.GetComponent<UnityEngine.UI.Image>().overrideSprite=Templates.getInstance().getRank(Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).rankReached).img;
					PlayerPrefs.SetInt("rank"+States.currentCampaign.id+"Campaign",Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).rankReached);
				}
				else
				{
					if(Templates.getInstance().getRank(Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).rankReached).level>Templates.getInstance().getRank(PlayerPrefs.GetInt("rank"+States.currentCampaign.id+"Campaign",-1)).level)
					{
						reachRankText.SetActive(true);
						reachRankImg.SetActive(true);
						reachRankText.GetComponent<UnityEngine.UI.Text>().text="New rank reached:\n"+Templates.getInstance().getRank(Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).rankReached).name;
						reachRankImg.GetComponent<UnityEngine.UI.Image>().overrideSprite=Templates.getInstance().getRank(Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).rankReached).img;
						PlayerPrefs.SetInt("rank"+States.currentCampaign.id+"Campaign",Templates.getInstance().getLevel((int)States.currentCampaign.levels[States.selectedLevel]).rankReached);
					}
					else
					{
						reachRankText.SetActive(false);
						reachRankImg.SetActive(false);
					}
				}
				
			}
			else
			{
				reachRankText.SetActive(false);
				reachRankImg.SetActive(false);
			}
			if(States.winloseResult>PlayerPrefs.GetInt("mission"+((int)States.currentCampaign.levels[States.selectedLevel])+"Stars",-1))
				PlayerPrefs.SetInt("mission"+((int)States.currentCampaign.levels[States.selectedLevel])+"Stars",States.winloseResult);
			okLabel.SetActive(true);
			if(States.selectedLevel==States.currentCampaign.levels.Count-1)
				nextButton.GetComponent<UnityEngine.UI.Button>().interactable=false;
			foreach(GameObject obj in stars)
				obj.SetActive(false);
			failLabel.SetActive(false);
			failHint.SetActive(false);
			stars[0].SetActive(true);
			if(States.winloseResult>=2)
				stars[1].SetActive(true);
			if(States.winloseResult>=3)
				stars[2].SetActive(true);
		}
	}
}
