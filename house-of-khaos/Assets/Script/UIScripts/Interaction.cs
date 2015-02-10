using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	
	public UILabel itemLabelDisplay;
	public UILabel doorOpenLabelDisplay;
	public UILabel doorCloseLabelDisplay;
	private GameObject interactLabel;
	private GameObject uiRoot;
	private GameObject doorOpenLabel;
	private GameObject doorCloseLabel;
	private GameObject itemLabel;
	public bool selected;
	public enum TestEnum{Item, Door};
	private string uiText = "";
	
	private ItemWheelControl itemControl;
	private GameObject itemWheel;
	
	//This is what you need to show in the inspector.
	public TestEnum interactionType;
	
	// Use this for initialization
	void Start () {

		uiRoot = GameObject.Find("UI Root");
		interactLabel = uiRoot.transform.FindChild("InteractLabel").gameObject;
		doorOpenLabel = interactLabel.transform.FindChild("DoorOpenInteractLabel").gameObject;
		doorCloseLabel = interactLabel.transform.FindChild("DoorCloseInteractLabel").gameObject;
		itemLabel = interactLabel.transform.FindChild("ItemInteractLabel").gameObject;
		itemWheel = uiRoot.transform.FindChild ("ItemWheel").gameObject;
		itemControl = itemWheel.GetComponent<ItemWheelControl>();
		doorOpenLabelDisplay = doorOpenLabel.GetComponent<UILabel>();
		doorCloseLabelDisplay = doorCloseLabel.GetComponent<UILabel>();
		itemLabelDisplay = itemLabel.GetComponent<UILabel>();
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
				NGUITools.SetActive(doorOpenLabel, true);
				
				if (Input.GetKeyDown(KeyCode.E))
				{
					//animate the door to be opened
				}
				
				//if door is closed
				
			}
		}
		if(interactionType == TestEnum.Item)
		{
			if (selected == true)
			{
				NGUITools.SetActive(itemLabel, true);
				
				if (Input.GetKeyDown(KeyCode.E))
				{
					itemControl.SetItem(this.gameObject);
					this.gameObject.transform.position = new Vector3 (0, -1000, 0);		
				}
			}
		}

		if (!selected) 
		{
			NGUITools.SetActive(doorOpenLabel, false);
			NGUITools.SetActive(doorCloseLabel, false);
			NGUITools.SetActive(itemLabel, false);
		}
		selected = false;
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
