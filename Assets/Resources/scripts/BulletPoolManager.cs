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
	private ArrayList bulletSmallGunList;
	private ArrayList activeBullets;
	private int i;
	private ArrayList activeBulletsToRemove;
	private const string enemyBulletTag="enemy_bullet",friendly_bullet="friendly_bullet";
	
	private BulletPoolManager()
	{
		bulletSmallGunList=new ArrayList();
		activeBullets = new ArrayList();
		activeBulletsToRemove=new ArrayList();
	}
	
	public void initialize()
	{
		bulletSmallGunList.Clear();
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
	
	public void shotBullet(BulletSide side, Vector3 startPosition, Vector3 destination, int damage, float speed, BulletType type, float maxRange, float dispersion)
	{
		
		bullTmp=getFreeBullet(type);
		bullTmp.gameObject.transform.position=startPosition;
		bullTmp.destination=destination;
		bullTmp.damage=damage;
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
