using UnityEngine;
using System.Collections;

public class EscapeScreen : MonoBehaviour {
	
	private bool isMenu = false;
	// Use this for initialization
	void Start () {
		GameObject.Find("UI Root").transform.FindChild("EscapeScreen").gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			isMenu = !isMenu;
			if(isMenu)
			{
				GameObject.Find("UI Root").transform.FindChild("EscapeScreen").gameObject.SetActive(true);
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				GameObject.Find("UI Root").transform.FindChild("EscapeScreen").gameObject.SetActive(false);
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
	}
	
    public void LeaveRoom()
    {
    	PhotonNetwork.LeaveRoom();
    	Application.LoadLevel("LobbyScreen");
    }
    
    public void ExitGame()
    {
		PhotonNetwork.LeaveRoom();
		Application.Quit();
    }
    
}
