using UnityEngine;
using System.Collections;

public class UITitleManager : MonoBehaviour {

	public GameObject titleCamera;
	public GameObject lobbyCamera;
	public GameObject optionCamera;
	public GameObject gameLobbyCamera;
	
	public GameObject videoTab;
	public GameObject soundTab;
	
	void Start() 
	{
		titleCamera.SetActive(true);
		lobbyCamera.SetActive(false);
		optionCamera.SetActive(false);
		gameLobbyCamera.SetActive(false);
	}
	
	public void SwitchTitleToLobby()
	{
		titleCamera.SetActive(false);
		lobbyCamera.SetActive (true);
	}
	
	public void SwitchLobbyToTitle()
	{
		titleCamera.SetActive(true); 
		lobbyCamera.SetActive(false);
	}
	
	public void SwitchTitleToOptions()
	{
		titleCamera.SetActive(false);
		optionCamera.SetActive(true);
		soundTab.SetActive(false);
	}
	
	public void SwitchOptionsToTitle()
	{
		titleCamera.SetActive(true);
		optionCamera.SetActive(false);
	}
	
	public void OptionsSwitchToVideoTab()
	{
		videoTab.SetActive(true);
		soundTab.SetActive(false);
	}
	
	public void OptionsSwitchToSoundTab()
	{
		videoTab.SetActive(false);
		soundTab.SetActive(true);
	}

	public void SwitchToGameLobby()
	{
		lobbyCamera.SetActive (false);
		gameLobbyCamera.SetActive(true);
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
}
