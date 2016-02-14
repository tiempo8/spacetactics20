using UnityEngine;
using System.Collections;

public class GameStorage {

	private static GameStorage instance = null;
	public static GameStorage getInstance()
	{
		if(instance==null)
			instance=new GameStorage();
		return instance;
	}
	
	private ArrayList friendlySpaceships,enemySpaceships;
	private ArrayList friendleSpaceShipsToRemove,enemySpaceshipsToRemove;
	private ArrayList friendlyExplodeList,enemyExplodeList;
	private ArrayList friendlyExplodeListToRemove,enemyExplodeListToRemove;
	private ArrayList rocketExplodeList,rocketExplodeListToRemove;
	private Object tmp;
	private float startTime,endTime;
	private float sin,mangle;
	private Vector2 v1,v2;
	private float d;
	private int curSelected=0;
	
	public GameStorage()
	{
		friendlySpaceships=new ArrayList();
		enemySpaceships=new ArrayList();
		friendleSpaceShipsToRemove=new ArrayList();
		enemySpaceshipsToRemove=new ArrayList();
		friendlyExplodeList=new ArrayList();
		enemyExplodeList=new ArrayList();
		enemyExplodeListToRemove=new ArrayList();
		friendlyExplodeListToRemove=new ArrayList();
		rocketExplodeList=new ArrayList();
		rocketExplodeListToRemove=new ArrayList();
	}
	
	public void clearStorage()
	{
		friendlySpaceships.Clear();
		enemySpaceships.Clear();
		friendleSpaceShipsToRemove.Clear();
		enemySpaceshipsToRemove.Clear();
		friendlyExplodeList.Clear();
		enemyExplodeList.Clear();
		enemyExplodeListToRemove.Clear();
		friendlyExplodeListToRemove.Clear();
		States.SummaryHp=0;
		curSelected=0;
	}
	
	public void processExplode()
	{
		if(enemyExplodeList.Count>0)
		{
			foreach(EnemySpaceship obj in enemyExplodeList)
			{
				if(obj.Explode())
					enemyExplodeListToRemove.Add(obj);
			}
			
			if(enemyExplodeListToRemove.Count>0)
			{
				foreach(EnemySpaceship obj in enemyExplodeListToRemove)
					enemyExplodeList.Remove(obj);
				enemyExplodeListToRemove.Clear();
			}
		}
		
		if(friendlyExplodeList.Count>0)
		{
			foreach(FriendlySpaceship obj in friendlyExplodeList)
			{
				if(obj.Explode())
					friendlyExplodeListToRemove.Add(obj);
			}
			
			if(friendlyExplodeListToRemove.Count>0)
			{
				foreach(FriendlySpaceship obj in friendlyExplodeListToRemove)
					friendlyExplodeList.Remove(obj);
				friendlyExplodeListToRemove.Clear();
			}
		}
		
		if(rocketExplodeList.Count>0)
		{
			foreach(MissleBehaviour obj in rocketExplodeList)
			{
				if(obj.Explode())
					rocketExplodeListToRemove.Add(obj);
			}
			
			if(rocketExplodeListToRemove.Count>0)
			{
				foreach(MissleBehaviour obj in rocketExplodeListToRemove)
				{
					rocketExplodeList.Remove(obj);
					if(obj.type==MissleType.ROCKET)
						MisslePoolManager.getInstance().DestroyRocket(obj);
					if(obj.type==MissleType.THORPEDE)
						MisslePoolManager.getInstance().DestroyThorpede(obj);
					if(obj.type==MissleType.MINES)
						MisslePoolManager.getInstance().DestroyMine(obj);
					if(obj.type==MissleType.GAS)
						MisslePoolManager.getInstance().DestroyGas(obj);
				}
				rocketExplodeListToRemove.Clear();
			}
		}
	}
	
	public void addToExplodeList(Object ob)
	{
		if(ob.GetType()==typeof(FriendlySpaceship))
			friendlyExplodeList.Add(ob);
		if(ob.GetType()==typeof(EnemySpaceship))
			enemyExplodeList.Add(ob);
		if(ob.GetType()==typeof(MissleBehaviour))
		{
			
			rocketExplodeList.Add(ob);
		}
	}
	
	public void addToList(Object ob)
	{
		if(ob.GetType()==typeof(FriendlySpaceship))
		{
			friendlySpaceships.Add(ob);
			States.SummaryHp+=Templates.getInstance().getPlaneTemplate(((FriendlySpaceship)ob).planeTemplateId).hp;
		}
		if(ob.GetType()==typeof(EnemySpaceship))
			enemySpaceships.Add(ob);
		
		checkZalpButtonState();
	}
	
	public void removeFromList(Object ob)
	{
		if(ob.GetType()==typeof(FriendlySpaceship))
			friendleSpaceShipsToRemove.Add(ob);
		if(ob.GetType()==typeof(EnemySpaceship))
			enemySpaceshipsToRemove.Add(ob);
	}
	
	public void launchStep()
	{
		startTime=Time.time;
		endTime=Time.time+3;
		toggleOnStepStart();
		States.WorldRunning=true;
	}
	
	public float getAngleDst(float fr, float to)
	{
		v1 = Quaternion.Euler(0,0,fr)*new Vector2(0,5);
		v2 = Quaternion.Euler(0,0,to)*new Vector2(0,5);
		
		sin = (v1.x*v2.y - v1.y*v2.x);
		mangle = fr-to;
		if(Mathf.Abs(mangle)>180)
			mangle=360-Mathf.Abs(mangle);
		
		if(sin>0)
			return -Mathf.Abs(mangle);
		else
			return Mathf.Abs(mangle);
	}
	
	public FriendlySpaceship getNearbyFriendly(EnemySpaceship attacker)
	{
		FriendlySpaceship ret=null;
		d=-1;
		v1.Set(attacker.gameObject.transform.position.x,attacker.gameObject.transform.position.z);
		foreach(FriendlySpaceship target in friendlySpaceships)
		{
			v2.Set(target.gameObject.transform.position.x,target.gameObject.transform.position.z);
			if(d<0)
			{
				ret=target;
				v2.Set(v1.x-v2.x,v1.y-v2.y);
				d = v2.sqrMagnitude;
			}
			else
			{
				v2.Set(v1.x-v2.x,v1.y-v2.y);
				if(d>v2.sqrMagnitude)
				{
					ret=target;
					d=v2.sqrMagnitude;
				}
			}
		}
		return ret;
	}
	
	public FriendlySpaceship getNearbyFriendly(MissleBehaviour attacker)
	{
		FriendlySpaceship ret=null;
		d=-1;
		v1.Set(attacker.gameObject.transform.position.x,attacker.gameObject.transform.position.z);
		foreach(FriendlySpaceship target in friendlySpaceships)
		{
			v2.Set(target.gameObject.transform.position.x,target.gameObject.transform.position.z);
			if(d<0)
			{
				ret=target;
				v2.Set(v1.x-v2.x,v1.y-v2.y);
				d = v2.sqrMagnitude;
			}
			else
			{
				v2.Set(v1.x-v2.x,v1.y-v2.y);
				if(d>v2.sqrMagnitude)
				{
					ret=target;
					d=v2.sqrMagnitude;
				}
			}
		}
		return ret;
	}
	
	public float getAngleRelative(GameObject a, GameObject b)
	{
		v1 = TagsStorage.oneVec;
		v2 = new Vector2(b.transform.position.x-a.transform.position.x,b.transform.position.z-a.transform.position.z);
		sin = (v1.x*v2.y - v1.y*v2.x);
		mangle = Vector2.Angle(v1,v2);
		if(sin<=0)
			return mangle;
		else
			return (180-mangle)+180;
	}
	
	public float getAngleRelative(Vector2 a, Vector2 b)
	{
		v1 = TagsStorage.oneVec;
		v2 = new Vector2(b.x-a.x,b.y-a.y);
		sin = (v1.x*v2.y - v1.y*v2.x);
		mangle = Vector2.Angle(v1,v2);
		if(sin<=0)
			return mangle;
		else
			return (180-mangle)+180;
	}
	
	public EnemySpaceship getEnemyInFireZone(FriendlySpaceship friendlyShuttle, Templates.GunOnShuttle gun)
	{
		EnemySpaceship ret = null;
		float dist=0;
		float mindist=-1;
		Templates.GunTemplate gunTemp = Templates.getInstance().getGunTemplate(gun.gunId);
		Vector2 gunPos = new Vector2(friendlyShuttle.gameObject.transform.position.x+gun.pos.x,friendlyShuttle.gameObject.transform.position.z+gun.pos.y);
		float gunAngle = Mathf.Repeat(friendlyShuttle.transform.eulerAngles.y+gun.turnAngle,360);
		Vector2 pos2;
		foreach(EnemySpaceship enemy in enemySpaceships)
		{
			if(enemy.isAlive())
			{
				pos2=new Vector2(enemy.gameObject.transform.position.x,enemy.gameObject.transform.position.z);
				if((dist=Vector2.Distance(gunPos,pos2))<=gunTemp.attackRange && Mathf.Abs(getAngleDst(gunAngle,getAngleRelative(friendlyShuttle.gameObject,enemy.gameObject)))<=gunTemp.attackAngle)
				{
					if(mindist<0)
					{
						ret=enemy;
						mindist=dist;
					}
					else
					{
						if(dist<mindist)
						{
							ret=enemy;
							mindist=dist;
						}
					}
				}
			}
		}
		return ret;
	}
	
	public FriendlySpaceship getFriendlyInFireZone(EnemySpaceship friendlyShuttle, Templates.GunOnShuttle gun)
	{
		FriendlySpaceship ret = null;
		float dist=0;
		float mindist=-1;
		Templates.GunTemplate gunTemp = Templates.getInstance().getGunTemplate(gun.gunId);
		Vector2 gunPos = new Vector2(friendlyShuttle.gameObject.transform.position.x+gun.pos.x,friendlyShuttle.gameObject.transform.position.z+gun.pos.y);
		float gunAngle = Mathf.Repeat(friendlyShuttle.transform.eulerAngles.y+gun.turnAngle,360);
		Vector2 pos2;
		foreach(FriendlySpaceship enemy in friendlySpaceships)
		{
			if(enemy.isAlive())
			{
				pos2=new Vector2(enemy.gameObject.transform.position.x,enemy.gameObject.transform.position.z);
				if((dist=Vector2.Distance(gunPos,pos2))<=gunTemp.attackRange && Mathf.Abs(getAngleDst(gunAngle,getAngleRelative(friendlyShuttle.gameObject,enemy.gameObject)))<=gunTemp.attackAngle)
				{
					if(mindist<0)
					{
						ret=enemy;
						mindist=dist;
					}
					else
					{
						if(dist<mindist)
						{
							ret=enemy;
							mindist=dist;
						}
					}
				}
			}
		}
		return ret;
	}
	
	private int CalculateRemainHp()
	{
		int res=0;
		foreach(FriendlySpaceship obj in friendlySpaceships)
			res+=obj.getHP();
		return res;
	}
	
	
	
	private void CalculateWinLose()
	{
		if(friendlySpaceships.Count>0)
			States.winloseResult=States.WinLoseSettings.getStars((int)(((float) CalculateRemainHp() / (float) States.SummaryHp)*100));
		else if(enemySpaceships.Count>0)
			States.winloseResult=-1;
		else
			States.winloseResult=0;
		States.winLoseController.ShowWinlose();
	}
	
	public void setRocketsAndThorpeds()
	{
		foreach(FriendlySpaceship obj in friendlySpaceships)
			obj.setRocketsOrThorpeds();
	}
	
	public float getStartTime()
	{
		return startTime;
	}
	
	public float getEndTime()
	{
		return endTime;
	}
	
	public void checkZalpButtonState()
	{
		bool res=false;
		foreach(FriendlySpaceship obj in friendlySpaceships)
		{
			if(obj.haveAbilRocketsOrThorpeds())
			{
				res=true;
				break;
			}
		}
		GamePhaseController.setZalpButtonActive(res);
	}
	
	public void AccelerateAllUnits()
	{
		foreach(EnemySpaceship obj in enemySpaceships)
			obj.STEP();
		foreach(FriendlySpaceship obj in friendlySpaceships)
			obj.STEP();
	}
	
	public FriendlySpaceship getNextFriendlyToFocus()
	{
		curSelected++;
		curSelected=curSelected%friendlySpaceships.Count;
		return (FriendlySpaceship) friendlySpaceships[curSelected];
	}
	
	public FriendlySpaceship getPrevFriendlyToFocus()
	{
		curSelected--;
		if(curSelected<0)
			curSelected+=friendlySpaceships.Count;
		return (FriendlySpaceship) friendlySpaceships[curSelected];
	}
	
	private void clearRemoveLists()
	{
		foreach(FriendlySpaceship obj in friendleSpaceShipsToRemove)
			friendlySpaceships.Remove(obj);
		
		foreach(EnemySpaceship obj in enemySpaceshipsToRemove)
			enemySpaceships.Remove(obj);
		
		enemySpaceshipsToRemove.Clear();
		friendleSpaceShipsToRemove.Clear();
	}
	
	public void loadLevel(int level)
	{
		States.gamePhaseGuiEnabled=true;
		States.inPauseMenu=false;
		clearStorage();
		BulletPoolManager.getInstance().initialize();
		MisslePoolManager.getInstance().initialize();
		UnityEngine.SceneManagement.SceneManager.LoadScene(Templates.getInstance().getLevel((int)States.currentCampaign.levels[level]).file);
	}
	
	//EVENTS
	public void toggleOnStepStart()
	{
		foreach(FriendlySpaceship obj in friendlySpaceships)
		{
			obj.onStepStart();
			obj.hideAttackIcon();
		}
		foreach(EnemySpaceship obj in enemySpaceships)
			obj.onStepStart();
		
		MisslePoolManager.getInstance().ToggleOnStepStart();
	}
	
	public void toggleOnStepEnd()
	{
		BulletPoolManager.getInstance().clearAllActiveBullets();
		foreach(FriendlySpaceship obj in friendlySpaceships)
		{
			obj.onStepEnd();
			obj.showAttackIcon();
		}
		foreach(EnemySpaceship obj in enemySpaceships)
			obj.onStepEnd();
		
		clearRemoveLists();
		if(friendlySpaceships.Count==0 || enemySpaceships.Count==0)
			CalculateWinLose();
		
		MisslePoolManager.getInstance().ToggleOnStepEnd();
		
		checkZalpButtonState();
	}
	
	public void toggleOnZoomChangedEvent()
	{
		foreach(FriendlySpaceship obj in friendlySpaceships)
			obj.onZoomChanged();
		MisslePoolManager.getInstance().ToggleOnZoomChanged();
	}
	
	public void toggleHideAllAttackIcons(FriendlySpaceship exclude)
	{
		foreach(FriendlySpaceship obj in friendlySpaceships)
			if(obj!=exclude)
				obj.hideAttackIcon();
		MisslePoolManager.getInstance().ToggleHideAllAttackIcons(null);
	}
	
	public void toggleHideAllAttackIcons(MissleBehaviour exclude)
	{
		foreach(FriendlySpaceship obj in friendlySpaceships)
			obj.hideAttackIcon();
		MisslePoolManager.getInstance().ToggleHideAllAttackIcons(exclude);
	}
	
	public void toggleShowAllAttackIcons(FriendlySpaceship exclude)
	{
		foreach(FriendlySpaceship obj in friendlySpaceships)
			if(obj!=exclude)
				obj.showAttackIcon();
		MisslePoolManager.getInstance().ToggleShowAllAttackIcons(null);
	}
	
	public void toggleShowAllAttackIcons(MissleBehaviour exclude)
	{
		foreach(FriendlySpaceship obj in friendlySpaceships)
			obj.showAttackIcon();
		MisslePoolManager.getInstance().ToggleShowAllAttackIcons(exclude);
	}
}
