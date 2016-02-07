using UnityEngine;
using System.Collections;

public class TestGuiScript : MonoBehaviour {

	public void Go()
	{
		Application.LoadLevel(1);
	}
	
	public void LoadGS()
	{
		Debug.Log("Load");
	}
}
