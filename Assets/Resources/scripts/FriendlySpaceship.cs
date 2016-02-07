using UnityEngine;
using System.Collections;

public class FriendlySpaceship : MonoBehaviour {

	public GameObject attackIcon;
	public GameObject explosionParticle;
	public Templates.PlaneTemplates planeTemplateId;
	private ParticleSystem trailPart1,trailPart2;
	private Templates.PlaneTemplate planeTemplate;
	private SpriteRenderer attackIconMeshRenderer;
	private LineRenderer lineRenderer;
	private Vector3 tmpVec;
	private Vector2 v2tmp1,v2tmp2;
	private float f1tmp,f2tmp,f3tmp,angtmp;
	private short sh1tmp;
	private Vector2 point1,point2,point3,point4,pointz,f;
	float x,y,tt,t=0;
	private float attackAngleMov=0f;
	private Vector2 oneVec = new Vector2(0,5);
	const float step = 0.02f;
	int i=0;
	private Templates.GunTemplate gunTemp;
	private EnemySpaceship enemy;
	private int hp;
	private Collider bodyCollider;
	private MeshRenderer bodyMeshRenderer;
	private int abilRandonRotateDirection=1;
	private bool rocketLaunched=false;
	
	
	//guns arcs
	private GameObject gunTmp;
	private Vector3[] vertices = new Vector3[101];
	private Mesh m;
	private Templates.GunTemplate gt;
	private ArrayList arcObjs;
	
	private Vector3 forUpdatePositionNORMAL;
	private Vector3 forAbilities=new Vector3(270,0,0);
	private Vector3 forActiveAbil=new Vector3(90,0,0);
	
	//ACTIVE CONSTRAINTS
	float maxAngle=0f;
	float maxLen=0f;
	float minLen=0f;
	private bool exploded=false;
	private float explodedDeltaTime=0;
	
	private Abilities.AbilityType activeAbil=Abilities.AbilityType.none;
	private int selectedAbilPosition=0;
	private bool abilityInReuse=false;

	public GameObject[] abilitiesGameObjects;
	
	
	private float fortestAngle=0;
	
	float d;
	
	public void onSelected()
	{
		foreach(GameObject obj in arcObjs)
			obj.SetActive(true);
		Debug.Log("FRIENDLY HP: "+hp);
		Debug.Log("Active abil: "+activeAbil);
		for(i=0;i<planeTemplate.abilities.Count;i++)
			abilitiesGameObjects[i+1].SetActive(true);
	}
	
	public void onDeselected()
	{
		foreach(GameObject obj in arcObjs)
			obj.SetActive(false);
		for(i=0;i<planeTemplate.abilities.Count;i++)
			abilitiesGameObjects[i+1].SetActive(false);
	}
	
	public void onStepStart()
	{
		//Debug.Log("ON START");
	}
	
	public void onAbilitySwitched(int abilNumber)
	{
		if(abilNumber!=-1)
		{
			if(activeAbil==Abilities.AbilityType.none)
			{
				abilitiesGameObjects[0].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon((Abilities.AbilityType)planeTemplate.abilities[abilNumber-1],true);
				abilitiesGameObjects[abilNumber].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon(Abilities.AbilityType.none,false);
				selectedAbilPosition=abilNumber;
				activeAbil=(Abilities.AbilityType) planeTemplate.abilities[abilNumber-1];
				
				abilitiesGameObjects[0].transform.eulerAngles=forActiveAbil;
			}
			else
			{
				if(activeAbil==(Abilities.AbilityType)planeTemplate.abilities[abilNumber-1])
				{
					abilitiesGameObjects[0].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon(Abilities.AbilityType.none,true);
					abilitiesGameObjects[abilNumber].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon(activeAbil,false);
					selectedAbilPosition=0;
					activeAbil=Abilities.AbilityType.none;
					CalculatePath();
				}
				else
				{
					abilitiesGameObjects[0].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon((Abilities.AbilityType)planeTemplate.abilities[abilNumber-1],true);
					abilitiesGameObjects[selectedAbilPosition].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon(activeAbil,false);
					abilitiesGameObjects[abilNumber].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon(Abilities.AbilityType.none,false);
					activeAbil=(Abilities.AbilityType) planeTemplate.abilities[abilNumber-1];
					selectedAbilPosition=abilNumber;
				}
			}
		}
		
		if(activeAbil==Abilities.AbilityType.none || activeAbil==Abilities.AbilityType.gas || activeAbil==Abilities.AbilityType.homingMissle || activeAbil==Abilities.AbilityType.homingThorpede || activeAbil==Abilities.AbilityType.mines || activeAbil==Abilities.AbilityType.shield)
		{
			maxLen=planeTemplate.maxRange;
			minLen=planeTemplate.minRange;
			maxAngle=planeTemplate.maxTurnAngle;
		}
		
		if(activeAbil==Abilities.AbilityType.halfRoundTurn)
		{
			maxLen=planeTemplate.minRange;
			minLen=planeTemplate.minRange;
			maxAngle=0.0f;
			abilRandonRotateDirection=Random.Range(0,2);
			if(abilRandonRotateDirection==0) abilRandonRotateDirection=-1;
			//Debug.Log("RA: "+abilRandonRotateDirection);
		}
		
		if(activeAbil==Abilities.AbilityType.turnAround)
		{
			maxLen=0.7f*planeTemplate.maxRange;
			minLen=0.7f*planeTemplate.maxRange;
			maxAngle=planeTemplate.maxTurnAngle;
			abilRandonRotateDirection=Random.Range(0,2);
			if(abilRandonRotateDirection==0) abilRandonRotateDirection=-1;
		}
		
		if(activeAbil==Abilities.AbilityType.doubleThrottle)
		{
			maxLen=2*planeTemplate.maxRange;
			minLen=2*planeTemplate.minRange;
			maxAngle=planeTemplate.maxTurnAngle;
		}
		
		attackIcon.transform.localPosition=new Vector3(0,-maxLen,0);
		CalculatePath();
		if(activeAbil!=Abilities.AbilityType.none)
			abilitiesGameObjects[0].transform.eulerAngles=forActiveAbil;
		abilitiesGameObjects[1].transform.eulerAngles=forAbilities;
		abilitiesGameObjects[2].transform.eulerAngles=forAbilities;
		abilitiesGameObjects[3].transform.eulerAngles=forAbilities;
		abilitiesGameObjects[4].transform.eulerAngles=forAbilities;
	}
	
	private void reloadIcons()
	{
		if(activeAbil!=Abilities.AbilityType.none)
			abilityInReuse=true;
		else if(activeAbil==Abilities.AbilityType.none && abilityInReuse)
			abilityInReuse=false;
		
		
		abilitiesGameObjects[0].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon(Abilities.AbilityType.none,true);
		for(i=0;i<planeTemplate.abilities.Count;i++)
		{
			if(abilityInReuse)
			{
				abilitiesGameObjects[i+1].tag="abil_reused";
				abilitiesGameObjects[i+1].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIconGrey((Abilities.AbilityType)planeTemplate.abilities[i],false);
			}
			else
			{
				abilitiesGameObjects[i+1].tag="abil_"+(i+1);
				abilitiesGameObjects[i+1].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon((Abilities.AbilityType)planeTemplate.abilities[i],false);
			}
		}
		selectedAbilPosition=0;
		activeAbil=Abilities.AbilityType.none;
		CalculatePath();
	}
	
	public void onStepEnd()
	{
		t=0;
		updateAttackIconPosition();
		reloadIcons();
		maxLen=planeTemplate.maxRange;
		minLen=planeTemplate.minRange;
		maxAngle=planeTemplate.maxTurnAngle;
		CalculatePath();
		rocketLaunched=false;
	}
	
	public void hideAttackIcon()
	{
		attackIcon.SetActive(false);
		lineRenderer.SetWidth(0,0);
	}
	
	public void showAttackIcon()
	{
		attackIcon.SetActive(true);
		lineRenderer.SetWidth(0.1f,0.2f);
	}
	
	public void STEP()
	{
		if(!exploded)
		{
			if(!Abilities.getLockGun(activeAbil))
			{
				foreach(Templates.GunOnShuttle gun in planeTemplate.guns)
				{
					gunTemp=Templates.getInstance().getGunTemplate(gun.gunId);
					if(!gun.ready)
						if(gun.shotTime+gunTemp.reuse<Time.time)
							gun.ready=true;
					
					enemy=GameStorage.getInstance().getEnemyInFireZone(this,gun);
					if(gun.ready && enemy!=null)
					{
						//SHOT PROCEDURE WITH POOLING
						f = Quaternion.Euler(0,0,-gameObject.transform.eulerAngles.y)*gun.pos;
						BulletPoolManager.getInstance().shotBullet(BulletSide.FRIENDLY,new Vector3(transform.position.x+f.x,5,transform.position.z+f.y),enemy.transform.position,gunTemp.damage,gunTemp.bulletSpeed,BulletType.GUN_SMALL_BULLET,gunTemp.attackRange,gunTemp.bulletDispersion);
						gun.ready=false;
						gun.shotTime=Time.time;
					}
				}
			}
			
			t+=1f/151f;
			x=(1-t)*(1-t)*(1-t)*point1.x+3*(1-t)*(1-t)*t*point2.x+3*(1-t)*t*t*point3.x+t*t*t*point4.x;
			y=(1-t)*(1-t)*(1-t)*point1.y+3*(1-t)*(1-t)*t*point2.y+3*(1-t)*t*t*point3.y+t*t*t*point4.y;
			tmpVec.x=x;
			tmpVec.y=5;
			tmpVec.z=y;
			
			if(activeAbil==Abilities.AbilityType.halfRoundTurn)
				f2tmp=transform.eulerAngles.y+abilRandonRotateDirection*180*Time.fixedDeltaTime/3;
			else if(activeAbil==Abilities.AbilityType.turnAround)
				f2tmp=transform.eulerAngles.y+abilRandonRotateDirection*360*Time.fixedDeltaTime/3;
			else
			{
				v2tmp1.x=x-transform.position.x;
				v2tmp1.y=y-transform.position.z;
				v2tmp2 = new Vector2(0,5);
				f1tmp = (v2tmp2.x*v2tmp1.y - v2tmp2.y*v2tmp1.x);
				f2tmp = Vector2.Angle(v2tmp2,v2tmp1);
				if(f1tmp>0)
					f2tmp=(180-f2tmp)+180;
			}
			
			if(activeAbil==Abilities.AbilityType.homingMissle && t>=0.5f && !rocketLaunched)
			{
				MisslePoolManager.getInstance().LaunchRocket(MissleSide.FRIENDLY,this);
				rocketLaunched=true;
			}
			else if(activeAbil==Abilities.AbilityType.homingThorpede && t>=0.5f && !rocketLaunched)
			{
				MisslePoolManager.getInstance().LaunchThorpede(MissleSide.FRIENDLY,this);
				rocketLaunched=true;
			}
			
			transform.eulerAngles=new Vector3(270,f2tmp,0);
			//Debug.Log("ALLY: "+f2tmp);
			fortestAngle=f2tmp;
			
			
			transform.position=tmpVec;
		}
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
	
	public bool isAlive()
	{
		return hp>0;
	}
	
	private void updateAttackIconPosition()
	{
		attackIcon.transform.localPosition=forUpdatePositionNORMAL;
		CalculatePath();
		CalculatePath();
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
	
	private void CalculatePath()
	{
		point1=new Vector2(transform.position.x,transform.position.z);
		point2=Quaternion.Euler(0,0,-transform.eulerAngles.y)*new Vector2(0,minLen*Mathf.Abs(getAngleDst(transform.eulerAngles.y,getAttackIconAngle())/planeTemplate.maxTurnAngle)*Vector2.Distance(point1,point4)/maxLen)*planeTemplate.lowerSmooth;
		point2+=point1;
		point4=new Vector2(attackIcon.transform.position.x,attackIcon.transform.position.z);
		pointz = new Vector2(point4.x-point2.x,point4.y-point2.y)/2;
		point3 = new Vector2(pointz.y,-pointz.x)*getAngleDst(transform.eulerAngles.y,getAttackIconAngle())/planeTemplate.maxTurnAngle*Vector2.Distance(point1,point4)/maxLen*planeTemplate.upperSmooth;
		point3 = point3+point2+pointz;
		
		i=0;
		for(tt=0f;tt<=1;tt+=step)
		{
			x = Mathf.Pow((1-tt),3)*point1.x+3*(1-tt)*(1-tt)*tt*point2.x+3*(1-tt)*tt*tt*point3.x+tt*tt*tt*point4.x;
			y = Mathf.Pow((1-tt),3)*point1.y+3*(1-tt)*(1-tt)*tt*point2.y+3*(1-tt)*tt*tt*point3.y+tt*tt*tt*point4.y;
			lineRenderer.SetPosition(i,new Vector3(x,0,y));
			if(i==49)
			{
				v2tmp2.x=x;
				v2tmp2.y=y;
			}
			else if(i==50)
			{
				
				v2tmp2.x=x-v2tmp2.x;
				v2tmp2.y=y-v2tmp2.y;
				v2tmp1 = oneVec;
				f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
				f2tmp = Vector2.Angle(v2tmp1,v2tmp2);
				if(f1tmp<=0)
					attackAngleMov= f2tmp;
				else
					attackAngleMov = (180-f2tmp)+180;
				attackIcon.transform.localEulerAngles=new Vector3(0,180,-attackAngleMov+270+transform.eulerAngles.y);
				if(activeAbil==Abilities.AbilityType.none)
					abilitiesGameObjects[0].transform.eulerAngles=new Vector3(90,attackAngleMov+270,0);
			}
			
			i++;
		}		
	}
	
	public void onAttackIconCapture()
	{
		//attackIconMeshRenderer.sprite=Templates.AttackIconState.pressState;
	}
	
	/*
	 * Return -1 if attackIcon is not in angle bounds from left, 1 if from right, 0 is in bounds. 
	 */
	
	private short attackIconOutOfBounds(float ang)
	{
		if(ang>=maxAngle)
			return (short)-1;
		else if(ang<=-maxAngle)
			return (short)1;
		else
			return (short)0;
	}
	
	public bool onAttackIconMoved(Vector3 validation)
	{
		tmpVec.x=validation.x;
		tmpVec.y=5;
		tmpVec.z=validation.z;
		d=(tmpVec-transform.position).sqrMagnitude;
		angtmp=getAttackAngle();
		f3tmp=getAngleDst(transform.eulerAngles.y,angtmp);
		sh1tmp=attackIconOutOfBounds(f3tmp);
		if(Mathf.Abs(f3tmp)>135) return false;
		
		if(d<minLen*minLen)
		{
			if(sh1tmp==1)
				tmpVec = Quaternion.Euler(0,0,maxAngle)*new Vector3(0,-minLen,0);	
			else if(sh1tmp==-1)
				tmpVec = Quaternion.Euler(0,0,-maxAngle)*new Vector3(0,-minLen,0);
			else
				tmpVec=Quaternion.Euler(0,0,-f3tmp)*new Vector3(0,-minLen,0);
			
			attackIcon.transform.localPosition=tmpVec;
		}
		else if(d>maxLen*maxLen)
		{
			if(sh1tmp==1)
				tmpVec = Quaternion.Euler(0,0,maxAngle)*new Vector3(0,-maxLen,0);
			else if(sh1tmp==-1)
				tmpVec = Quaternion.Euler(0,0,-maxAngle)*new Vector3(0,-maxLen,0);
			else
				tmpVec=Quaternion.Euler(0,0,-f3tmp)*new Vector3(0,-maxLen,0);
			
			attackIcon.transform.localPosition=tmpVec;
		}
		else
		{
			if(sh1tmp==1)
			{
				tmpVec = Quaternion.Euler(0,0,maxAngle)*new Vector3(0,-Mathf.Sqrt(d),0);
				attackIcon.transform.localPosition=tmpVec;
			}
			else if(sh1tmp==-1)
			{
				tmpVec = Quaternion.Euler(0,0,-maxAngle)*new Vector3(0,-Mathf.Sqrt(d),0);
				attackIcon.transform.localPosition=tmpVec;
			}
			else
				attackIcon.transform.position=tmpVec;
		}
		
		
		if(activeAbil!=Abilities.AbilityType.none)
			abilitiesGameObjects[0].transform.eulerAngles=forActiveAbil;
		abilitiesGameObjects[1].transform.eulerAngles=forAbilities;
		abilitiesGameObjects[2].transform.eulerAngles=forAbilities;
		abilitiesGameObjects[3].transform.eulerAngles=forAbilities;
		abilitiesGameObjects[4].transform.eulerAngles=forAbilities;
		
		CalculatePath();
		return true;
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
	
	public void onAttackIconDeCaptured()
	{
		//attackIconMeshRenderer.sprite=Templates.AttackIconState.normalState;
	}
	
	public void onZoomChanged()
	{
		if(attackIcon!=null)
		{
			tmpVec.x=(States.currentZoom/20)*5;
			tmpVec.y=(States.currentZoom/20)*5;
			tmpVec.z=1;
			attackIcon.transform.localScale=tmpVec;
		}
	}
	
	private void createArcs()
	{
		arcObjs=new ArrayList();
		foreach(Templates.GunOnShuttle goss in planeTemplate.guns)
		{
			//go
			gunTmp = (GameObject) Instantiate(Resources.Load("prefab/gunArcPrefab"),transform.position,Quaternion.Euler(0,goss.turnAngle,0));
			gt = Templates.getInstance().getGunTemplate(goss.gunId);
			m = gunTmp.GetComponent<MeshFilter>().mesh;
	      	m.Clear();
	        vertices[0]=new Vector3(0,1,0);
	        d=gt.attackRange;
	        
	        Vector2 va;
	        i=1;
	        for(float ang = -gt.attackAngle; ang<=gt.attackAngle; ang+=(2*gt.attackAngle)/100.0f)
	        {
	        	va=Quaternion.Euler(0,0,ang)*new Vector2(0,d);
	        	vertices[i]=new Vector3(va.x,0,va.y);
	        	i++;
	        	if(i==101) break;
	        }
	        
	        int[] triangles = new int[vertices.Length*3];
	        int bb=0;
	        for(i=0;i<99;i++)
	        {
	        	triangles[bb*3]=0;
	        	triangles[bb*3+1]=i+1;
	        	triangles[bb*3+2]=i+2;
	        	bb++;
	        }
	   
	        Vector2[] uvs = new Vector2[vertices.Length];
			for (i = 0; i < uvs.Length; i++) {
				uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
			}
       
	        m.vertices=vertices;
	        m.triangles=triangles;
	        m.uv=uvs;
	       	m.RecalculateBounds();
	        m.RecalculateNormals();
	        arcObjs.Add(gunTmp);
	        gunTmp.transform.SetParent(transform);
	        gunTmp.transform.localPosition-=new Vector3(goss.pos.x,goss.pos.y,0);
	        gunTmp.SetActive(false);
		}
	}
	
	private void fillConstraints()
	{		
		maxLen=planeTemplate.maxRange;
		minLen=planeTemplate.minRange;
		maxAngle=planeTemplate.maxTurnAngle;
		forUpdatePositionNORMAL=new Vector3(0,-maxLen,0);
		hp=planeTemplate.hp;
	}
	
	private void initAbils()
	{
		i=1;
		foreach(int abil in planeTemplate.abilities)
		{
			//abilitiesGameObjects[i].SetActive(true);
			abilitiesGameObjects[i].GetComponent<SpriteRenderer>().sprite=Templates.AbilitiesIcons.getIcon((Abilities.AbilityType) abil,false);
			abilitiesGameObjects[i].transform.eulerAngles=forAbilities;
			i++;
		}
		
		if(Abilities.haveAbil(planeTemplate.abilities,Abilities.AbilityType.homingMissle))
			MisslePoolManager.getInstance().createPoolObjects(MissleSide.FRIENDLY,MissleType.ROCKET,4);
		if(Abilities.haveAbil(planeTemplate.abilities,Abilities.AbilityType.homingThorpede))
			MisslePoolManager.getInstance().createPoolObjects(MissleSide.FRIENDLY,MissleType.THORPEDE,4);
	}
	
	void Start () {
		BulletPoolManager.getInstance().createPoolObjs(BulletType.GUN_SMALL_BULLET,15);
		planeTemplate=Templates.getInstance().getPlaneTemplate(planeTemplateId);
		attackIconMeshRenderer=attackIcon.GetComponent<SpriteRenderer>();
		lineRenderer=gameObject.GetComponent<LineRenderer>();
		bodyMeshRenderer=GetComponent<MeshRenderer>();
		bodyCollider=GetComponent<Collider>();
		explosionParticle.SetActive(false);
		GameStorage.getInstance().addToList(this);
		fillConstraints();
		createArcs();
		updateAttackIconPosition();
		CalculatePath();
		initAbils();
	}
}
