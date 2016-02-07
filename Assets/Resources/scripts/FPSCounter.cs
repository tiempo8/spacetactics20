using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSCounter : MonoBehaviour {
	public GameObject textForFps;
	float delt=0;
	Text tex;
	string fps = " FPS";
	int value=0;
	
	
	
	void Start()
	{
		tex=textForFps.GetComponent<Text>();
	}
	
	void Update()
	{
		if(delt>=1)
		{
			value=((int)(1.0f / Time.smoothDeltaTime));
			tex.text=value+fps;
			delt=0;
			
		}
		delt+=Time.deltaTime;
	}
}
