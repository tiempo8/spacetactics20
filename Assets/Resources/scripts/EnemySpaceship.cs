using UnityEngine;
using System.Collections;

public class EnemySpaceship : MonoBehaviour {

	public GameObject attackIcon;
	public GameObject explosionParticle;
	public Templates.PlaneTemplates planeTemplateId;
	private ParticleSystem trailPart1,trailPart2;
	private Templates.PlaneTemplate planeTemplate;
	private Vector3 tmpVec;
	private Vector2 v2tmp1,v2tmp2;
	private float f1tmp,f2tmp,f3tmp,angtmp;
	private Vector2 point1,point2,point3,point4,pointz,f;
	float x,y,tt,t=0;
	private MeshRenderer bodyMeshRenderer;
	private Vector2 oneVec = new Vector2(0,5);
	const float step = 0.02f;
	int i=0;
	private Templates.GunTemplate gunTemp;
	private FriendlySpaceship enemy;
	private Vector2 targetPosition;
	private Collider bodyCollider;
	
	private FriendlySpaceship target;
	
	private Vector3 forUpdatePositionNORMAL;
	
	
	//ACTIVE CONSTRAINTS
	float maxAngle=0f;
	float maxLen=0f;
	float minLen=0f;
	int hp;
	
	private float between,newAngle;
	private int preferencedDirection=0;
	
	//STATES
	private bool exploded=false;
	private float explodedDeltaTime=0;
	
	private float fortestAngle=0;
	
	float d;
	
	public void onSelected()
	{
		Debug.Log("ENEMY HP: "+hp);
	}
	
	public void onDeselected()
	{
	}
	
	public void onStepStart()
	{
		UpdateCalc();
	}
	
	public void onStepEnd()
	{
		t=0;
		//UpdateCalc();
	}
		
	public void STEP()
	{
		if(!exploded)
		{
			foreach(Templates.GunOnShuttle gun in planeTemplate.guns)
			{
				gunTemp=Templates.getInstance().getGunTemplate(gun.gunId);
				if(!gun.ready)
					if(gun.shotTime+gunTemp.reuse<Time.time)
						gun.ready=true;
				
				enemy=GameStorage.getInstance().getFriendlyInFireZone(this,gun);
				if(gun.ready && enemy!=null)
				{
					f = Quaternion.Euler(0,0,-gameObject.transform.eulerAngles.y)*gun.pos;
					BulletPoolManager.getInstance().shotBullet(BulletSide.ENEMY,new Vector3(transform.position.x+f.x,5,transform.position.z+f.y),enemy.transform.position,gunTemp.damage,gunTemp.bulletSpeed,BulletType.GUN_SMALL_BULLET,gunTemp.attackRange,gunTemp.bulletDispersion);
					gun.ready=false;
					gun.shotTime=Time.time;
				}
			}
			
			t+=1f/151f;
			x=(1-t)*(1-t)*(1-t)*point1.x+3*(1-t)*(1-t)*t*point2.x+3*(1-t)*t*t*point3.x+t*t*t*point4.x;
			y=(1-t)*(1-t)*(1-t)*point1.y+3*(1-t)*(1-t)*t*point2.y+3*(1-t)*t*t*point3.y+t*t*t*point4.y;
			tmpVec.x=x;
			tmpVec.y=5;
			tmpVec.z=y;
			
			v2tmp1.x=x-transform.position.x;
			v2tmp1.y=y-transform.position.z;
			v2tmp2 = new Vector2(0,5);
			f1tmp = (v2tmp2.x*v2tmp1.y - v2tmp2.y*v2tmp1.x);
			f2tmp = Vector2.Angle(v2tmp2,v2tmp1);
			if(f1tmp>0)
				f2tmp=(180-f2tmp)+180;
			transform.eulerAngles=new Vector3(270,f2tmp,0);
			fortestAngle=f2tmp;
			
			transform.position=tmpVec;
		}
	}
	
	private float getAttackAngle()
	{
		
		v2tmp1 = new Vector2(0,5);
		v2tmp2 = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x-transform.position.x,Camera.main.ScreenToWorldPoint(Input.mousePosition).z-transform.position.z);
		f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
		f2tmp = Vector2.Angle(v2tmp1,v2tmp2);
		if(f1tmp<=0)
			return f2tmp;
		else
			return (180-f2tmp)+180;
	}
	
	private void calculatePreferencedDirection()
	{
		int gunsl=0,gunsr=0;
		foreach(Templates.GunOnShuttle goss in planeTemplate.guns)
		{
			if(goss.pos.x<0) gunsl++;
			if(goss.pos.x>0) gunsr++;
		}
		if(gunsr>gunsl) preferencedDirection=1;
		if(gunsl>gunsr) preferencedDirection=-1;
	}
	
	private float getAttackIconAngle()
	{
		
		v2tmp1 = new Vector2(0,5);
		v2tmp2 = new Vector2(attackIcon.transform.position.x-transform.position.x,attackIcon.transform.position.z-transform.position.z);
		f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
		f2tmp = Vector2.Angle(v2tmp1,v2tmp2);
		if(f1tmp<=0)
			return f2tmp;
		else
			return (180-f2tmp)+180;
	}
	
	private void UpdateCalc()
	{
		CalculatePath();
		CalculatePath();
	}
	
	private void CalculatePath()
	{
		target=GameStorage.getInstance().getNearbyFriendly(this);
		if(target!=null)
		{
			v2tmp1 = new Vector2(target.transform.position.x-transform.position.x,target.transform.position.z-transform.position.z);
			v2tmp2 = oneVec;
			f1tmp = (v2tmp2.x*v2tmp1.y - v2tmp2.y*v2tmp1.x); // sinphi
			f2tmp = Vector2.Angle(v2tmp1,v2tmp2); // mangle
			if(f1tmp>=0)
				f2tmp=(180-f2tmp)+180;
			
			between = GameStorage.getInstance().getAngleDst(transform.eulerAngles.y,f2tmp);
			
			
			if(Mathf.Abs(between)>maxAngle)
			{
				if(preferencedDirection==-1)
					newAngle=Mathf.Repeat(transform.eulerAngles.y-maxAngle,360);
				else if(preferencedDirection==1)
					newAngle=Mathf.Repeat(transform.eulerAngles.y+maxAngle,360);
				else
				{
					if(between>0)
						newAngle=Mathf.Repeat(transform.eulerAngles.y-maxAngle,360);
					else
						newAngle=Mathf.Repeat(transform.eulerAngles.y+maxAngle,360);
				}
			}
			else
				newAngle=Mathf.Repeat(transform.eulerAngles.y-between,360);
			
			targetPosition=Quaternion.Euler(0,0,-newAngle)*new Vector2(0,Random.Range(minLen,maxLen));
			attackIcon.transform.position=new Vector3(targetPosition.x+transform.position.x,5,targetPosition.y+transform.position.z);
		}
	
		
		point1=new Vector2(transform.position.x,transform.position.z);
		point2=Quaternion.Euler(0,0,-transform.eulerAngles.y)*new Vector2(0,minLen*Mathf.Abs(getAngleDst(transform.eulerAngles.y,getAttackIconAngle())/maxAngle)*Vector2.Distance(point1,point4)/maxLen)*planeTemplate.lowerSmooth;
		point2+=point1;
		point4=new Vector2(attackIcon.transform.position.x,attackIcon.transform.position.z);
		pointz = new Vector2(point4.x-point2.x,point4.y-point2.y)/2;
		point3 = new Vector2(pointz.y,-pointz.x)*getAngleDst(transform.eulerAngles.y,getAttackIconAngle())/maxAngle*Vector2.Distance(point1,point4)/maxLen*planeTemplate.upperSmooth;
		point3 = point3+point2+pointz;	
	}
	
	private float getAngleDst(float fr, float to)
	{
		v2tmp1 = Quaternion.Euler(0,0,fr)*new Vector2(0,5);
		v2tmp2 = Quaternion.Euler(0,0,to)*new Vector2(0,5);
		
		f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
		f2tmp = fr-to;
		if(Mathf.Abs(f2tmp)>180)
			f2tmp=360-Mathf.Abs(f2tmp);
		
		if(f1tmp>0)
			return -Mathf.Abs(f2tmp);
		else
			return Mathf.Abs(f2tmp);
	}
	
	public bool Explode()
	{
		if(exploded)
		{
			explodedDeltaTime+=Time.deltaTime;
			if(explodedDeltaTime>=1)
				bodyMeshRenderer.enabled=false;
			if(explodedDeltaTime>=3)
			{
				gameObject.SetActive(false);
				return true;
			}
		}
		return false;
	}
	
	public void Attacked(int damage)
	{
		hp-=damage;
		if(hp<=0)
		{
			Destroyed();
		}
	}
	
	private void Destroyed()
	{
		explosionParticle.SetActive(true);
		bodyCollider.enabled=false;
		exploded=true;
		GameStorage.getInstance().removeFromList(this);
		GameStorage.getInstance().addToExplodeList(this);
	}
	
	public bool isAlive()
	{
		return hp>0;
	}
		
	private void fillConstraints()
	{		
		maxLen=planeTemplate.maxRange;
		minLen=planeTemplate.minRange;
		maxAngle=planeTemplate.maxTurnAngle;
		forUpdatePositionNORMAL=new Vector3(0,-maxLen,0);
		hp=planeTemplate.hp;
	}
	
	void Start () {
		BulletPoolManager.getInstance().createPoolObjs(BulletType.GUN_SMALL_BULLET,15);
		planeTemplate=Templates.getInstance().getPlaneTemplate(planeTemplateId);
		calculatePreferencedDirection();
		GameStorage.getInstance().addToList(this);
		bodyMeshRenderer=GetComponent<MeshRenderer>();
		bodyCollider=GetComponent<Collider>();
		explosionParticle.SetActive(false);
		fillConstraints();
	}
}