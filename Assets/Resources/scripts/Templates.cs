﻿using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class Templates{
	
	private static Templates instance = null;
	public static Templates getInstance()
	{
		if(instance==null)
			instance=new Templates();
		return instance;
	}
	
	public ArrayList campaignsList,levelList;
	private ArrayList planeClasses,gunClasses;
	public GameObject missionBlockPrefab;
	
	private Sprite[] numbersSprites;
	private Sprite[] numbersSpritesGrey;
	
	public Templates()
	{
		missionBlockPrefab=(GameObject)Resources.Load("prefab/gui/missionBlock");
		//f
		
		AbilitiesIcons.abil_180x70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_180x70");
		AbilitiesIcons.abil_360x70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_360x70");
		AbilitiesIcons.abil_double_throttlex70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_double_throttlex70");
		AbilitiesIcons.abil_gasx70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_gasx70");
		AbilitiesIcons.abil_rocketx70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_rocketx70");
		AbilitiesIcons.abil_thorpedex70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_thorpedex70");
		AbilitiesIcons.abil_minesx70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_minesx70");
		AbilitiesIcons.abil_shieldx70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_shieldx70");
		AbilitiesIcons.abil_commonx70=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_commonx70");
		
		AbilitiesIcons.abil_180x80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_180x80");
		AbilitiesIcons.abil_360x80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_360x80");
		AbilitiesIcons.abil_double_throttlex80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_double_throttlex80");
		AbilitiesIcons.abil_gasx80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_gasx80");
		AbilitiesIcons.abil_rocketx80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_rocketx80");
		AbilitiesIcons.abil_thorpedex80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_thorpedex80");
		AbilitiesIcons.abil_minesx80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_minesx80");
		AbilitiesIcons.abil_shieldx80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_shieldx80");
		AbilitiesIcons.abil_commonx80=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_commonx80");
		
		AbilitiesIcons.abil_180x70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_180x70_grey");
		AbilitiesIcons.abil_360x70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_360x70_grey");
		AbilitiesIcons.abil_double_throttlex70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_double_throttlex70_grey");
		AbilitiesIcons.abil_gasx70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_gasx70_grey");
		AbilitiesIcons.abil_rocketx70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_rocketx70_grey");
		AbilitiesIcons.abil_thorpedex70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_thorpedex70_grey");
		AbilitiesIcons.abil_minesx70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_minesx70_grey");
		AbilitiesIcons.abil_shieldx70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_shieldx70_grey");
		AbilitiesIcons.abil_commonx70_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_commonx70_grey");
		
		AbilitiesIcons.abil_180x80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_180x80_grey");
		AbilitiesIcons.abil_360x80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_360x80_grey");
		AbilitiesIcons.abil_double_throttlex80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_double_throttlex80_grey");
		AbilitiesIcons.abil_gasx80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_gasx80_grey");
		AbilitiesIcons.abil_rocketx80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_rocketx80_grey");
		AbilitiesIcons.abil_thorpedex80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_thorpedex80_grey");
		AbilitiesIcons.abil_minesx80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_minesx80_grey");
		AbilitiesIcons.abil_shieldx80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_shieldx80_grey");
		AbilitiesIcons.abil_commonx80_grey=(Sprite)Resources.Load<Sprite>("gui/skinsLow/img/abil_commonx80_grey");
//		
		
		//
		
		campaignsList=new ArrayList();
		loadCampaigns();
		levelList=new ArrayList();
		loadLevels();
		numbersSprites=new Sprite[10];
		numbersSpritesGrey=new Sprite[10];
		loadNumberGrafics();
		planeClasses=new ArrayList();
		gunClasses=new ArrayList();
		LoadGunClasses();
		LoadPlaneClasses();
	}
	
	public enum GunTemplates : int
	{
		default_gun=1,
		small_gun=2,
		medium_gun=3,
		large_gun=4,
		corvette_gun=5,
		battlecruiser_gun=6,
		fighter_gun=7,
		dogfight_gun=8
	};
	
	public class GunOnShuttle
	{
		public int gunId;
		public Vector2 pos;
		public float turnAngle;
		public bool ready=true;
		public float shotTime=0;
	}
	
	public static class AttackIconState
	{
		public static Sprite normalState=null;
		public static Sprite pressState=null;
		public static Sprite disabledState=null;
	}
	
	public static class AbilitiesIcons
	{
		public static Sprite abil_360x70;
		public static Sprite abil_180x70;
		public static Sprite abil_double_throttlex70;
		public static Sprite abil_gasx70;
		public static Sprite abil_rocketx70;
		public static Sprite abil_thorpedex70;
		public static Sprite abil_minesx70;
		public static Sprite abil_shieldx70;
		public static Sprite abil_commonx70;
		
		public static Sprite abil_360x80;
		public static Sprite abil_180x80;
		public static Sprite abil_double_throttlex80;
		public static Sprite abil_gasx80;
		public static Sprite abil_rocketx80;
		public static Sprite abil_thorpedex80;
		public static Sprite abil_minesx80;
		public static Sprite abil_shieldx80;
		public static Sprite abil_commonx80;
		
		public static Sprite abil_360x70_grey;
		public static Sprite abil_180x70_grey;
		public static Sprite abil_double_throttlex70_grey;
		public static Sprite abil_gasx70_grey;
		public static Sprite abil_rocketx70_grey;
		public static Sprite abil_thorpedex70_grey;
		public static Sprite abil_minesx70_grey;
		public static Sprite abil_shieldx70_grey;
		public static Sprite abil_commonx70_grey;
		
		public static Sprite abil_360x80_grey;
		public static Sprite abil_180x80_grey;
		public static Sprite abil_double_throttlex80_grey;
		public static Sprite abil_gasx80_grey;
		public static Sprite abil_rocketx80_grey;
		public static Sprite abil_thorpedex80_grey;
		public static Sprite abil_minesx80_grey;
		public static Sprite abil_shieldx80_grey;
		public static Sprite abil_commonx80_grey;
		
		public static Sprite getIcon(Abilities.AbilityType type, bool highres)
		{
			switch(type)
			{
				case Abilities.AbilityType.halfRoundTurn:
					if(highres)
						return abil_180x80;
					return abil_180x70;
					
				case Abilities.AbilityType.doubleThrottle:
					if(highres)
						return abil_double_throttlex80;
					return abil_double_throttlex70;
					
				case Abilities.AbilityType.gas:
					if(highres)
						return abil_gasx80;
					return abil_gasx70;
					
				case Abilities.AbilityType.homingMissle:
					if(highres)
						return abil_rocketx80;
					return abil_rocketx70;
					
				case Abilities.AbilityType.homingThorpede:
					if(highres)
						return abil_thorpedex80;
					return abil_thorpedex70;
					
				case Abilities.AbilityType.mines:
					if(highres)
						return abil_minesx80;
					return abil_minesx70;
					
				case Abilities.AbilityType.none:
					if(highres)
						return abil_commonx80;
					return abil_commonx70;
					
				case Abilities.AbilityType.shield:
					if(highres)
						return abil_shieldx80;
					return abil_shieldx70;
					
				case Abilities.AbilityType.turnAround:
					if(highres)
						return abil_360x80;
					return abil_360x70;
					
				default:
					return null;
			}
		}
		
		public static Sprite getIconGrey(Abilities.AbilityType type, bool highres)
		{
			switch(type)
			{
				case Abilities.AbilityType.halfRoundTurn:
					if(highres)
						return abil_180x80_grey;
					return abil_180x70_grey;
					
				case Abilities.AbilityType.doubleThrottle:
					if(highres)
						return abil_double_throttlex80_grey;
					return abil_double_throttlex70_grey;
					
				case Abilities.AbilityType.gas:
					if(highres)
						return abil_gasx80_grey;
					return abil_gasx70_grey;
					
				case Abilities.AbilityType.homingMissle:
					if(highres)
						return abil_rocketx80_grey;
					return abil_rocketx70_grey;
					
				case Abilities.AbilityType.homingThorpede:
					if(highres)
						return abil_thorpedex80_grey;
					return abil_thorpedex70_grey;
					
				case Abilities.AbilityType.mines:
					if(highres)
						return abil_minesx80_grey;
					return abil_minesx70_grey;
					
				case Abilities.AbilityType.none:
					if(highres)
						return abil_commonx80_grey;
					return abil_commonx70_grey;
					
				case Abilities.AbilityType.shield:
					if(highres)
						return abil_shieldx80_grey;
					return abil_shieldx70_grey;
					
				case Abilities.AbilityType.turnAround:
					if(highres)
						return abil_360x80_grey;
					return abil_360x70_grey;
					
				default:
					return null;
			}
		}
	}
	
	public class AttackIconConstraints
	{
		public float maxLen;
		public float minLen;
		public float maxAngle;
	}
	
	public class CampaignInfo
	{
		public int id;
		public string name;
		public string defaultRank;
		public string desc = "";
		public ArrayList levels = new ArrayList();
	}
	
	public class GunTemplate
	{
		public int id;
		public string gunName;
		public int damage;
		public float reuse;
		public float bulletSpeed;
		public float bulletDispersion;
		public float attackAngle,attackRange;
		public string bulletMesh;
		//public float[] defectsChance = new float[Enum.GetNames(typeof(Defects.DefectType)).Length];
	}
	
	public enum PlaneTemplates : int
	{
		default_class=1,
		AllyFighter=2,
		AllyScout=3,
		AllyAssault=4,
		AllyGas=5,
		AllyFrigate=6,
		AllyCruiser=7,
		AllyBattleship=8,
		AllyDreadnaught=9,
		AllyCorvette=10,
		AllyBattlecruiser=11,
		EnemyFighter=12,
		EnemyScout=13,
		EnemyAssault=14,
		EnemyGas=15,
		EnemyFrigate=16,
		EnemyCruiser=17,
		EnemyBattleship=18,
		EnemyDreadnaught=19,
		AllyShieldedFighter=20,
		AllyRoundFighter=21,
		AllyHeavyFighter=22,
		AllyNewFighter=23,
		AllyNewScout=24,
		EnemyShieldedFighter=25,
		EnemyRoundFighter=26,
		EnemyHeavyFighter=27,
		EnemyNewFighter=28,
		EnemyNewScout=29
	};
	
	public PlaneTemplate getPlaneTemplate(PlaneTemplates id)
	{
		foreach(PlaneTemplate p in planeClasses)
		{
			if(p.id==(int)id)
			{
				PlaneTemplate t = new PlaneTemplate();
				t.classname=p.classname;
				t.description=p.description;
				t.hp=p.hp;
				t.id=p.id;
				t.maxRange=p.maxRange;
				t.lowerSmooth=p.lowerSmooth;
				t.upperSmooth=p.upperSmooth;
				t.maxTurnAngle=p.maxTurnAngle;
				t.minRange=p.minRange;
				t.weapons=p.weapons;
				t.armor=p.armor;
				t.speed=p.speed;
				t.maneuverability=p.maneuverability;
				foreach(GunOnShuttle f in p.guns)
				{
					GunOnShuttle z = new GunOnShuttle();
					z.gunId=f.gunId;
					z.pos=f.pos;
					z.turnAngle=f.turnAngle;
					t.guns.Add(z);
				}
				
				foreach(int abilId in p.abilities)
					t.abilities.Add(abilId);
				return t;
			}
		}
		return null;
	}
	
	public GunTemplate getGunTemplate(int id)
	{
		foreach(GunTemplate p in gunClasses)
		{
			if(p.id==(int)id)
				return p;
		}
		return null;
	}
	
	private void LoadGunClasses()
	{
		string name;
		foreach(int Class in Enum.GetValues(typeof(GunTemplates)))
		{
			name = Enum.GetName(typeof(GunTemplates),Class);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(((TextAsset) Resources.Load("xml/guns/"+name)).text);
			foreach(XmlNode x in doc.ChildNodes)
			{
				if(x.Name=="gun")
				{
					GunTemplate p = new GunTemplate();
					foreach(XmlNode m in x.Attributes)
					{
						if(m.Name=="id")
							p.id=int.Parse(m.Value);
						else if(m.Name=="gunName")
							p.gunName=m.Value;
						else if(m.Name=="damage")
							p.damage=int.Parse(m.Value);
						else if(m.Name=="reuse")
							p.reuse=float.Parse(m.Value);
						else if(m.Name=="attackAngle")
							p.attackAngle=float.Parse(m.Value);
						else if(m.Name=="attackRange")
							p.attackRange=float.Parse(m.Value);
						else if(m.Name=="bulletSpeed")
							p.bulletSpeed=float.Parse(m.Value);
						else if(m.Name=="bulletDispersion")
							p.bulletDispersion=float.Parse(m.Value);
						else if(m.Name=="bulletMesh")
							p.bulletMesh=m.Value;
					}
					
					foreach(XmlNode m in x.ChildNodes)
					{
						if(m.Name=="defectChance")
						{
							int id=-1;
							float chance=-1;
							foreach(XmlNode l in m.Attributes)
							{
								if(l.Name=="id")
									id=int.Parse(l.Value);
								else if(l.Name=="chance")
									chance=float.Parse(l.Value);
							}
							//p.defectsChance[id]=chance;
						}
					}
					
					gunClasses.Add(p);
				}
			}
		}
		Debug.Log("Loaded: "+gunClasses.Count+" gun classes.");
	}
	
	public PlaneTemplate getPlaneTemplate(int id)
	{
		foreach(PlaneTemplate p in planeClasses)
		{
			if(p.id==(int)id)
			{
				PlaneTemplate t = new PlaneTemplate();
				t.classname=p.classname;
				t.description=p.description;
				t.hp=p.hp;
				t.id=p.id;
				t.maxRange=p.maxRange;
				t.lowerSmooth=p.lowerSmooth;
				t.upperSmooth=p.upperSmooth;
				t.maxTurnAngle=p.maxTurnAngle;
				t.minRange=p.minRange;
				t.weapons=p.weapons;
				t.armor=p.armor;
				t.speed=p.speed;
				t.maneuverability=p.maneuverability;
				foreach(GunOnShuttle f in p.guns)
				{
					GunOnShuttle z = new GunOnShuttle();
					z.gunId=f.gunId;
					z.pos=f.pos;
					z.turnAngle=f.turnAngle;
					t.guns.Add(z);
				}
				
//				foreach(int abilId in p.abilities)
//					t.abilities.Add(abilId);
				return t;
			}
		}
		return null;
	}
	
	private void LoadPlaneClasses()
	{
		string name;
		foreach(int Class in Enum.GetValues(typeof(PlaneTemplates)))
		{
			name = Enum.GetName(typeof(PlaneTemplates),Class);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(((TextAsset) Resources.Load("xml/planes/"+name)).text);
			foreach(XmlNode x in doc.ChildNodes)
			{
				if(x.Name=="plane")
				{
					PlaneTemplate p = new PlaneTemplate();
					foreach(XmlNode m in x.Attributes)
					{
						if(m.Name=="id")
							p.id=int.Parse(m.Value);
						else if(m.Name=="hp")
							p.hp=int.Parse(m.Value);
						else if(m.Name=="upperSmooth")
							p.upperSmooth=float.Parse(m.Value);
						else if(m.Name=="lowerSmooth")
							p.lowerSmooth=float.Parse(m.Value);
						else if(m.Name=="classname")
							p.classname=m.Value;
						else if(m.Name=="minRange")
							p.minRange=float.Parse(m.Value);
						else if(m.Name=="maxRange")
							p.maxRange=float.Parse(m.Value);
						else if(m.Name=="maxTurnAngle")
							p.maxTurnAngle=float.Parse(m.Value);
						else if(m.Name=="description")
							p.description=m.Value;
						else if(m.Name=="weapon")
							p.weapons=int.Parse(m.Value);
						else if(m.Name=="armor")
							p.armor=int.Parse(m.Value);
						else if(m.Name=="speed")
							p.speed=int.Parse(m.Value);
						else if(m.Name=="maneuverability")
							p.maneuverability=int.Parse(m.Value);
										
					}
					
					foreach(XmlNode m in x.ChildNodes)
					{
						if(m.Name=="gun")
						{
							GunOnShuttle gos = new GunOnShuttle();
							foreach(XmlNode l in m.Attributes)
							{
								if(l.Name=="id")
									gos.gunId=int.Parse(l.Value);
								else if(l.Name=="pos")
								{
									float xx,yy;
									xx=float.Parse(l.Value.Split(',')[0]);
									yy=float.Parse(l.Value.Split(',')[1]);
									gos.pos=new Vector2(xx,yy);
								}
								else if(l.Name=="turnAngle")
									gos.turnAngle=float.Parse(l.Value);
							}
							p.guns.Add(gos);
						}
						else if(m.Name=="ability")
						{
							int id=-1;
							foreach(XmlNode l in m.Attributes)
							{
								if(l.Name=="id")
									id=int.Parse(l.Value);
							}
							p.abilities.Add(id);
						}
					}
					planeClasses.Add(p);
				}
			}
		}
		Debug.Log("Loaded: "+planeClasses.Count+" plane classes.");
	}
	
	public class LevelInfo
	{
		public int num;
		public string levelName,file,description="",hint="";
		public int rankReached=-1;
	}
	
	private void loadNumberGrafics()
	{
		int i=0;
		for(i=0;i<10;i++)
		{
			numbersSprites[i]=Resources.Load<Sprite>("gui/skinsLow/img/"+i);
			numbersSpritesGrey[i]=Resources.Load<Sprite>("gui/skinsLow/img/"+i+"_grey");
		}
	}
	
	public Sprite[] getNumber(int num)
	{
		return new Sprite[] {numbersSprites[num/10],numbersSprites[num%10]};
	}
	
	public Sprite[] getNumberGrey(int num)
	{
		return new Sprite[] {(Sprite)numbersSpritesGrey[num/10],(Sprite)numbersSpritesGrey[num%10]};
	}
	
	public LevelInfo getLevel(int num)
	{
		foreach(LevelInfo l in levelList)
		{
			if(l.num==num)
				return l;
		}
		return null;
	}
	
	public class PlaneTemplate
	{
		public int id;
		public string classname;
		public int hp;
		public float minRange,maxRange,maxTurnAngle;
		public float upperSmooth=1f;
		public float lowerSmooth=1f;
		public string description;
		//STATS
		public int weapons=1;
		public int armor=1;
		public int speed=1;
		public int maneuverability=1;
		
		public ArrayList guns = new ArrayList();
		public ArrayList abilities = new ArrayList();
	}
	
	private void loadLevels()
	{
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(((TextAsset) Resources.Load("levels/levels")).text);
		foreach(XmlNode x in doc.ChildNodes)
		{
			if(x.Name=="levels")
			{
				foreach(XmlNode m in x.ChildNodes)
				{
					if(m.Name=="level")
					{
						LevelInfo li = new LevelInfo();
						foreach(XmlNode l in m.Attributes)
						{
							if(l.Name=="num")
								li.num=int.Parse(l.Value);
							else if(l.Name=="levelName")
								li.levelName=l.Value;
							else if(l.Name=="file")
								li.file=l.Value;
							else if(l.Name=="rankReached")
								li.rankReached=int.Parse(l.Value);
							else if(l.Name=="description")
								li.description=l.Value;
							else if(l.Name=="hint")
								li.hint=l.Value;
						}
						levelList.Add(li);
					}
				}
			}
		}
		Debug.Log("Loaded "+levelList.Count+" levels.");
	}
	
	private void loadCampaigns()
	{
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(((TextAsset) Resources.Load("levels/campaigns")).text);
		foreach(XmlNode x in doc.ChildNodes)
		{
			if(x.Name=="campaigns")
			{
				foreach(XmlNode m in x.ChildNodes)
				{
					if(m.Name=="campaign")
					{
						CampaignInfo li = new CampaignInfo();
						foreach(XmlNode l in m.Attributes)
						{
							if(l.Name=="id")
								li.id=int.Parse(l.Value);
							else if(l.Name=="name")
								li.name=l.Value;
							else if(l.Name=="defaultRank")
								li.defaultRank=l.Value;
							else if(l.Name=="description")
								li.desc=l.Value;
						}
						
						foreach(XmlNode l in m.ChildNodes)
						{
							int levelNum=-1;
							if(l.Name=="level")
							{
								foreach(XmlNode b in l.Attributes)
								{
									if(b.Name=="id")
										levelNum=int.Parse(b.Value);
								}
							}
							li.levels.Add(levelNum);
						}
						
						campaignsList.Add(li);
					}
				}
			}
		}
		Debug.Log("Loaded "+campaignsList.Count+" campaigns.");
	}
}
//	
//	private GUISkin[] abilitySkins = new GUISkin[9];
//	private GUISkin[] abilityDisabledSkins = new GUISkin[9];
//	
//	public static class StarsSettings {
//		public static int oneStar=30;
//		public static int threeStar=60;
//	}
//	
//	
//	private ArrayList planeClasses;
//	private ArrayList gunClasses;
//	private ArrayList levelList;
//	private ArrayList ranksList;
//	private ArrayList campaignsList;
//	private string planeTempFolder="xml/planes";
//	private string gunTempFolder="xml/guns";
//	private string levelsFolder = "levels";
//	private string ranksFolder = "xml";
//	
//	public GUISkin button_level = null;
//	public GUISkin button_level_selected = null;
//	public GUISkin button_level_start = null;
//	public GUISkin button_level_grey;
//	public GUISkin label_level_star = null;
//	public GUISkin mainPopupRichtext = null;
//	public GUISkin none_scroll_skin = null;
//	public GUISkin progressHpSkin=null;
//	public GUISkin statPointGrey,statPointBlue;
//	public GUISkin bg;
//	public GUISkin menu_button_plot,menu_button_help,menu_button;
//	public GUISkin zalp_button,zalp_button_grey;
//	public GUISkin buttonZoomInSkin,buttonZoomOutSkin;
//	public GUISkin buttonPlay,buttonPlayGrey;
//	public GUISkin buttonPrev,buttonNext;
//	public GUISkin buttonPause;
//	public GUISkin buttonRestart,buttonContinue,buttonMenu,buttonNextLevel;
//	public GUISkin popupWindow;
//	public GUISkin gamePausedStyle;
//	public GUISkin gamePausedDescStyle;
//	public GUISkin gamePausedNameStyle;
//	public GUISkin pauseMission,pauseMissionOk,pauseMissionFail;
//	public GUISkin startLarge;
//	public GUISkin rankStyle;
//	public GUISkin arrowRedSkin,arrowBlueSkin;
//	public GUISkin campaigns_bg;
//	public GUISkin mission_bg;
//	public GUISkin company_panel,company_panel_hover;
//	public GUISkin fader;
//	public GUISkin missions_tree;
//	public GUISkin helpplotpopup,infoGamePopup;
//	//TEST
//	
//	public string plotContext,helpContent;
//	
//	private GUISkin[] numbers;
//	private GUISkin[] numbers_grey;
//	
//	public void reloadIcons()
//	{
//		loadNumbersIcons();
//		loadAbilityIcons();
//		loadContents();
//		loadLevelsSkins();
//	}
//	
//	public Templates()
//	{
//		planeClasses=new ArrayList();
//		LoadPlaneClasses();
//		ranksList=new ArrayList();
//		loadRanks();
//		gunClasses=new ArrayList();
//		LoadGunClasses();
//		levelList=new ArrayList();
//		loadLevels();
//		campaignsList=new ArrayList();
//		loadCampaigns();
//		numbers=new GUISkin[10];
//		numbers_grey=new GUISkin[10];
//		loadNumbersIcons();
//		loadAbilityIcons();
//		loadContents();
//		loadLevelsSkins();
//		Loaded();
//	}
//	
//	private void loadLevelsSkins()
//	{
//		button_level = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_level");
//		button_level_selected = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_level_selected");
//		button_level_start = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_level_start");
//		label_level_star = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/label_level_star");
//		mainPopupRichtext = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/main_popup_richtext");
//		none_scroll_skin = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/none_scroll");
//		progressHpSkin=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/progressStyle");
//		statPointBlue=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/statPointBlue");
//		statPointGrey=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/statPointGrey");
//		bg = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/bg");
//		menu_button = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/menu_button");
//		menu_button_help = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/menu_button_help");
//		menu_button_plot = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/menu_button_plot");
//		button_level_grey = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_level_grey");
//		zalp_button = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_zalp");
//		zalp_button_grey = (GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_zalp_grey");
//		buttonZoomInSkin=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_zoomIn");
//		buttonZoomOutSkin=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_zoomOut");
//		buttonPlay=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_play");
//		buttonPlayGrey=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_play_grey");
//		buttonNext=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_next");
//		buttonPrev=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_prev");
//		buttonPause=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_pause");
//		buttonRestart=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_restart");
//		buttonContinue=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_continue");
//		buttonMenu=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_menu");
//		buttonNextLevel=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/button_nextLevel");
//		popupWindow=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/popup_window");
//		gamePausedStyle=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/gamePausedLabel");
//		gamePausedDescStyle=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/gamePauseDesc");
//		gamePausedNameStyle=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/gamePauseName");
//		pauseMission=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/pauseMission");
//		pauseMissionOk=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/pauseMissionOk");
//		pauseMissionFail=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/pauseMissionFail");
//		startLarge=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/largeStar");
//		rankStyle=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/rankStyle");
//		arrowRedSkin=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/arrowRedSkin");
//		arrowBlueSkin=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/arrowBlueSkin");
//		campaigns_bg=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/campaigns_bg");
//		mission_bg=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/mission_bg");
//		company_panel=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/company_panel");
//		company_panel_hover=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/company_panel_hover");
//		fader=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/fader");
//		missions_tree=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/missions_tree_start");
//		helpplotpopup=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/helpplotpopup");
//		infoGamePopup=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/info_game_popup");
//	}
//	
//	public GUISkin[] getNumberIcons(int num, bool grey)
//	{
//		GUISkin first,second;
//		if(grey)
//		{
//			first=numbers_grey[num/10];
//			second=numbers_grey[num%10];
//		}
//		else
//		{
//			first=numbers[num/10];
//			second=numbers[num%10];
//		}
//		return new GUISkin[] {first,second};
//	}
//	
//	private void loadNumbersIcons()
//	{
//		for(int i=0;i<10;i++)
//		{
//			numbers[i]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/numbers/"+i);
//			numbers_grey[i]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/numbers/"+i+"_grey");
//		}
//	}
//	
//	private void loadAbilityIcons()
//	{
//		abilitySkins[0]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_common");
//		abilitySkins[1]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_180");
//		abilitySkins[2]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_360");
//		abilitySkins[3]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_dt");
//		abilitySkins[4]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_gas");
//		abilitySkins[5]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_rocket");
//		abilitySkins[6]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_shield");
//		abilitySkins[7]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_thorpede");
//		abilitySkins[8]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_mines");
//		
//		abilityDisabledSkins[0]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_common");
//		abilityDisabledSkins[1]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_180_grey");
//		abilityDisabledSkins[2]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_360_grey");
//		abilityDisabledSkins[3]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_dt_grey");
//		abilityDisabledSkins[4]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_gas_grey");
//		abilityDisabledSkins[5]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_rocket_grey");
//		abilityDisabledSkins[6]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_shield_grey");
//		abilityDisabledSkins[7]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_thorpede_grey");
//		abilityDisabledSkins[8]=(GUISkin) Resources.Load("gui/"+Templates.ResolutionProblems.getSkinsFolder()+"/ability_mines_grey");
//	}
//	
//	private void loadContents()
//	{
//		XmlDocument doc = new XmlDocument();
//		doc.LoadXml(((TextAsset) Resources.Load("xml/gui_content/main_menu")).text);
//		foreach(XmlNode x in doc.ChildNodes)
//		{
//			if(x.Name=="contents")
//			{
//				foreach(XmlNode m in x.ChildNodes)
//				{
//					if(m.Name=="content")
//					{
//						string name="",text="";
//						foreach(XmlNode l in m.Attributes)
//						{
//							if(l.Name=="name")
//								name=l.Value;
//							else if(l.Name=="text")
//								text=l.Value;
//						}
//						if(name=="plot")
//							plotContext=text;
//						else if(name=="help")
//							helpContent=text;
//					}
//				}
//			}
//		}
//	}
//	

//	
//	public GUISkin getAbilityIcon(int id)
//	{
//		return abilitySkins[id+1];
//	}
//	
//	public GUISkin getAbilityIcon(Abilities.AbilityType id)
//	{
//		return abilitySkins[((int)id)+1];
//	}
//	
//	public GUISkin getAbilityIconGrey(int id)
//	{
//		return abilityDisabledSkins[id+1];
//	}
//	
//	public GUISkin getAbilityIconGrey(Abilities.AbilityType id)
//	{
//		return abilityDisabledSkins[((int)id)+1];
//	}
//	
//	void Loaded()
//	{
//		GameStorage.getInstance().allReady=true;
//	}
//	
//	public class Rank
//	{
//		public int id;
//		public string name;
//		// maybe ico
//	}
//	

//	
//	public GunTemplate getGunTemplate(GunTemplates id)
//	{
//		foreach(GunTemplate p in gunClasses)
//		{
//			if(p.id==(int)id)
//				return p;
//		}
//		return null;
//	}
//	
//	public Rank getRank(int id)
//	{
//		foreach(Rank p in ranksList)
//		{
//			if(p.id==(int)id)
//				return p;
//		}
//		return null;
//	}
//	
//	public ArrayList getCampaigns()
//	{
//		return campaignsList;
//	}
//	
	
//	
//	public ArrayList getAllLevels()
//	{
//		return levelList;
//	}
//	
	
//	
//	private void loadRanks()
//	{
//		XmlDocument doc = new XmlDocument();
//		doc.LoadXml(((TextAsset) Resources.Load(ranksFolder+"/ranks")).text);
//		foreach(XmlNode x in doc.ChildNodes)
//		{
//			if(x.Name=="ranks")
//			{
//				foreach(XmlNode m in x.ChildNodes)
//				{
//					if(m.Name=="rank")
//					{
//						Rank li = new Rank();
//						foreach(XmlNode l in m.Attributes)
//						{
//							if(l.Name=="id")
//								li.id=int.Parse(l.Value);
//							else if(l.Name=="name")
//								li.name=l.Value;
//						}
//						ranksList.Add(li);
//					}
//				}
//			}
//		}
//		Debug.Log("Loaded "+ranksList.Count+" ranks.");
//	}
//	
//	
	
//	
	
//	
//	public static class ResolutionProblems {
//		public static float Ethalon=1352;
//		public static float EthalonH=743;
//		private static float mainMenuFontEth=36;
//		private static float mainMenuPaddingTopEth=35;
//		
//		//levels
//		private static float levelsButtonSizeEth=96;
//		private static float levelsStartPositinEth=50;
//		private static float levelsNumberHEth=58,levelsNumberWEth=50;
//		private static float starOffsetXEth=12,starOffsetYEth=58,thirdStarYOffsetEth=5;
//		private static float levelsNumberOffsetX=15,levelsNumberOffsetY=13;
//		private static float starSize=28;
//		private static float levelButtonStartW=98;
//		private static float levelButtonStartH=29;
//		private static float off=-55;
//		
//		//PAUSE/WIN/LOSE
//		private static float pauseBoxH=600;
//		private static float pauseButtonW=136,pauseButtonH=40,pauseButtonOffset=17,pauseDopWidth=60,pauseOffsetX=10;
//		private static int gamePauseFontSize=22, pauseDescFontSize=14, pauseNameFontSize=15,reachedRankFontSize=16;
//		private static float missionH=50,missionW=200,accH=50,accW=300,failH=70,failW=250,largeStarSize=70;
//		
//		
//		//Game phase
//		private static float abilIconOffset=100;
//		private static float abilIconSize=80;
//		private static float popupInfoBannerOffset=40;
//		private static float popupInfoBannerWidth=200;
//		private static float popupInfoBannerHpBannerWidth=143;
//		private static float popupInfoBannerAbilSize=32;
//		private static float popupInfoBannerPointSize=16;
//		
//		private static int company_panel_fontSize=28;
//		private static int company_panel_label_fontSize=16;
//		private static float company_panel_h=180,company_panel_offset=30;
//		private static float company_panel_backButtonH=60,company_panel_backButtonW=205;
//		
//		private static float missionTreeLabelH=60,missionTreeLabelW=200,missionTreeLabelOffset=190,levBetweenDist=-1,levOf=35;
//		
//		
//		public static int getMainMenuFontSize(float swidth)
//		{
//			return (int)(mainMenuFontEth*swidth/Ethalon);
//		}
//		
//		public static int getMainMenuPaddingTop(float swidth)
//		{
//			return (int)(mainMenuPaddingTopEth*swidth/Ethalon);
//		}
//		
//		public static float getLevelsButtonSize(float swidth)
//		{
//			return levelsButtonSizeEth*swidth/Ethalon;
//		}
//		
//		public static float getLevelsStartPosition(float swidth)
//		{
//			return levelsStartPositinEth*swidth/Ethalon;
//		}
//		
//		public static float getLevelsNumberH(float swidth)
//		{
//			return levelsNumberHEth*swidth/Ethalon;
//		}
//		
//		public static float getLevelsNumberW(float swidth)
//		{
//			return levelsNumberWEth*swidth/Ethalon;
//		}
//		
//		public static float getLevelsStarOffsetX(float swidth)
//		{
//			return starOffsetXEth*swidth/Ethalon;
//		}
//		
//		public static float getLevelsStarOffsetY(float swidth)
//		{
//			return starOffsetYEth*swidth/Ethalon;
//		}
//		
//		public static float getLevelsThirdstarOffsetY(float swidth)
//		{
//			return thirdStarYOffsetEth*swidth/Ethalon;
//		}
//		
//		public static float getLevelsNumberOffsetX(float swidth)
//		{
//			return levelsNumberOffsetX*swidth/Ethalon;
//		}
//		
//		public static float getLevelsNumberOffsetY(float swidth)
//		{
//			return levelsNumberOffsetY*swidth/Ethalon;
//		}
//		
//		public static float getLevelsStarSize(float swidth)
//		{
//			return starSize*swidth/Ethalon;
//		}
//		
//		public static float getLevelsOff(float swidth)
//		{
//			return off*swidth/Ethalon;
//		}
//		
//		public static float getActionAbilityOffset(float swidth)
//		{
//			return abilIconOffset*swidth/Ethalon;
//		}
//		
//		public static float getActionAbilitySize(float swidth)
//		{
//			return abilIconSize*swidth/Ethalon;
//		}
//		
//		public static float getLevelButtonStartW(float swidth)
//		{
//			return levelButtonStartW*swidth/Ethalon;
//		}
//		
//		public static float getLevelButtonStartH(float swidth)
//		{
//			return levelButtonStartH*swidth/Ethalon;
//		}
//		
//		public static float getPauseButtonStartW(float swidth)
//		{
//			return pauseButtonW*swidth/Ethalon;
//		}
//		
//		public static float getPauseButtonStartH(float swidth)
//		{
//			return pauseButtonH*swidth/Ethalon;
//		}
//		
//		public static float getPauseButtonOffset(float swidth)
//		{
//			return pauseButtonOffset*swidth/Ethalon;
//		}
//		
//		public static float getPauseButtonDopWidth(float swidth)
//		{
//			return pauseDopWidth*swidth/Ethalon;
//		}
//		
//		public static float getPauseButtonOffsetX(float swidth)
//		{
//			return pauseOffsetX*swidth/Ethalon;
//		}
//		
//		public static float getPauseButtonBoxH(float swidth)
//		{
//			return pauseBoxH*swidth/Ethalon;
//		}
//		
//		public static int getPauseMenuFontSize(float swidth)
//		{
//			return (int)(gamePauseFontSize*swidth/Ethalon);
//		}
//		
//		public static int getPauseDescFontSize(float swidth)
//		{
//			return (int)(pauseDescFontSize*swidth/Ethalon);
//		}
//		
//		public static int getPauseNameFontSize(float swidth)
//		{
//			return (int)(pauseNameFontSize*swidth/Ethalon);
//		}
//		
//		public static int getPauseReachedRankFontSize(float swidth)
//		{
//			return (int)(reachedRankFontSize*swidth/Ethalon);
//		}
//		
//		public static float getPauseMissionH(float swidth)
//		{
//			return missionH*swidth/Ethalon;
//		}
//		
//		public static float getPauseMissionW(float swidth)
//		{
//			return missionW*swidth/Ethalon;
//		}
//		
//		public static float getPauseAccH(float swidth)
//		{
//			return accH*swidth/Ethalon;
//		}
//		
//		public static float getPauseAccW(float swidth)
//		{
//			return accW*swidth/Ethalon;
//		}
//		
//		public static float getPauseFailH(float swidth)
//		{
//			return failH*swidth/Ethalon;
//		}
//		
//		public static float getPauseFailW(float swidth)
//		{
//			return failW*swidth/Ethalon;
//		}
//		
//		public static float getPauseLargeStarSize(float swidth)
//		{
//			return largeStarSize*swidth/Ethalon;
//		}
//		
//		public static float getPopupBannerOffset(float swidth)
//		{
//			return popupInfoBannerOffset*swidth/Ethalon;
//		}
//		
//		public static float getPopupBannerWidth(float swidth)
//		{
//			return popupInfoBannerWidth*swidth/Ethalon;
//		}
//		
//		public static float getPopupBannerHpWidth(float swidth)
//		{
//			return popupInfoBannerHpBannerWidth*swidth/Ethalon;
//		}
//		
//		public static float getPopupBannerPointSize(float swidth)
//		{
//			return popupInfoBannerPointSize*swidth/Ethalon;
//		}
//		
//		public static float getPopupBannerAbilSize(float swidth)
//		{
//			return popupInfoBannerAbilSize*swidth/Ethalon;
//		}
//		
//		public static int getCompanyPanelFontSize(float swidth)
//		{
//			return (int)(company_panel_fontSize*swidth/Ethalon);
//		}
//		
//		public static int getCompanyPanelLabelFontSize(float swidth)
//		{
//			return (int)(company_panel_label_fontSize*swidth/Ethalon);
//		}
//		
//		public static float getCompanyPanelH(float swidth)
//		{
//			return company_panel_h*swidth/EthalonH;
//		}
//		
//		public static float getCompanyPanelBackButtonH(float swidth)
//		{
//			return company_panel_backButtonH*swidth/EthalonH;
//		}
//		
//		public static float getCompanyPanelBackButtonW(float swidth)
//		{
//			return company_panel_backButtonW*swidth/Ethalon;
//		}
//		
//		public static float getCompanyPanelOffset(float swidth)
//		{
//			return company_panel_offset*swidth/EthalonH;
//		}
//		
//		public static float getMissionTreeLabelH(float swidth)
//		{
//			return missionTreeLabelH*swidth/Ethalon;
//		}
//		
//		public static float getMissionTreeLabelW(float swidth)
//		{
//			return missionTreeLabelW*swidth/Ethalon;
//		}
//		
//		public static float getMissionTreeLabelOffset(float swidth)
//		{
//			return missionTreeLabelOffset*swidth/Ethalon;
//		}
//		
//		public static float getLevOf(float swidth)
//		{
//			return levOf*swidth/Ethalon;
//		}
//		
//		public static float getLevBetween(float swidth)
//		{
//			return levBetweenDist*swidth/Ethalon;
//		}
//		
//		public static string getSkinsFolder()
//		{
//			if(Screen.width>Ethalon)
//				return "skinsLow";
//			else
//				return "skinsLow";
//		}
//	}
//}
