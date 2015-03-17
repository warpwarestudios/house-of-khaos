using UnityEngine;
using System.Collections;

public class InteractionLabel : MonoBehaviour {

	// Use this for initialization
	UILabel font;
	public string TextChange {get; set;}
	
	void Start()
	{
		font = this.gameObject.GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		font.text = TextChange;
	}

	
}
