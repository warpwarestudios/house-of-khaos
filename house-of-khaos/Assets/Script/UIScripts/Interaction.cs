using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	
	UILabel font;
	GameObject interactLabel;
	
	// Use this for initialization
	void Start () {
		interactLabel = GameObject.Find("InteractLabel");
		font = interactLabel.GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.color = Color.white;
	}
	
	public void OnLookEnter()
	{
		renderer.material.color = Color.red;
		font.text = "'E' to interact";
	}
}
