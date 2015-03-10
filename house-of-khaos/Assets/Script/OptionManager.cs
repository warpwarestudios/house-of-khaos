using UnityEngine;
using System.Collections;

public class OptionManager : MonoBehaviour {
	
	public GameObject ResolutionDropDown;
	public GameObject FPSDropDown;
	public GameObject FullScreenToggle;
	
	public GameObject VolumeBar;
	public GameObject MuteToggle;
	
	private bool isFullScreen;
	private int screenHeight;
	private int screenWidth;
	private int fps;
	
	// Use this for initialization
	void Start () {
		isFullScreen = Screen.fullScreen;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		fps = 60;
		
		ResolutionDropDown.GetComponent<UILabel>().text = screenWidth + "x" + screenHeight;
		if(fps == 0)
		{
			FPSDropDown.GetComponent<UILabel>().text = "Unlimited";
		}
		else
		{
			FPSDropDown.GetComponent<UILabel>().text = fps.ToString();
		}
		if(isFullScreen == true)
		{
			FullScreenToggle.GetComponent<UIToggle>().value = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		VolumeControl();
		
		
	}
	
	public void ResolutionAndFPSControl()
	{
		if(FullScreenToggle.GetComponent<UIToggle>().value == true)
		{
			isFullScreen = true;
		}
		else
		{
			isFullScreen = false;
		}
		
		CheckResolution();
		CheckFPS();
		
		Screen.SetResolution(screenWidth, screenHeight, isFullScreen, fps);	
	}
	
	void CheckResolution()
	{
		if(ResolutionDropDown.GetComponent<UILabel>().text == "1680x1050")
		{
			screenWidth = 1680;
			screenHeight = 1050;
		}
		if(ResolutionDropDown.GetComponent<UILabel>().text == "1920x1080")
		{
			screenWidth = 1680;
			screenHeight = 1050;
		}
	}
	
	void CheckFPS()
	{
		if(FPSDropDown.GetComponent<UILabel>().text == "Unlimited")
		{
			fps = 0;
		}
		
		if(FPSDropDown.GetComponent<UILabel>().text == "30")
		{
			fps = 30;
		}
		
		if(FPSDropDown.GetComponent<UILabel>().text == "60")
		{
			fps = 60;
		}
	}
	public void VolumeControl()
	{
		if (MuteToggle.GetComponent<UIToggle>().value == false)
		{	
			AudioListener.volume = VolumeBar.GetComponent<UIScrollBar>().value;
		}
		else
		{
			AudioListener.volume = 0.0f;
		}
	}
}
