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
	
	private HotbarManager itemControl;
	private GameObject hotBar;
	
	//This is what you need to show in the inspector.
	public TestEnum interactionType;
	
	// Use this for initialization
	void Start () {

		uiRoot = GameObject.Find("UI Root");
		interactLabel = uiRoot.transform.FindChild("InteractLabel").gameObject;
//		doorOpenLabel = interactLabel.transform.FindChild("DoorOpenInteractLabel").gameObject;
//		doorCloseLabel = interactLabel.transform.FindChild("DoorCloseInteractLabel").gameObject;
//		itemLabel = interactLabel.transform.FindChild("ItemInteractLabel").gameObject;
		hotBar = GameObject.Find("HotbarManager");
		itemControl = hotBar.GetComponent<HotbarManager>();
//		doorOpenLabelDisplay = doorOpenLabel.GetComponent<UILabel>();
//		doorCloseLabelDisplay = doorCloseLabel.GetComponent<UILabel>();
//		itemLabelDisplay = itemLabel.GetComponent<UILabel>();
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
				interactLabel.GetComponent<UILabel>().text = "'E' to open door";
				
				if (Input.GetKeyDown(KeyCode.E))
				{
					//animate the door to be opened
				}
				
				//if door is closed
				interactLabel.GetComponent<UILabel>().text = "'E' to close door";
			}
		}
		if(interactionType == TestEnum.Item)
		{
			if (selected == true)
			{
				interactLabel.GetComponent<UILabel>().text = "'E' to pickup item";
				
				if (Input.GetKeyDown(KeyCode.E))
				{
					itemControl.SetItem(this.gameObject);
				}
			}
		}

		if (!selected) 
		{
			interactLabel.GetComponent<UILabel>().text = "";
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
