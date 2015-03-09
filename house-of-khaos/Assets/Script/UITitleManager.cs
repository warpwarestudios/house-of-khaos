using UnityEngine;
using System.Collections;

public class UITitleManager : MonoBehaviour {

	public GameObject titleCamera;
	public GameObject lobbyCamera;
	
	void Start() 
	{
		titleCamera.SetActive(true);
		lobbyCamera.SetActive(false);
	}
	
	public void SwitchToLobby()
	{
		titleCamera.SetActive(false);
		lobbyCamera.SetActive (true);
	}
	
	public void SwitchToTitle()
	{
		titleCamera.SetActive(true); 
		lobbyCamera.SetActive(false);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
}
