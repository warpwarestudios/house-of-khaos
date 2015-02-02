using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	
	UILabel font;
	GameObject interactLabel;
	GameObject uiRoot;
	bool selected; 
	
	// Use this for initialization
	void Start () {
		uiRoot = GameObject.Find("UI Root");
		interactLabel = uiRoot.transform.FindChild("InteractLabel").gameObject;
		font = interactLabel.GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
		Interact();
	}
	
	void Interact()
	{
		if(this.gameObject.tag == "Item")
		{
			if (getSelected() == true)
			{
				this.renderer.material.color = Color.red;
				font.text = "'E' to Use Item";
				if (Input.GetKeyDown(KeyCode.E))
				{
					this.rigidbody.AddForce(Vector3.up * 1000);
				}
				
			}
			else
			{
				this.renderer.material.color = Color.white;
				font.text = "";
			}
		}
		else if (this.gameObject.tag == "Door")
		{
			if (getSelected() == true)
			{
				//If door is open
				font.text = "'E' to open Door";
				if (Input.GetKeyDown(KeyCode.E))
				{
					//animate the door to be opened
				}
				
				//if door is closed
				
			}
			else
			{
				
				font.text = "";
			}
		}
	}
	
	
	
	public void OnLookEnter()
	{
		setSelected(true);
		Debug.Log("Enter the OnlookEnterScript");
		Debug.Log("The Cube Should be red");
		
		Debug.Log("E to interact should be popped");
	}
	
	
	public void setSelected(bool select)
	{
		selected = select;
	}
	
	public bool getSelected()
	{
		return selected;
	}
}
