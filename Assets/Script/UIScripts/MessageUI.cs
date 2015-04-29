﻿using UnityEngine;
using System.Collections;

public class MessageUI : MonoBehaviour {
	
	public GameObject messageBox;
	public GameObject mesBox;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Message(string title, string message)
	{
		mesBox = Instantiate(messageBox) as GameObject;
		this.transform.parent = mesBox.transform;
		mesBox.gameObject.transform.FindChild("MessageLabel").GetComponent<UILabel>().text = title;
		mesBox.gameObject.transform.FindChild("Message").GetComponent<UILabel>().text = message;
		
	}
	
	public void OkButton()
	{
		Destroy(mesBox);
	}

}