using UnityEngine;
using System.Collections;

public class MisslePoolManager {
	private static MisslePoolManager instance=null;
	public static MisslePoolManager getInstance()
	{
		if(instance==null)
			instance=new MisslePoolManager();
		return instance;
	}
	
	private ArrayList enemyRockets,friendlyRockets,activeFriendlyRockets,activeEnemyRockets;
	private ArrayList enemyThorpeds,friendlyThorpeds,activeFriendlyThorpedes,activeEnemyThorpeds;
	private ArrayList friendlyGasList,enemyGasList,activeFriendlyGasList,activeEnemyGasList;
	private ArrayList friendlyGasRemoveList,enemyGasRemoveList;
	private ArrayList friendlyMinesList,enemyMinesList,activeFriendlyMinesList,activeEnemyMinesList;
	private ArrayList friendlyMinesRemoveList,enemyMinesRemoveList;
	
	private GameObject tmp;
	private MissleBehaviour misTmp;
	private int i;
	public Vector3 stackPos = new Vector3(9999,9999,9999);
	
	private GameObject friendlyRocketPrefab,enemyRocketPrefab;
	private GameObject friendlyThorpedePrefab,enemyThorpedePrefab;
	private GameObject gasPrefab;
	private GameObject minePrefab;
	private FriendlySpaceship friendlyInstance;
	private EnemySpaceship enemyInstance;
	
	private MisslePoolManager()
	{
		enemyRockets=new ArrayList();
		friendlyRockets=new ArrayList();
		activeFriendlyRockets=new ArrayList();
		activeEnemyRockets=new ArrayList();
		friendlyThorpeds=new ArrayList();
		enemyThorpeds=new ArrayList();
		activeEnemyThorpeds=new ArrayList();
		activeFriendlyThorpedes=new ArrayList();
		
		friendlyGasList=new ArrayList();
		enemyGasList=new ArrayList();
		activeEnemyGasList=new ArrayList();
		activeFriendlyGasList=new ArrayList();
		enemyGasRemoveList=new ArrayList();
		friendlyGasRemoveList=new ArrayList();
		
		friendlyMinesList=new ArrayList();
		enemyMinesList=new ArrayList();
		activeEnemyMinesList=new ArrayList();
		activeFriendlyMinesList=new ArrayList();
		enemyMinesRemoveList=new ArrayList();
		friendlyMinesRemoveList=new ArrayList();
		
		friendlyRocketPrefab=(GameObject) Resources.Load("prefab/missles/friendly_rocket");
		enemyRocketPrefab=(GameObject) Resources.Load("prefab/missles/enemy_rocket");
		friendlyThorpedePrefab=(GameObject) Resources.Load("prefab/missles/friendly_thorpede");
		enemyThorpedePrefab=(GameObject) Resources.Load("prefab/missles/enemy_thorpede");
		gasPrefab=(GameObject) Resources.Load("prefab/missles/gas");
		minePrefab=(GameObject) Resources.Load("prefab/missles/mine");
	}
	
	public void initialize()
	{
		enemyRockets.Clear();
		friendlyRockets.Clear();
		activeFriendlyRockets.Clear();
		activeEnemyRockets.Clear();
		
		enemyThorpeds.Clear();
		friendlyThorpeds.Clear();
		activeEnemyThorpeds.Clear();
		activeFriendlyThorpedes.Clear();
		
		friendlyGasList.Clear();
		enemyGasList.Clear();
		activeFriendlyGasList.Clear();
		activeEnemyGasList.Clear();
		friendlyGasRemoveList.Clear();
		enemyGasRemoveList.Clear();
		
		friendlyMinesList.Clear();
		enemyMinesList.Clear();
		activeFriendlyMinesList.Clear();
		activeEnemyMinesList.Clear();
		friendlyMinesRemoveList.Clear();
		enemyMinesRemoveList.Clear();
	}
	
	public void MoveRocketsAndThorpeds()
	{
		foreach(MissleBehaviour obj in activeFriendlyRockets)
			obj.STEP();
		foreach(MissleBehaviour obj in activeEnemyRockets)
			obj.STEP();
		foreach(MissleBehaviour obj in activeEnemyThorpeds)
			obj.STEP();
		foreach(MissleBehaviour obj in activeFriendlyThorpedes)
			obj.STEP();
	}
	
	public void ToggleOnStepStart()
	{
		foreach(MissleBehaviour obj in activeFriendlyRockets)
			obj.onStepStart();
		foreach(MissleBehaviour obj in activeEnemyRockets)
			obj.onStepStart();
		foreach(MissleBehaviour obj in activeEnemyThorpeds)
			obj.onStepStart();
		foreach(MissleBehaviour obj in activeFriendlyThorpedes)
			obj.onStepStart();
		foreach(MissleBehaviour obj in activeFriendlyGasList)
			obj.onStepStart();
		foreach(MissleBehaviour obj in activeEnemyGasList)
			obj.onStepStart();
		foreach(MissleBehaviour obj in activeFriendlyMinesList)
			obj.onStepStart();
		foreach(MissleBehaviour obj in activeEnemyMinesList)
			obj.onStepStart();
	}
	
	public void ToggleOnStepEnd()
	{
		foreach(MissleBehaviour obj in activeFriendlyRockets)
			obj.onStepEnd();
		foreach(MissleBehaviour obj in activeEnemyRockets)
			obj.onStepEnd();
		foreach(MissleBehaviour obj in activeEnemyThorpeds)
			obj.onStepEnd();
		foreach(MissleBehaviour obj in activeFriendlyThorpedes)
			obj.onStepEnd();
		foreach(MissleBehaviour obj in activeFriendlyGasList)
			obj.onStepEnd();
		foreach(MissleBehaviour obj in activeEnemyGasList)
			obj.onStepEnd();
		
		foreach(MissleBehaviour obj in friendlyGasRemoveList)
		{
			activeFriendlyGasList.Remove(obj);
			DestroyGas(obj);
		}
		friendlyGasRemoveList.Clear();
		
		foreach(MissleBehaviour obj in enemyGasRemoveList)
		{
			activeEnemyGasList.Remove(obj);
			DestroyGas(obj);
		}
		enemyGasRemoveList.Clear();
		
		foreach(MissleBehaviour obj in activeFriendlyMinesList)
			obj.onStepEnd();
		foreach(MissleBehaviour obj in activeEnemyMinesList)
			obj.onStepEnd();
	}
	
	public void ToggleHideAllAttackIcons(MissleBehaviour exclude)
	{
		foreach(MissleBehaviour obj in activeFriendlyRockets)
			if(obj!=exclude)
				obj.hideIcon();
		foreach(MissleBehaviour obj in activeFriendlyThorpedes)
			if(obj!=exclude)
				obj.hideIcon();
		//THORPEDS
	}
	
	
	public void ToggleShowAllAttackIcons(MissleBehaviour exclude)
	{
		foreach(MissleBehaviour obj in activeFriendlyRockets)
			if(obj!=exclude)
				obj.showIcon();
		foreach(MissleBehaviour obj in activeFriendlyThorpedes)
			if(obj!=exclude)
				obj.showIcon();
		//THORPEDS
	}
	
	public void ToggleOnZoomChanged()
	{
		foreach(MissleBehaviour obj in activeFriendlyRockets)
			obj.onZoomChanged();
		foreach(MissleBehaviour obj in activeFriendlyThorpedes)
			obj.onZoomChanged();
		
		//THORPEDS
	}
	
	public void createPoolObjects(MissleSide side, MissleType type, int count)
	{
		for(i=0;i<count;i++)
			createPoolObjects(side,type);
	}
	
	public void createPoolObjects(MissleSide side, MissleType type)
	{
		switch(type)
		{
			case MissleType.ROCKET:
				if(side==MissleSide.FRIENDLY)
				{
					tmp=(GameObject) GameObject.Instantiate(friendlyRocketPrefab,stackPos,Quaternion.identity);
					tmp.SetActive(false);
					friendlyRockets.Add(tmp.GetComponent<MissleBehaviour>());
				}
				else if(side==MissleSide.ENEMY)
				{
					tmp=(GameObject) GameObject.Instantiate(enemyRocketPrefab,stackPos,Quaternion.identity);
					tmp.SetActive(false);
					enemyRockets.Add(tmp.GetComponent<MissleBehaviour>());
				}
				break;
				
			case MissleType.THORPEDE:
				if(side==MissleSide.FRIENDLY)
				{
					tmp=(GameObject) GameObject.Instantiate(friendlyThorpedePrefab,stackPos,Quaternion.identity);
					tmp.SetActive(false);
					friendlyThorpeds.Add(tmp.GetComponent<MissleBehaviour>());
				}
				else if(side==MissleSide.ENEMY)
				{
					tmp=(GameObject) GameObject.Instantiate(enemyThorpedePrefab,stackPos,Quaternion.identity);
					tmp.SetActive(false);
					enemyThorpeds.Add(tmp.GetComponent<MissleBehaviour>());
				}
				break;
				
			case MissleType.GAS:
				tmp=(GameObject) GameObject.Instantiate(gasPrefab,stackPos,Quaternion.identity);
				tmp.SetActive(false);
				tmp.GetComponent<MissleBehaviour>().side=side;
				if(side==MissleSide.FRIENDLY)
					friendlyGasList.Add(tmp.GetComponent<MissleBehaviour>());
				else if(side==MissleSide.ENEMY)
					enemyGasList.Add(tmp.GetComponent<MissleBehaviour>());
				break;
				
			case MissleType.MINES:
				tmp=(GameObject) GameObject.Instantiate(minePrefab,stackPos,Quaternion.identity);
				tmp.SetActive(false);
				tmp.GetComponent<MissleBehaviour>().side=side;
				if(side==MissleSide.FRIENDLY)
					friendlyMinesList.Add(tmp.GetComponent<MissleBehaviour>());
				else if(side==MissleSide.ENEMY)
					enemyMinesList.Add(tmp.GetComponent<MissleBehaviour>());
				break;
		}
	}
	
	private void bringToBack(MissleBehaviour missle)
	{
		if(missle.type==MissleType.ROCKET)
		{
			if(missle.side==MissleSide.FRIENDLY)
				friendlyRockets.Add(missle);
			if(missle.side==MissleSide.ENEMY)
				enemyRockets.Add(missle);
		}
		if(missle.type==MissleType.THORPEDE)
		{
			if(missle.side==MissleSide.FRIENDLY)
				friendlyThorpeds.Add(missle);
			if(missle.side==MissleSide.ENEMY)
				enemyThorpeds.Add(missle);
		}
		if(missle.type==MissleType.GAS)
		{
			if(missle.side==MissleSide.FRIENDLY)
				friendlyGasList.Add(missle);
			if(missle.side==MissleSide.ENEMY)
				enemyGasList.Add(missle);
		}
		if(missle.type==MissleType.MINES)
		{
			if(missle.side==MissleSide.FRIENDLY)
				friendlyMinesList.Add(missle);
			if(missle.side==MissleSide.ENEMY)
				enemyMinesList.Add(missle);
		}
	}
	
	private MissleBehaviour getFreeRocket(MissleSide side)
	{
		if(side==MissleSide.FRIENDLY)
		{
			foreach(MissleBehaviour missle in friendlyRockets)
			{
				misTmp=missle;
				break;
			}
			friendlyRockets.Remove(misTmp);
			return misTmp;
		}
		if(side==MissleSide.ENEMY)
		{
			foreach(MissleBehaviour missle in enemyRockets)
			{
				misTmp=missle;
				break;
			}
			enemyRockets.Remove(misTmp);
			return misTmp;
		}
		return null;
	}
	
	private MissleBehaviour getFreeMine(MissleSide side)
	{
		if(side==MissleSide.FRIENDLY)
		{
			foreach(MissleBehaviour missle in friendlyMinesList)
			{
				misTmp=missle;
				break;
			}
			friendlyMinesList.Remove(misTmp);
			return misTmp;
		}
		if(side==MissleSide.ENEMY)
		{
			foreach(MissleBehaviour missle in enemyMinesList)
			{
				misTmp=missle;
				break;
			}
			enemyMinesList.Remove(misTmp);
			return misTmp;
		}
		return null;
	}
	
	private MissleBehaviour getFreeGas(MissleSide side)
	{
		if(side==MissleSide.FRIENDLY)
		{
			foreach(MissleBehaviour missle in friendlyGasList)
			{
				misTmp=missle;
				break;
			}
			friendlyGasList.Remove(misTmp);
			return misTmp;
		}
		if(side==MissleSide.ENEMY)
		{
			foreach(MissleBehaviour missle in enemyGasList)
			{
				misTmp=missle;
				break;
			}
			enemyGasList.Remove(misTmp);
			return misTmp;
		}
		return null;
	}
	
	private MissleBehaviour getFreeThorpede(MissleSide side)
	{
		if(side==MissleSide.FRIENDLY)
		{
			foreach(MissleBehaviour missle in friendlyThorpeds)
			{
				misTmp=missle;
				break;
			}
			friendlyThorpeds.Remove(misTmp);
			return misTmp;
		}
		if(side==MissleSide.ENEMY)
		{
			foreach(MissleBehaviour missle in enemyThorpeds)
			{
				misTmp=missle;
				break;
			}
			enemyThorpeds.Remove(misTmp);
			return misTmp;
		}
		return null;
	}
	
	public void LaunchRocket(MissleSide side, Object beh)
	{
		if(side==MissleSide.FRIENDLY)
		{
			friendlyInstance=(FriendlySpaceship) beh;
			misTmp=getFreeRocket(side);
			misTmp.gameObject.transform.position=friendlyInstance.MisslesLaunchPoint.transform.position;
			misTmp.gameObject.transform.eulerAngles=new Vector3(270,friendlyInstance.transform.eulerAngles.y,0);
			misTmp.Activate();
			misTmp.hideIcon();
			misTmp.gameObject.SetActive(true);
			activeFriendlyRockets.Add(misTmp);
		}
		if(side==MissleSide.ENEMY)
		{
			enemyInstance=(EnemySpaceship) beh;
			misTmp=getFreeRocket(side);
			misTmp.gameObject.transform.position=enemyInstance.MissleLaunchPos.transform.position;
			misTmp.gameObject.transform.eulerAngles=new Vector3(270,enemyInstance.transform.eulerAngles.y,0);
			misTmp.Activate();
			misTmp.gameObject.SetActive(true);
			activeEnemyRockets.Add(misTmp);
		}
	}
	
	public void DestroyRocket(MissleBehaviour mis)
	{
		if(mis.side==MissleSide.ENEMY)
			activeEnemyRockets.Remove(mis);
		else if(mis.side==MissleSide.FRIENDLY)
			activeFriendlyRockets.Remove(mis);
		mis.gameObject.transform.position=stackPos;
		mis.gameObject.SetActive(false);
		bringToBack(mis);
	}
	
	public void LaunchThorpede(MissleSide side, Object beh)
	{
		if(side==MissleSide.FRIENDLY)
		{
			friendlyInstance=(FriendlySpaceship) beh;
			misTmp=getFreeThorpede(side);
		
			misTmp.gameObject.transform.position=friendlyInstance.MisslesLaunchPoint.transform.position;
			misTmp.gameObject.transform.eulerAngles=new Vector3(270,friendlyInstance.transform.eulerAngles.y,0);
			misTmp.Activate();
			misTmp.hideIcon();
			misTmp.gameObject.SetActive(true);
			activeFriendlyThorpedes.Add(misTmp);
		}
		if(side==MissleSide.ENEMY)
		{
			enemyInstance=(EnemySpaceship) beh;
			misTmp=getFreeThorpede(side);
			misTmp.gameObject.transform.position=enemyInstance.MissleLaunchPos.transform.position;
			misTmp.gameObject.transform.eulerAngles=new Vector3(270,enemyInstance.transform.eulerAngles.y,0);
			misTmp.Activate();
			misTmp.gameObject.SetActive(true);
			activeEnemyThorpeds.Add(misTmp);
		}
	}
	
	public void DestroyThorpede(MissleBehaviour mis)
	{
		if(mis.side==MissleSide.ENEMY)
			activeEnemyThorpeds.Remove(mis);
		else if(mis.side==MissleSide.FRIENDLY)
			activeFriendlyThorpedes.Remove(mis);
		mis.gameObject.transform.position=stackPos;
		mis.gameObject.SetActive(false);
		bringToBack(mis);
	}
	
	public void LaunchGas(MissleSide side, Object beh)
	{
		if(side==MissleSide.FRIENDLY)
		{
			friendlyInstance=(FriendlySpaceship) beh;
			misTmp=getFreeGas(side);
			misTmp.gameObject.transform.position=friendlyInstance.transform.position;
			misTmp.gameObject.transform.eulerAngles=new Vector3(270,friendlyInstance.transform.eulerAngles.y,0);
			misTmp.Activate();
			misTmp.gameObject.SetActive(true);
			activeFriendlyGasList.Add(misTmp);
		}
		if(side==MissleSide.ENEMY)
		{
			enemyInstance=(EnemySpaceship) beh;
			misTmp=getFreeGas(side);
			misTmp.gameObject.transform.position=enemyInstance.transform.position;
			misTmp.gameObject.transform.eulerAngles=new Vector3(270,enemyInstance.transform.eulerAngles.y,0);
			misTmp.Activate();
			misTmp.gameObject.SetActive(true);
			activeEnemyGasList.Add(misTmp);
		}
	}
	
	public void LaunchMine(MissleSide side, Object beh)
	{
		if(side==MissleSide.FRIENDLY)
		{
			friendlyInstance=(FriendlySpaceship) beh;
			misTmp=getFreeMine(side);
			misTmp.gameObject.transform.position=friendlyInstance.transform.position;
			misTmp.gameObject.transform.eulerAngles=new Vector3(270,friendlyInstance.transform.eulerAngles.y,0);
			misTmp.Activate();
			misTmp.gameObject.SetActive(true);
			activeFriendlyMinesList.Add(misTmp);
		}
		if(side==MissleSide.ENEMY)
		{
			enemyInstance=(EnemySpaceship) beh;
			misTmp=getFreeMine(side);
			misTmp.gameObject.transform.position=enemyInstance.transform.position;
			misTmp.gameObject.transform.eulerAngles=new Vector3(270,enemyInstance.transform.eulerAngles.y,0);
			misTmp.Activate();
			misTmp.gameObject.SetActive(true);
			activeEnemyMinesList.Add(misTmp);
		}
	}
	
	public void MarkToRemoveStaticMissle(MissleBehaviour mis)
	{
		if(mis.type==MissleType.GAS)
		{
			if(mis.side==MissleSide.ENEMY)
				enemyGasRemoveList.Add(mis);
			if(mis.side==MissleSide.FRIENDLY)
				friendlyGasRemoveList.Add(mis);
		}
	}
	
	public void DestroyMine(MissleBehaviour mis)
	{
		if(mis.side==MissleSide.ENEMY)
			activeEnemyMinesList.Remove(mis);
		else if(mis.side==MissleSide.FRIENDLY)
			activeFriendlyMinesList.Remove(mis);
		mis.gameObject.transform.position=stackPos;
		mis.gameObject.SetActive(false);
		bringToBack(mis);
	}
	
	public void DestroyGas(MissleBehaviour mis)
	{
		if(mis.side==MissleSide.ENEMY)
			activeEnemyGasList.Remove(mis);
		else if(mis.side==MissleSide.FRIENDLY)
			activeFriendlyGasList.Remove(mis);
		//mis.gameObject.transform.position=stackPos;
		mis.gameObject.SetActive(false);
		bringToBack(mis);
	}
}
