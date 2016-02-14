using UnityEngine;
using System.Collections;

public class BulletPoolManager {
	private static BulletPoolManager instance=null;
	public static BulletPoolManager getInstance()
	{
		if(instance==null)
			instance=new BulletPoolManager();
		return instance;
	}
	
	private Vector3 bulletStartPosition = new Vector3(-1000,-1000,-1000);
	private GameObject tmp;
	private BulletContainer bullTmp;
	private ArrayList bulletSmallGunList,bulletMidGunList,bulletLargeGunList;
	private ArrayList activeBullets;
	private int i;
	private ArrayList activeBulletsToRemove;
	private const string enemyBulletTag="enemy_bullet",friendly_bullet="friendly_bullet";
	
	private BulletPoolManager()
	{
		bulletSmallGunList=new ArrayList();
		bulletMidGunList=new ArrayList();
		bulletLargeGunList=new ArrayList();
		
		activeBullets = new ArrayList();
		activeBulletsToRemove=new ArrayList();
	}
	
	public void initialize()
	{
		bulletSmallGunList.Clear();
		bulletMidGunList.Clear();
		bulletLargeGunList.Clear();
		
		activeBullets.Clear();
		activeBulletsToRemove.Clear();
	}
	
	public void createPoolObjs(BulletType type, int count)
	{
		i=0;
		if(type==BulletType.GUN_SMALL_BULLET)
		{
			for(i=0;i<count;i++)
			{
				tmp=(GameObject) GameObject.Instantiate(Resources.Load("prefab/bullets/smallGunBullet"),bulletStartPosition,Quaternion.identity);
				bringToBack(type,tmp.GetComponent<BulletContainer>());
			}
		}
		else if(type==BulletType.GUN_MID_BULLET)
		{
			for(i=0;i<count;i++)
			{
				tmp=(GameObject) GameObject.Instantiate(Resources.Load("prefab/bullets/midGunBullet"),bulletStartPosition,Quaternion.identity);
				bringToBack(type,tmp.GetComponent<BulletContainer>());
			}
		}
		else if(type==BulletType.GUN_LARGE_BULLET)
		{
			for(i=0;i<count;i++)
			{
				tmp=(GameObject) GameObject.Instantiate(Resources.Load("prefab/bullets/largeGunBullet"),bulletStartPosition,Quaternion.identity);
				bringToBack(type,tmp.GetComponent<BulletContainer>());
			}
		}
	}
	
	public void stepAllBullets()
	{
		foreach(BulletContainer bull in activeBullets)
		{
			bull.STEP();
			if(!bull.active)
				activeBulletsToRemove.Add(bull);
		}
		foreach(BulletContainer bull in activeBulletsToRemove)
		{
			activeBullets.Remove(bull);
			deactivateBullet(bull);
		}
		activeBulletsToRemove.Clear();
	}
	
	public void clearAllActiveBullets()
	{
		foreach(BulletContainer bull in activeBullets)
		{
			bull.active=false;
			deactivateBullet(bull);
		}
		activeBullets.Clear();
		activeBulletsToRemove.Clear();
	}
	
	private void bringToBack(BulletType type, BulletContainer bullet)
	{
		if(type==BulletType.GUN_SMALL_BULLET)
		{
			bullet.gameObject.SetActive(false);
			bullet.gameObject.transform.position=bulletStartPosition;
			bulletSmallGunList.Add(bullet);
		}
		else if(type==BulletType.GUN_MID_BULLET)
		{
			bullet.gameObject.SetActive(false);
			bullet.gameObject.transform.position=bulletStartPosition;
			bulletMidGunList.Add(bullet);
		}
		else if(type==BulletType.GUN_LARGE_BULLET)
		{
			bullet.gameObject.SetActive(false);
			bullet.gameObject.transform.position=bulletStartPosition;
			bulletLargeGunList.Add(bullet);
		}
	}
	
	private BulletContainer getFreeBullet(BulletType type)
	{
		if(type==BulletType.GUN_SMALL_BULLET)
		{
			foreach(BulletContainer bull in bulletSmallGunList)
			{
				bullTmp=bull;
				break;
			}
			bulletSmallGunList.Remove(bullTmp);
			return bullTmp;
		}
		else if(type==BulletType.GUN_MID_BULLET)
		{
			foreach(BulletContainer bull in bulletMidGunList)
			{
				bullTmp=bull;
				break;
			}
			bulletMidGunList.Remove(bullTmp);
			return bullTmp;
		}
		else if(type==BulletType.GUN_LARGE_BULLET)
		{
			foreach(BulletContainer bull in bulletLargeGunList)
			{
				bullTmp=bull;
				break;
			}
			bulletLargeGunList.Remove(bullTmp);
			return bullTmp;
		}
		return null;
	}
	
	private void addActiveBullet(BulletContainer bullet)
	{
		activeBullets.Add(bullet);
	}
	
	public void deactivateBullet(BulletContainer bullet)
	{
		bringToBack(bullet.type,bullet);
	}
	
	private Vector2 v2tmp1,v2tmp2;
	private float f1tmp,f2tmp;
	
	private float getBulletAngle(Vector3 start, Vector3 destination)
	{
		
		v2tmp1 = TagsStorage.oneVec;
		v2tmp2 = new Vector2(destination.x-start.x,destination.z-start.z);
		f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
		f2tmp = Vector2.Angle(v2tmp1,v2tmp2);
		if(f1tmp<=0)
			return f2tmp;
		else
			return (180-f2tmp)+180;
	}
	
	public void shotBullet(BulletSide side, Vector3 startPosition, Vector3 destination, int damage, float speed, BulletType type, float maxRange, float dispersion)
	{
		bullTmp=getFreeBullet(type);
		bullTmp.gameObject.transform.position=startPosition;
		bullTmp.destination=destination;
		bullTmp.damage=damage;
		bullTmp.transform.eulerAngles=new Vector3(bullTmp.transform.eulerAngles.x,getBulletAngle(startPosition,destination),bullTmp.transform.eulerAngles.z);
		bullTmp.speedModifier=speed;
		bullTmp.type=type;
		bullTmp.dispersion=dispersion;
		bullTmp.side=side;
		bullTmp.maxRange=maxRange;
		bullTmp.initialize();
		bullTmp.gameObject.SetActive(true);
		addActiveBullet(bullTmp);
	}
}
