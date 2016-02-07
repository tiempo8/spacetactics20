using UnityEngine;
using System.Collections;

public class GamePhaseController : MonoBehaviour {
	private static GamePhaseController instance;
	
	public static GamePhaseController getInstance()
	{
		return instance;
	}
	
	void Start()
	{
		instance=this;
		//Debug.Log("initialized");
	}
	
	public GameObject mainPanel;
	public GameObject loadingPanel;
	
	public static void showLoading()
	{
		States.gamePhaseGuiEnabled=false;
		if(getInstance().mainPanel!=null)
			getInstance().mainPanel.SetActive(false);
		if(getInstance().loadingPanel!=null)
			getInstance().loadingPanel.SetActive(true);
	}
}
