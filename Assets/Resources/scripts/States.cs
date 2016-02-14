using UnityEngine;
using System.Collections;

public static class States {
	public static Templates.CampaignInfo currentCampaign;
	public static ArrayList objectsInMissionBlock = new ArrayList();
	public static int selectedLevel=-1;
	public static GameObject currentSelected=null;
	public static float currentZoom=20;
	public static GameObject currentAttackIconCaptured=null;
	public static FriendlySpaceship parentSpaceshipInstance=null;
	public static MissleBehaviour parentMissleInstance=null;
	public static SelectedType currentSelectedTag=SelectedType.NONE;
	public static bool WorldRunning=false;
	public static int winloseResult=0;
	public static int SummaryHp=0;
	public static int retries=0;
	public static WinLoseController winLoseController;
	public static int zoomAction=0;
	public static FriendlySpaceship nextToFocus=null;
	
	public static bool inPauseMenu=false;
	
	public static bool gamePhaseGuiEnabled=true;
	
	public enum SelectedType : int {
		NONE=-1,
		FRIENDLY=0,
		ENEMY=1
	}
	
	public static class WinLoseSettings
	{
		public static int triesToHint=1;
		private const int FIRST_PLANC=40;
		private const int SECOND_PLANC=60;
		
		public static int getStars(int x)
		{
			if(x >= 0 && x <= FIRST_PLANC)
				return 1;
			if(x > FIRST_PLANC && x <= SECOND_PLANC)
				return 2;
			if(x > SECOND_PLANC && x <= 100)
				return 3;
			return -2;
		}
	}
}
