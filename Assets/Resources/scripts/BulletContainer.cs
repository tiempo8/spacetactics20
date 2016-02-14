using UnityEngine;
using System.Collections;

public class BulletContainer : MonoBehaviour {
	private float t=0;
	public Vector3 destination;
	private Vector3 startPoint;
	public int damage;
	public float speedModifier;
	public BulletType type;
	public float maxRange;
	private Vector2 distvec;
	public bool active=true;
	private bool inDeal=true;
	Vector2 tmp1;
	public float dispersion;
	public BulletSide side;
	private string enemyTag="enemy",friendlyTag="friendly";
	private float timeToDestroy=0;
	public GameObject gunParticle;
	public GameObject collisionParticle;
	public GameObject trailForMid;
	private float x,y,d=0;
	
	public void initialize()
	{
		gunParticle.SetActive(true);
		if(trailForMid!=null)
			trailForMid.SetActive(true);
		collisionParticle.SetActive(false);
		t=0;
		d=0;
		inDeal=true;
		timeToDestroy=0;
		active=true;
		startPoint=transform.position;
		tmp1.x=destination.x-startPoint.x;
		tmp1.y=destination.z-startPoint.z;
		tmp1/=tmp1.magnitude;
		tmp1*=maxRange;
		//gunParticle.GetComponent<ParticleSystem>().startLifetime=0;
		destination.Set(tmp1.x+startPoint.x+Random.Range(-dispersion,dispersion),destination.y,tmp1.y+startPoint.z+UnityEngine.Random.Range(-dispersion,dispersion));
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(side==BulletSide.FRIENDLY)
		{
			if(col.gameObject.tag==TagsStorage.ABIL_SHIELD)
				deactivate();
			
			if(col.gameObject.tag==TagsStorage.ENEMY_TAG)
			{
				col.gameObject.GetComponent<EnemySpaceship>().Attacked(damage);
				deactivate();
			}
		}
		
		if(side==BulletSide.ENEMY)
		{
			if(col.gameObject.tag==TagsStorage.FRIENDLY_TAG)
			{
				col.gameObject.GetComponent<FriendlySpaceship>().Attacked(damage);
				deactivate();
			}
			if(col.gameObject.tag==TagsStorage.ABIL_SHIELD)
				deactivate();
		}
		
		if(col.gameObject.tag==TagsStorage.ASTEROID_TAG)
			deactivate();
	}
	
	public void STEP()
	{
		if(inDeal)
		{
			t+=Time.deltaTime*speedModifier;
			x = (1-t)*startPoint.x+t*destination.x;
			y = (1-t)*startPoint.z+t*destination.z;
			transform.position=new Vector3(x,5,y);
			distvec.x=x-startPoint.x;
			distvec.y=y-startPoint.z;
			d=distvec.sqrMagnitude;
			if(d>=maxRange*maxRange)
				deactivate();
		}
		else
		{
			if(Time.time>=timeToDestroy)
				destroy();
		}
	}
	
	public void deactivate()
	{
		gunParticle.SetActive(false);
		if(trailForMid!=null)
			trailForMid.SetActive(false);
		collisionParticle.SetActive(true);
		timeToDestroy=Time.time+1.0f;
		inDeal=false;
		//gunParticle.GetComponent<ParticleSystem>().Stop();
	}
	
	public void destroy()
	{
		active=false;
	}
}
