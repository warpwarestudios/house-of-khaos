using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	
	public bool selected;
	
	private GameObject interactLabel;
	private GameObject itemLabel;
	private string uiText = "";
	
	private HotbarManager itemControl;
	private GameObject hotBar;
	
	//This is what you need to show in the inspector.
	public enum TestEnum{Item, Door};
	public TestEnum interactionType;
	
	// Use this for initialization
	void Start () {
		hotBar = GameObject.Find("HotbarManager");
		itemControl = hotBar.GetComponent<HotbarManager>();
	}
	
	// Update is called once per frame
	void Update () {
		//Interact();
	}
	
	public void Item()
	{
		itemControl.SetItem(this.gameObject);
	}
	
	public void Door()
	{
		//Insert door animation here
	}
	
//	void Interact()
//	{
//		if (interactionType == TestEnum.Door)
//		{
//			if (selected == true)
//			{
//				//If door is open
//				interactLabel.GetComponent<UILabel>().text = "'E' to open door";
//				
//				if (Input.GetKeyDown(KeyCode.E))
//				{
//					//animate the door to be opened
//				}
//				
//				//if door is closed
//				interactLabel.GetComponent<UILabel>().text = "'E' to close door";
//			}
//		}
//		if(interactionType == TestEnum.Item)
//		{
//			if (selected == true)
//			{
//				interactLabel.GetComponent<UILabel>().text = "'E' to pickup item";
//				
//				if (Input.GetKeyDown(KeyCode.E))
//				{
//					itemControl.SetItem(this.gameObject);
//				}
//			}
//		}
//
//		//if (!selected) 
//		//{
//			interactLabel.GetComponent<UILabel>().text = "";
//		//}
//	//	selected = false;
//	}
//	
//	
//	
////	public void OnLookEnter()
////	{
////	 	selected = !selected;
////	}
////	
//	
//	public void setSelected()
//	{
//		selected = !selected;
//	}
//	
////	public bool getSelected()
////	{
////		return selected;
////	}
}
