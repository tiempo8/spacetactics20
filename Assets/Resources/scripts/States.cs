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
	
	public static bool gamePhaseGuiEnabled=true;
	
	public enum SelectedType : int {
		NONE=-1,
		FRIENDLY=0,
		ENEMY=1
	}
}
