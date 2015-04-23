using UnityEngine;
using System.Collections;

public class EscapeScreen : MonoBehaviour {
	
	
	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKey(KeyCode.Escape))
		{
			GameObject.Find("EscapeScreen").gameObject.SetActive(true);
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		
		}
		else
		{
			GameObject.Find("EscapeScreen").gameObject.SetActive(true);
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}
	
    public void LeaveGame()
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
