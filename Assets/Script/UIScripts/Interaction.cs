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
	
}