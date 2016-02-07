using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {
	
	//CLICK SYSTEM
	private Ray ray;
	private RaycastHit hit;
	
	//CAMERA DRAG SYSTEM
	private bool canDragCamera=false;
	private Vector3 prevMousePos,curMousePos;
	
	//CAMERA ZOOM SYSTEM
	private const float maxZoom=50,minZoom=5,zoomSpd=5;
	private float zoomA,zoomB;
	private Touch touchZero,touchOne;
	
	//BOUNDS
	private const float xMax = 250,zMax=185;
	private float lh,lw;
	private Vector3 v3temp1,v3temp2;
	private Vector2 v2temp1,v2temp2;
	
	//HZWTF
	private int forAndroid=0;
	
	//TMP
	private GameObject tmp;
	
	void Update () {
		
		
			BeginOfUpdate();
			if(Application.isMobilePlatform)
			{
				if(Input.touchCount==1 && Input.GetTouch(0).phase==TouchPhase.Began)
				{
					if((tmp=fingerOver(Input.GetTouch(0)))!=null)
					{
						if(tmp.tag=="attack_icon")
						{
							States.parentSpaceshipInstance=tmp.transform.parent.GetComponent<FriendlySpaceship>();
							States.parentSpaceshipInstance.onAttackIconCapture();
							GameStorage.getInstance().toggleHideAllAttackIcons(States.parentSpaceshipInstance);
							States.currentAttackIconCaptured=tmp;
							if(States.currentSelected!=States.parentSpaceshipInstance.gameObject)
							{
								if(States.currentSelected!=null)
								{
									if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
										States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
									if(States.currentSelectedTag==States.SelectedType.ENEMY)
										States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
								}
								States.parentSpaceshipInstance.onSelected();
								States.currentSelected=States.parentSpaceshipInstance.gameObject;
								States.currentSelectedTag=States.SelectedType.FRIENDLY;
							}
						}
						else if(tmp.tag=="friendly")
						{
							if(States.currentSelected!=null)
							{
								if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
									States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
								if(States.currentSelectedTag==States.SelectedType.ENEMY)
										States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
							}
							tmp.GetComponent<FriendlySpaceship>().onSelected();
							States.currentSelected=tmp;
							States.currentSelectedTag=States.SelectedType.FRIENDLY;
						}
						else if(tmp.tag=="enemy")
						{
							if(States.currentSelected!=null)
							{
								if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
									States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
								if(States.currentSelectedTag==States.SelectedType.ENEMY)
										States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
							}
							tmp.GetComponent<EnemySpaceship>().onSelected();
							States.currentSelected=tmp;
							States.currentSelectedTag=States.SelectedType.ENEMY;
						}
						else if(tmp.tag.Split('_')[0]=="abil")
						{
							if(tmp.tag.Split('_')[1]!="reused")
								tmp.transform.parent.transform.parent.GetComponent<FriendlySpaceship>().onAbilitySwitched(int.Parse(tmp.tag.Split('_')[1]));
							else
								tmp.transform.parent.transform.parent.GetComponent<FriendlySpaceship>().onAbilitySwitched(-1);
						}
					
						else
						{
							if(States.currentSelected!=null)
							{
								if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
								{
									States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
									States.currentSelected=null;
									States.currentSelectedTag=States.SelectedType.NONE;
								}
							}
						}
					}
					else
					{
						if(States.currentSelected!=null)
						{
							if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
							{
								States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
								States.currentSelected=null;
								States.currentSelectedTag=States.SelectedType.NONE;
							}
							else if(States.currentSelectedTag==States.SelectedType.ENEMY)
							{
								States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
								States.currentSelected=null;
								States.currentSelectedTag=States.SelectedType.NONE;
							}
						}
					}
				}
				
				if(Input.touchCount>=1)
				{
					if(Input.GetTouch(0).phase==TouchPhase.Canceled || Input.GetTouch(0).phase==TouchPhase.Ended)
					{
						if(States.currentAttackIconCaptured!=null)
						{
							GameStorage.getInstance().toggleShowAllAttackIcons(States.parentSpaceshipInstance);
							States.parentSpaceshipInstance.onAttackIconDeCaptured();
							States.parentSpaceshipInstance=null;
							States.currentAttackIconCaptured=null;
						}
					}
				}
				
				if(States.currentAttackIconCaptured!=null)
				{
					if(!States.parentSpaceshipInstance.onAttackIconMoved(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)))
					{
						GameStorage.getInstance().toggleShowAllAttackIcons(States.parentSpaceshipInstance);
						States.parentSpaceshipInstance.onAttackIconDeCaptured();
						States.parentSpaceshipInstance=null;
						States.currentAttackIconCaptured=null;
					}
				}
			}
			else
			{
				if(Input.GetMouseButtonDown(0))
				{
					if((tmp=mouseOver())!=null)
					{
						if(tmp.tag==TagsStorage.ATTACK_ICON_TAG)
						{
							States.parentSpaceshipInstance=tmp.transform.parent.GetComponent<FriendlySpaceship>();
							States.parentSpaceshipInstance.onAttackIconCapture();
							GameStorage.getInstance().toggleHideAllAttackIcons(States.parentSpaceshipInstance);
							States.currentAttackIconCaptured=tmp;
							
							if(States.currentSelected!=States.parentSpaceshipInstance.gameObject)
							{
								if(States.currentSelected!=null)
								{
									if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
										States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
									if(States.currentSelectedTag==States.SelectedType.ENEMY)
										States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
								}
								States.parentSpaceshipInstance.onSelected();
								States.currentSelected=States.parentSpaceshipInstance.gameObject;
								States.currentSelectedTag=States.SelectedType.FRIENDLY;
							}
						}
						else if(tmp.tag==TagsStorage.ATTACK_ICON_TAG_MISSLE)
						{
							States.parentMissleInstance=tmp.transform.parent.GetComponent<MissleBehaviour>();
							States.parentMissleInstance.OnAttackIconCaptured();
							GameStorage.getInstance().toggleHideAllAttackIcons(States.parentMissleInstance);
							States.currentAttackIconCaptured=tmp;
							
							
							if(States.currentSelected!=null)
							{
								if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
									States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
								if(States.currentSelectedTag==States.SelectedType.ENEMY)
									States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
							}
							States.currentSelected=null;
							States.currentSelectedTag=States.SelectedType.NONE;
						}
						else if(tmp.tag==TagsStorage.FRIENDLY_TAG)
						{
							if(States.currentSelected!=null)
							{
								if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
									States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
								if(States.currentSelectedTag==States.SelectedType.ENEMY)
										States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
							}
							tmp.GetComponent<FriendlySpaceship>().onSelected();
							States.currentSelected=tmp;
							States.currentSelectedTag=States.SelectedType.FRIENDLY;
						}
						else if(tmp.tag==TagsStorage.ENEMY_TAG)
						{
							if(States.currentSelected!=null)
							{
								if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
									States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
								if(States.currentSelectedTag==States.SelectedType.ENEMY)
										States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
							}
							tmp.GetComponent<EnemySpaceship>().onSelected();
							States.currentSelected=tmp;
							States.currentSelectedTag=States.SelectedType.ENEMY;
						}
						else if(tmp.tag.Split('_')[0]==TagsStorage.ABIL_SUBTAG)
						{
							if(tmp.tag.Split('_')[1]!=TagsStorage.ABIL_SUBTAG_REUSED)
								tmp.transform.parent.transform.parent.GetComponent<FriendlySpaceship>().onAbilitySwitched(int.Parse(tmp.tag.Split('_')[1]));
							else
								tmp.transform.parent.transform.parent.GetComponent<FriendlySpaceship>().onAbilitySwitched(-1);
						}
						else
						{
							if(States.currentSelected!=null)
							{
								if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
								{
									States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
									States.currentSelected=null;
									States.currentSelectedTag=States.SelectedType.NONE;
								}
							}
							else if(States.currentSelectedTag==States.SelectedType.ENEMY)
							{
								States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
								States.currentSelected=null;
								States.currentSelectedTag=States.SelectedType.NONE;
							}
						}
					}
					else
					{
						if(States.currentSelected!=null)
						{
							if(States.currentSelectedTag==States.SelectedType.FRIENDLY)
							{
								States.currentSelected.GetComponent<FriendlySpaceship>().onDeselected();
								States.currentSelected=null;
								States.currentSelectedTag=States.SelectedType.NONE;
							}
							else if(States.currentSelectedTag==States.SelectedType.ENEMY)
							{
								States.currentSelected.GetComponent<EnemySpaceship>().onDeselected();
								States.currentSelected=null;
								States.currentSelectedTag=States.SelectedType.NONE;
							}
						}
					}
				}
				
				if(Input.GetMouseButtonUp(0))
				{
					if(States.currentAttackIconCaptured!=null)
					{
						if(States.currentAttackIconCaptured.tag==TagsStorage.ATTACK_ICON_TAG_MISSLE)
						{
							GameStorage.getInstance().toggleShowAllAttackIcons(States.parentMissleInstance);
							States.parentMissleInstance.OnAttackIconDecaptured();
							States.parentMissleInstance=null;
							States.currentAttackIconCaptured=null;
						}
						else
						{
							GameStorage.getInstance().toggleShowAllAttackIcons(States.parentSpaceshipInstance);
							States.parentSpaceshipInstance.onAttackIconDeCaptured();
							States.parentSpaceshipInstance=null;
							States.currentAttackIconCaptured=null;
						}
					}
				}
				
				if(States.currentAttackIconCaptured!=null)
				{
					if(States.currentAttackIconCaptured.tag==TagsStorage.ATTACK_ICON_TAG_MISSLE)
					{
						if(!States.parentMissleInstance.OnAttackIconMoved(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
						{
							GameStorage.getInstance().toggleShowAllAttackIcons(States.parentMissleInstance);
							States.parentMissleInstance.OnAttackIconDecaptured();
							States.parentMissleInstance=null;
							States.currentAttackIconCaptured=null;
						}
					}
					else
					{
						if(!States.parentSpaceshipInstance.onAttackIconMoved(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
						{
							GameStorage.getInstance().toggleShowAllAttackIcons(States.parentSpaceshipInstance);
							States.parentSpaceshipInstance.onAttackIconDeCaptured();
							States.parentSpaceshipInstance=null;
							States.currentAttackIconCaptured=null;
						}
					}
				}
			}
			DragCamera();
			MoveCamera();
			CameraZoom();
			EndOfUpdate();
		
	}
	
	void FixedUpdate()
	{
		Accelerates();
	}
	
	private void Accelerates()
	{
		GameStorage.getInstance().processExplode();
		if(States.WorldRunning)
		{
			GameStorage.getInstance().AccelerateAllUnits();
			BulletPoolManager.getInstance().stepAllBullets();
			MisslePoolManager.getInstance().MoveRocketsAndThorpeds();
			if(Time.time>=GameStorage.getInstance().getEndTime())
			{
				States.WorldRunning=false;
				GameStorage.getInstance().toggleOnStepEnd();
			}
		}
	}
	
	/*
	 * Функция вызывается в начале отрисовки (перед функцией Update)
	 */
	private void BeginOfUpdate()
	{
		if(Application.platform==RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			curMousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
	
	/*
	 * Функция вызывается в конце отрисовки (после функции Update)
	 */
	private void EndOfUpdate()
	{
		if(Application.platform==RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
			prevMousePos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
	
	private void CameraZoom()
	{
		if(Application.platform==RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			if(Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				if(Camera.main.orthographicSize<=minZoom)
					Camera.main.orthographicSize=minZoom;
				else
					Camera.main.orthographicSize-=Input.GetAxis("Mouse ScrollWheel")*zoomSpd;
				States.currentZoom=Camera.main.orthographicSize;
				
				GameStorage.getInstance().toggleOnZoomChangedEvent();
				
				CorrectCameraPosition();
			}
			else if(Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				if(Camera.main.orthographicSize>=maxZoom)
					Camera.main.orthographicSize=maxZoom;
				else
					Camera.main.orthographicSize-=Input.GetAxis("Mouse ScrollWheel")*zoomSpd;
				States.currentZoom=Camera.main.orthographicSize;
				
				GameStorage.getInstance().toggleOnZoomChangedEvent();
				
				CorrectCameraPosition();
			}
		}
		else if(Application.platform==RuntimePlatform.Android)
		{
			if(Input.touchCount==2)
			{
				touchZero = Input.GetTouch(0);
	            touchOne = Input.GetTouch(1);
				prevMousePos = touchZero.position - touchZero.deltaPosition;
	            curMousePos = touchOne.position - touchOne.deltaPosition;
				zoomA = (prevMousePos - curMousePos).magnitude;
	            zoomB = (touchZero.position - touchOne.position).magnitude;
	            Camera.main.orthographicSize += (zoomA - zoomB) * 0.5f;
                Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, minZoom);
                Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, maxZoom);
                CorrectCameraPosition();
                States.currentZoom=Camera.main.orthographicSize;
				GameStorage.getInstance().toggleOnZoomChangedEvent();
			}
		}
	}
	
	private GameObject mouseOver() 
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      	if(Physics.Raycast(ray,out hit,float.PositiveInfinity))
      	{
      		if(hit.collider.gameObject.tag!="bounds")
        		return hit.collider.gameObject;
      	}
    	return null;
    }   
	
	private GameObject fingerOver(Touch touch)
	{
		ray = Camera.main.ScreenPointToRay(touch.position);
      	if(Physics.Raycast(ray,out hit,float.PositiveInfinity))
      	{
      		if(hit.collider.gameObject.tag!="bounds")
        		return hit.collider.gameObject;
      	}
    	return null;
	}
	
	private void DragCamera()
	{
		if(Application.platform==RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			if(Input.GetMouseButtonDown(0))
			{
				if(mouseOver()==null)
				{
					canDragCamera=true;
				}
			}
		}
		else if(Application.platform==RuntimePlatform.Android)
		{
			if(Input.touchCount>=1)
			{
				if(Input.touchCount==1 && Input.GetTouch(0).phase == TouchPhase.Began && fingerOver(Input.GetTouch(0))==null)
				{
					curMousePos=Vector3.zero;
					canDragCamera=true;
				}
			}
		}
		
		if(Application.platform==RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
		{
			if(Input.GetMouseButtonUp(0))
				canDragCamera=false;
		}
		else if(Application.platform==RuntimePlatform.Android)
		{
			if(Input.touchCount>=1)
			{
				if((Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled))
				{
					canDragCamera=false;
					curMousePos=Vector3.zero;
				}
			}
		}
	}
	
	private void MoveCamera()
	{
		if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
		{
			if(canDragCamera)
			{
				Camera.main.transform.position+=new Vector3((curMousePos.x-prevMousePos.x),0,(curMousePos.z-prevMousePos.z))*-1;
				CorrectCameraPosition();
			}
		}
		else if(Application.platform == RuntimePlatform.Android)
		{
			if(canDragCamera)
			{
				if(Input.touchCount==1)
				{
					if(Input.GetTouch(0).phase == TouchPhase.Moved) 
					{
	            	 	curMousePos = Input.GetTouch(0).deltaPosition;
	            	 	transform.position-=new Vector3(curMousePos.x,0,curMousePos.y)/10*(Camera.main.orthographicSize/(maxZoom/2));
	            	 	CorrectCameraPosition();
	         		}
				}
			}
		}
	}
	
	private void CorrectCameraPosition()
	{
		v3temp1=Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,0));
		v2temp1=new Vector2(v3temp1.x,v3temp1.z);
	
		v3temp2=Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height/2,0));
		v2temp2=new Vector2(v3temp2.x,v3temp2.z);
	
		lw=Vector2.Distance(v2temp1,v2temp2);
	
		v3temp1=Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2,0));
		v2temp1=new Vector2(v3temp1.x,v3temp1.z);
	
		v3temp2=Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,0,0));
		v2temp2=new Vector2(v3temp2.x,v3temp2.z);
	
		lh=Vector2.Distance(v2temp1,v2temp2);
	
		if(Camera.main.transform.position.x<(-xMax+lw))
			Camera.main.transform.position=new Vector3(-xMax+lw,Camera.main.transform.position.y,Camera.main.transform.position.z);
		if(Camera.main.transform.position.x>(xMax-lw))
			Camera.main.transform.position=new Vector3(xMax-lw,Camera.main.transform.position.y,Camera.main.transform.position.z);
		
		if(Camera.main.transform.position.z<(-zMax+lh))
			Camera.main.transform.position=new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,-zMax+lh);
		if(Camera.main.transform.position.z>(zMax-lh))
			Camera.main.transform.position=new Vector3(Camera.main.transform.position.x,Camera.main.transform.position.y,zMax-lh);
	}
}
