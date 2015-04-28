using UnityEngine;
using System.Collections;

public class MessageUI : MonoBehaviour {
	
	public GameObject messageBox;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Message(string title, string message)
	{
		Instantiate(messageBox);
		this.transform.parent = messageBox.transform;
		messageBox.gameObject.transform.FindChild("MessageLabel").GetComponent<UILabel>().text = title;
		messageBox.gameObject.transform.FindChild("Message").GetComponent<UILabel>().text = message;
		
	}
	
	public void OkButton()
	{
		Destroy(messageBox);
	}

}
