using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
 
public class DebugConsole : MonoBehaviour
{
	private Text currentConsole;
	private static DebugConsole inst=null;
	
	void Start()
	{
		currentConsole=gameObject.GetComponent<Text>();
		inst=this;
	}
	
	public static DebugConsole getInstance()
	{
		return inst;
	}
	
	public void Log(string s)
	{
		currentConsole.text="Log: "+s+"\n"+currentConsole.text;
	}
	
	public void Display(string s)
	{
		currentConsole.text=s;
	}
}