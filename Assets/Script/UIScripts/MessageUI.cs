using UnityEngine;
using System.Collections;

public class MessageUI : MonoBehaviour {
	
	GameObject messageBox;
	GameObject lobbyScreen;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Message(string title, string body)
	{
		Instantiate(messageBox);
		lobbyScreen.transform.parent = messageBox.transform;
		
	}
}
