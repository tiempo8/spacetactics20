using UnityEngine;
using System.Collections;

public class MainMenuStarter : MonoBehaviour {

	public static GameObject mainMenu;
	public static GameObject helpMenu;
	public static GameObject plotMenu;
	public static GameObject campaignsMenu;
	public static GameObject missionsMenu;
	public static GameObject start_but;
	
	private GameObject campaignsContent;
	private GameObject missionsContainer;
	private GameObject companyPanelTmp;
	
	private GameObject rank_text,rank_img;
	
	public static  GameObject descriptionPanel;
	private UnityEngine.UI.Button buttonTmp;
	
	private UnityEngine.UI.SpriteState selectedState;
	
	void Start () {
		
		Templates.getInstance();
		GameStorage.getInstance();
		
		//TEST
		//PlayerPrefs.DeleteAll();
		//TEST END
		
		mainMenu=GameObject.Find("main_panel");
		helpMenu=GameObject.Find("help_panel");
		helpMenu.SetActive(false);
		plotMenu=GameObject.Find("plot_panel");
		plotMenu.SetActive(false);
		
		rank_img=GameObject.Find("rank_img");
		rank_text=GameObject.Find("rank_text");
		
		start_but=GameObject.Find("start_but");
		missionsContainer=GameObject.Find("missions_container");
		descriptionPanel=GameObject.Find("description_panel");
		missionsMenu=GameObject.Find("missions_panel");
		missionsMenu.SetActive(false);
		
		campaignsContent=GameObject.Find("campaigns_content");
		campaignsMenu=GameObject.Find("campaigns_panel");
		foreach(Templates.CampaignInfo camp in Templates.getInstance().campaignsList)
		{
			companyPanelTmp=(GameObject)Instantiate(Resources.Load("prefab/gui/company_panel"),Vector3.zero,Quaternion.identity);
			companyPanelTmp.transform.SetParent(campaignsContent.transform);
			companyPanelTmp.transform.localScale=new Vector3(1,1,1);
			((UnityEngine.UI.Text)companyPanelTmp.GetComponentsInChildren<UnityEngine.UI.Text>()[0]).text=camp.name;
			((UnityEngine.UI.Text)companyPanelTmp.GetComponentsInChildren<UnityEngine.UI.Text>()[1]).text=camp.desc;
			buttonTmp=companyPanelTmp.GetComponent<UnityEngine.UI.Button>();
			Templates.CampaignInfo cap=camp;
			buttonTmp.onClick.AddListener(() => {
			                              	selectCampaign(cap);
			 });
		}
		campaignsMenu.SetActive(false);
		
	}
	
	public void selectCampaign(Templates.CampaignInfo camp)
	{
		States.selectedLevel=-1;
		int i;
		start_but.SetActive(false);
		for(i=0;i<missionsContainer.transform.childCount; i++)
			Destroy(missionsContainer.transform.GetChild(i).gameObject);
		States.currentCampaign=camp;
		//load load
		campaignsMenu.SetActive(false);
		missionsMenu.SetActive(true);
		((UnityEngine.UI.Text)descriptionPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[0]).text=camp.name;
		((UnityEngine.UI.Text)descriptionPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1]).text=camp.desc;
		
		int currank= PlayerPrefs.GetInt("rank"+States.currentCampaign.id+"Campaign",-1);
		if(currank==-1)
		{
			rank_text.GetComponent<UnityEngine.UI.Text>().text="Rank:\n"+Templates.getInstance().getRank(States.currentCampaign.defaultRank).name;
			rank_img.GetComponent<UnityEngine.UI.Image>().overrideSprite=Templates.getInstance().getRank(States.currentCampaign.defaultRank).img;
		}
		else
		{
			rank_text.GetComponent<UnityEngine.UI.Text>().text="Rank:\n"+Templates.getInstance().getRank(currank).name;
			rank_img.GetComponent<UnityEngine.UI.Image>().overrideSprite=Templates.getInstance().getRank(currank).img;
		}
		
		int repetitions=camp.levels.Count/2;
		bool free=true;
		int stars1,stars2=0;
		for(i=0;i<repetitions;i++)
		{
			stars1=PlayerPrefs.GetInt("mission"+camp.levels[i*2]+"Stars",-1);
			stars2=PlayerPrefs.GetInt("mission"+camp.levels[i*2+1]+"Stars",-1);
			GameObject tmp=(GameObject)Instantiate(Templates.getInstance().missionBlockPrefab,Vector3.zero,Quaternion.identity);
			selectedState=new UnityEngine.UI.SpriteState();
			selectedState.disabledSprite=((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).spriteState.pressedSprite;
			selectedState.highlightedSprite=((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).spriteState.pressedSprite;
			selectedState.pressedSprite=((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).spriteState.pressedSprite;
			tmp.transform.SetParent(missionsContainer.transform);
			tmp.transform.localScale=new Vector3(1,1,1);
			
			if(stars1==-1)
			{
				if(!free)
				{
					((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).enabled=false;
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[0]).overrideSprite=((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).spriteState.disabledSprite;
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[1]).overrideSprite = Templates.getInstance().getNumberGrey(i*2+1)[0];
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[2]).overrideSprite = Templates.getInstance().getNumberGrey(i*2+1)[1];
			
				}
				else
				{
					free=false;
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[1]).overrideSprite = Templates.getInstance().getNumber(i*2+1)[0];
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[2]).overrideSprite = Templates.getInstance().getNumber(i*2+1)[1];
				}
			}
			else
			{
				((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[1]).overrideSprite = Templates.getInstance().getNumber(i*2+1)[0];
				((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[2]).overrideSprite = Templates.getInstance().getNumber(i*2+1)[1];
				if(stars1>=1)
				{
					Color c = ((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.transform.GetChild(3).gameObject.GetComponent<UnityEngine.UI.Image>().color;
					c.a=1;
					((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.transform.GetChild(3).gameObject.GetComponent<UnityEngine.UI.Image>().color=c;
				}
				if(stars1>=2)
				{
					Color c = ((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Image>().color;
					c.a=1;
					((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Image>().color=c;
				}
				if(stars1>=3)
				{
					Color c = ((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.transform.GetChild(5).gameObject.GetComponent<UnityEngine.UI.Image>().color;
					c.a=1;
					((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.transform.GetChild(5).gameObject.GetComponent<UnityEngine.UI.Image>().color=c;
				}
				
			}
			
			if(stars2==-1)
			{
				if(!free)
				{
					((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).enabled=false;
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[0]).overrideSprite=((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).spriteState.disabledSprite;
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[1]).overrideSprite = Templates.getInstance().getNumberGrey(i*2+2)[0];
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[2]).overrideSprite = Templates.getInstance().getNumberGrey(i*2+2)[1];
				}
				else
				{
					free=false;
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[1]).overrideSprite = Templates.getInstance().getNumber(i*2+2)[0];
					((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[2]).overrideSprite = Templates.getInstance().getNumber(i*2+2)[1];
				}
			}
			else
			{
				((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[1]).overrideSprite = Templates.getInstance().getNumber(i*2+2)[0];
				((UnityEngine.UI.Image)((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[2]).overrideSprite = Templates.getInstance().getNumber(i*2+2)[1];
				if(stars1>=1)
				{
					Color c = ((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.transform.GetChild(3).gameObject.GetComponent<UnityEngine.UI.Image>().color;
					c.a=1;
					((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.transform.GetChild(3).gameObject.GetComponent<UnityEngine.UI.Image>().color=c;
				}
				if(stars1>=2)
				{
					Color c = ((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Image>().color;
					c.a=1;
					((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Image>().color=c;
				}
				if(stars1>=3)
				{
					Color c = ((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.transform.GetChild(5).gameObject.GetComponent<UnityEngine.UI.Image>().color;
					c.a=1;
					((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.transform.GetChild(5).gameObject.GetComponent<UnityEngine.UI.Image>().color=c;
				}
			}
			
			GameObject block=tmp;
			int lev=(i*2);
			((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).onClick.AddListener(() => setSelection(block,lev));
			((UnityEngine.UI.Button)tmp.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).onClick.AddListener(() => setSelection(block,lev+1));
		}
	}
	
	public void setSelection(GameObject block, int lev)
	{
		if(States.selectedLevel==lev)
		{
			GamePhaseController.showLoading();
			GameStorage.getInstance().loadLevel(lev);
		}
		if(States.selectedLevel!=lev)
		{
			if(lev%2==0)
			{
				((UnityEngine.UI.Image)((UnityEngine.UI.Button)block.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[0]).overrideSprite=((UnityEngine.UI.Button)block.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).spriteState.pressedSprite;
				((UnityEngine.UI.Button)block.GetComponentsInChildren<UnityEngine.UI.Button>()[0]).spriteState=selectedState;
			}
			if(lev%2==1)
			{
				((UnityEngine.UI.Image)((UnityEngine.UI.Button)block.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).gameObject.GetComponentsInChildren<UnityEngine.UI.Image>()[0]).overrideSprite=((UnityEngine.UI.Button)block.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).spriteState.pressedSprite;
				((UnityEngine.UI.Button)block.GetComponentsInChildren<UnityEngine.UI.Button>()[1]).spriteState=selectedState;
			}
			descriptionPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[0].text=((Templates.LevelInfo)Templates.getInstance().getLevel((int)States.currentCampaign.levels[lev])).levelName;
			descriptionPanel.GetComponentsInChildren<UnityEngine.UI.Text>()[1].text=((Templates.LevelInfo)Templates.getInstance().getLevel((int)States.currentCampaign.levels[lev])).description;
			States.selectedLevel=lev;
			start_but.SetActive(true);
		}
	}
}
