using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	
	public UILabel font;
	private GameObject interactLabel;
	private GameObject uiRoot;
	public bool selected;
	public enum TestEnum{Item, Door};
	
	//This is what you need to show in the inspector.
	public TestEnum interactionType;
	
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
		if (interactionType == TestEnum.Door)
		{
			if (selected == true)
			{
				//If door is open
				this.renderer.material.color = Color.yellow;
				font.text = "'E' to open Door";
				Debug.Log(font.text);
				if (Input.GetKeyDown(KeyCode.E))
				{
					//animate the door to be opened
				}
				
				//if door is closed
				
			}
			else
			{
				this.renderer.material.color = Color.blue;
			}
		}
		if(interactionType == TestEnum.Item)
		{
			if (selected == true)
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
			}
		}

		if (!selected) 
		{
			font.text = "";
		}
		//selected = false;
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
