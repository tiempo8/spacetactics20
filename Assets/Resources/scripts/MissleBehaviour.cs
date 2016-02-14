using UnityEngine;
using System.Collections;

public class MissleBehaviour : MonoBehaviour {

	public MissleSide side;
	public MissleType type;
	public GameObject attackIcon;
	private Vector3 tmpVec;
	private float d,angtmp,f3tmp,f1tmp,f2tmp,maxLen,minLen,maxAngle,lowerSmooth,upperSmooth;
	private Vector2 v2tmp1,v2tmp2,point1,point2,point3,point4,pointz;
	private short sh1tmp;
	private LineRenderer lineRenderer;
	float x,y,tt,t=0,step=0.02f;
	private float attackAngleMov=0f;
	private int i;
	private Vector3 forUpdatePositionNORMAL;
	private bool exploded=false;
	private FriendlySpaceship target;
	private Vector2 targetPosition;
	private float between,newAngle;
	public GameObject explosionParticle;
	private float explodedDeltaTime=0;
	private MeshRenderer meshRender;
	private int lifeCycle=0;
	
	private float getAttackIconAngle()
	{
		
		v2tmp1 = TagsStorage.oneVec;
		v2tmp2 = new Vector2(attackIcon.transform.position.x-transform.position.x,attackIcon.transform.position.z-transform.position.z);
		f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
		f2tmp = Vector2.Angle(v2tmp1,v2tmp2);
		if(f1tmp<=0)
			return f2tmp;
		else
			return (180-f2tmp)+180;
	}
	
	public void onStepEnd()
	{
		t=0;
		if(type==MissleType.ROCKET)
		{
			if(lifeCycle==Abilities.RocketParameters.lifeTimeRounds)
				Destroyed();
			
			if(side==MissleSide.FRIENDLY)
			{
				if(!exploded)
				{
					updateAttackIconPosition();
					showIcon();
				}
			}
			
		}
		if(type==MissleType.THORPEDE)
		{
			if(lifeCycle==Abilities.ThorpedeParameters.lifeTimeRounds)
				Destroyed();
			
			if(side==MissleSide.FRIENDLY)
			{
				if(!exploded)
				{
					updateAttackIconPosition();
					showIcon();
				}
			}
			
		}
		if(type==MissleType.GAS)
		{
			if(lifeCycle==Abilities.GasParameters.lifeTimeRounds)
				DestroyedGas();
		}
		if(type==MissleType.MINES)
		{
			if(lifeCycle==Abilities.MinesParameters.lifeTimeRounds)
				Destroyed();
		}
		lifeCycle++;
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
	
	public void onStepStart()
	{
		if(type==MissleType.ROCKET || type==MissleType.THORPEDE)
		{
			if(side==MissleSide.FRIENDLY)
				hideIcon();
			else
			{
				CalculatePath();
				CalculatePath();
			}
		}
	}
		
	private float getAngleDst(float fr, float to)
	{
		v2tmp1 = Quaternion.Euler(0,0,fr)*TagsStorage.oneVec;
		v2tmp2 = Quaternion.Euler(0,0,to)*TagsStorage.oneVec;
		
		f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
		f2tmp = fr-to;
		if(Mathf.Abs(f2tmp)>180)
			f2tmp=360-Mathf.Abs(f2tmp);
		
		if(f1tmp>0)
			return -Mathf.Abs(f2tmp);
		else
			return Mathf.Abs(f2tmp);
	}
	
	private float getAttackAngle()
	{
		v2tmp1 = TagsStorage.oneVec;
		v2tmp2 = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x-transform.position.x,Camera.main.ScreenToWorldPoint(Input.mousePosition).z-transform.position.z);
		f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
		f2tmp = Vector2.Angle(v2tmp1,v2tmp2);
		if(f1tmp<=0)
			return f2tmp;
		else
			return (180-f2tmp)+180;
	}
	
	private short attackIconOutOfBounds(float ang)
	{
		if(ang>=maxAngle)
			return (short)-1;
		else if(ang<=-maxAngle)
			return (short)1;
		else
			return (short)0;
	}
	
	public void OnAttackIconCaptured()
	{
		
	}
	
	public void OnAttackIconDecaptured()
	{
		
	}
	
	public void STEP()
	{
		if(!exploded)
		{
			t+=1f/151f;
			x=(1-t)*(1-t)*(1-t)*point1.x+3*(1-t)*(1-t)*t*point2.x+3*(1-t)*t*t*point3.x+t*t*t*point4.x;
			y=(1-t)*(1-t)*(1-t)*point1.y+3*(1-t)*(1-t)*t*point2.y+3*(1-t)*t*t*point3.y+t*t*t*point4.y;
			tmpVec.x=x;
			tmpVec.y=5;
			tmpVec.z=y;
			v2tmp1.x=x-transform.position.x;
			v2tmp1.y=y-transform.position.z;
			v2tmp2 = TagsStorage.oneVec;
			f1tmp = (v2tmp2.x*v2tmp1.y - v2tmp2.y*v2tmp1.x);
			f2tmp = Vector2.Angle(v2tmp2,v2tmp1);
			if(f1tmp>0)
				f2tmp=(180-f2tmp)+180;
			transform.eulerAngles=new Vector3(270,f2tmp,0);		
			transform.position=tmpVec;
		}
	}
	
	private void CalculatePath()
	{
		if(side==MissleSide.ENEMY)
		{
			target=GameStorage.getInstance().getNearbyFriendly(this);
			if(target!=null)
			{
				v2tmp1 = new Vector2(target.transform.position.x-transform.position.x,target.transform.position.z-transform.position.z);
				v2tmp2 = TagsStorage.oneVec;
				f1tmp = (v2tmp2.x*v2tmp1.y - v2tmp2.y*v2tmp1.x); // sinphi
				f2tmp = Vector2.Angle(v2tmp1,v2tmp2); // mangle
				if(f1tmp>=0)
					f2tmp=(180-f2tmp)+180;
				
				between = GameStorage.getInstance().getAngleDst(transform.eulerAngles.y,f2tmp);
				
				
				if(Mathf.Abs(between)>maxAngle)
				{
					if(between>0)
						newAngle=Mathf.Repeat(transform.eulerAngles.y-maxAngle,360);
					else
						newAngle=Mathf.Repeat(transform.eulerAngles.y+maxAngle,360);
				}
				else
					newAngle=Mathf.Repeat(transform.eulerAngles.y-between,360);
				
				targetPosition=Quaternion.Euler(0,0,-newAngle)*new Vector2(0,Random.Range(minLen,maxLen));
				attackIcon.transform.position=new Vector3(targetPosition.x+transform.position.x,5,targetPosition.y+transform.position.z);
			}
		}
		
		point1=new Vector2(transform.position.x,transform.position.z);
		point2=Quaternion.Euler(0,0,-transform.eulerAngles.y)*new Vector2(0,minLen*Mathf.Abs(getAngleDst(transform.eulerAngles.y,getAttackIconAngle())/maxAngle)*Vector2.Distance(point1,point4)/maxLen)*lowerSmooth;
		point2+=point1;
		point4=new Vector2(attackIcon.transform.position.x,attackIcon.transform.position.z);
		pointz = new Vector2(point4.x-point2.x,point4.y-point2.y)/2;
		point3 = new Vector2(pointz.y,-pointz.x)*getAngleDst(transform.eulerAngles.y,getAttackIconAngle())/maxAngle*Vector2.Distance(point1,point4)/maxLen*upperSmooth;
		point3 = point3+point2+pointz;
		
		if(side==MissleSide.FRIENDLY)
		{
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
					v2tmp1 = TagsStorage.oneVec;
					f1tmp = (v2tmp1.x*v2tmp2.y - v2tmp1.y*v2tmp2.x);
					f2tmp = Vector2.Angle(v2tmp1,v2tmp2);
					if(f1tmp<=0)
						attackAngleMov= f2tmp;
					else
						attackAngleMov = (180-f2tmp)+180;
					attackIcon.transform.localEulerAngles=new Vector3(0,180,-attackAngleMov+270+transform.eulerAngles.y);
					attackIcon.transform.GetChild(0).transform.eulerAngles=new Vector3(90,attackAngleMov+270,0);
				}
				
				i++;
			}	
		}	
	}
	
	public bool OnAttackIconMoved(Vector3 validation)
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
		
		CalculatePath();
		CalculatePath();
		return true;
	}
	
	
	
	public void hideIcon()
	{
		attackIcon.SetActive(false);
		lineRenderer.SetWidth(0,0);
	}
	
	public void showIcon()
	{
		attackIcon.SetActive(true);
		lineRenderer.SetWidth(0.2f,0.2f);
	}
	
	private void updateAttackIconPosition()
	{
		attackIcon.transform.localPosition=forUpdatePositionNORMAL;
		CalculatePath();
		CalculatePath();
	}
	
	public void Activate()
	{
		if(type==MissleType.ROCKET)
		{
			maxLen=Abilities.RocketParameters.maxRange;
			minLen=Abilities.RocketParameters.minRange;
			maxAngle=Abilities.RocketParameters.maxTurnAngle;
			lowerSmooth=Abilities.RocketParameters.lowerSmooth;
			upperSmooth=Abilities.RocketParameters.upperSmooth;
			if(side==MissleSide.FRIENDLY)
				lineRenderer=GetComponent<LineRenderer>();
			forUpdatePositionNORMAL=new Vector3(0,-maxLen,0);
			updateAttackIconPosition();
			meshRender=GetComponent<MeshRenderer>();
		}
		else if(type==MissleType.THORPEDE)
		{
			maxLen=Abilities.ThorpedeParameters.maxRange;
			minLen=Abilities.ThorpedeParameters.minRange;
			maxAngle=Abilities.ThorpedeParameters.maxTurnAngle;
			lowerSmooth=Abilities.ThorpedeParameters.lowerSmooth;
			upperSmooth=Abilities.ThorpedeParameters.upperSmooth;
			if(side==MissleSide.FRIENDLY)
				lineRenderer=GetComponent<LineRenderer>();
			forUpdatePositionNORMAL=new Vector3(0,-maxLen,0);
			updateAttackIconPosition();
			meshRender=GetComponent<MeshRenderer>();
		}
		else if(type==MissleType.MINES)
		{
			meshRender=GetComponent<MeshRenderer>();
		}
		lifeCycle=0;
	}
	
	public void Boom()
	{
		if(type==MissleType.GAS)
			DestroyedGas();
		else
			Destroyed();
	}
	
	public bool Explode()
	{
		if(exploded)
		{
			explodedDeltaTime+=Time.deltaTime;
			if(type==MissleType.GAS)
			{
				if(explodedDeltaTime>=5)
					return true;
			}
			else
			{
				if(explodedDeltaTime>=3)
					return true;
			}
		}
		return false;
	}
	
	private void Destroyed()
	{
		explosionParticle.SetActive(true);
		meshRender.enabled=false;
		exploded=true;
		GameStorage.getInstance().addToExplodeList(this);
	}
	
	private void DestroyedGas()
	{
		//explosionParticle.SetActive(true);
		//meshRender.enabled=false;
		this.transform.position=MisslePoolManager.getInstance().stackPos;
		exploded=true;
		GameStorage.getInstance().addToExplodeList(this);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if(side==MissleSide.ENEMY)
		{
			if(type==MissleType.ROCKET)
			{
				if(col.gameObject.tag==TagsStorage.FRIENDLY_TAG)
				{
					col.gameObject.GetComponent<FriendlySpaceship>().Attacked(Abilities.RocketParameters.damage);
					Destroyed();
				}
			}
			
			if(type==MissleType.THORPEDE)
			{
				if(col.gameObject.tag==TagsStorage.FRIENDLY_TAG)
				{
					col.gameObject.GetComponent<FriendlySpaceship>().Attacked(Abilities.ThorpedeParameters.damage);
					Destroyed();
				}
			}
			
			if(type==MissleType.GAS)
			{
				if(col.gameObject.tag==TagsStorage.FRIENDLY_TAG)
				{
					col.gameObject.GetComponent<FriendlySpaceship>().onGasAreaEnter();
				}
			}
			
			if(type==MissleType.MINES)
			{
				if(col.gameObject.tag==TagsStorage.FRIENDLY_TAG)
				{
					col.gameObject.GetComponent<FriendlySpaceship>().Attacked(Abilities.MinesParameters.Damage);
					Destroyed();
				}
			}
		}
		
		if(side==MissleSide.FRIENDLY)
		{
			if(type==MissleType.ROCKET)
			{
				if(col.gameObject.tag==TagsStorage.ENEMY_TAG)
				{
					col.gameObject.GetComponent<EnemySpaceship>().Attacked(Abilities.RocketParameters.damage);
					Destroyed();
				}
			}
			
			if(type==MissleType.THORPEDE)
			{
				if(col.gameObject.tag==TagsStorage.ENEMY_TAG)
				{
					col.gameObject.GetComponent<EnemySpaceship>().Attacked(Abilities.ThorpedeParameters.damage);
					Destroyed();
				}
			}
			
			if(type==MissleType.GAS)
			{
				if(col.gameObject.tag==TagsStorage.ENEMY_TAG)
				{
					col.gameObject.GetComponent<EnemySpaceship>().onGasAreaEnter();
				}
			}
			
			if(type==MissleType.MINES)
			{
				if(col.gameObject.tag==TagsStorage.ENEMY_TAG)
				{
					col.gameObject.GetComponent<EnemySpaceship>().Attacked(Abilities.MinesParameters.Damage);
					Destroyed();
				}
			}
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if(side==MissleSide.FRIENDLY)
		{
			if(type==MissleType.GAS)
			{
				if(col.gameObject.tag==TagsStorage.ENEMY_TAG)
				{
					col.gameObject.GetComponent<EnemySpaceship>().onGasAreaLeave();
				}
			}
		}
		
		if(side==MissleSide.ENEMY)
		{
			if(type==MissleType.GAS)
			{
				if(col.gameObject.tag==TagsStorage.FRIENDLY_TAG)
				{
					col.gameObject.GetComponent<FriendlySpaceship>().onGasAreaLeave();
				}
			}
		}
	}
}
