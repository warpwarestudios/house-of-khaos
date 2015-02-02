using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	
	UILabel font;
	GameObject interactLabel;
	GameObject uiRoot;
	bool selected;
	public enum TestEnum{Item, Door};
	
	//This is what you need to show in the inspector.
	public TestEnum interactionType;

	
//	Crosshair crossHair;
//	GameObject FPC;
//	GameObject mainCamera;
//	
	// Use this for initialization
	void Start () {
		uiRoot = GameObject.Find("UI Root");
		interactLabel = uiRoot.transform.FindChild("InteractLabel").gameObject;
		font = interactLabel.GetComponent<UILabel>();
		
//		FPC = GameObject.Find ("First Person Controller");
//		mainCamera = FPC.transform.FindChild("Main Camera").gameObject;
//		crossHair = mainCamera.GetComponent<Crosshair>();
	}
	
	// Update is called once per frame
	void Update () {
		Interact();
	}
	
	void Interact()
	{
		if(interactionType == TestEnum.Item)
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
		else if (interactionType == TestEnum.Door)
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
