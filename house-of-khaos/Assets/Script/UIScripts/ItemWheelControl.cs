using UnityEngine;
using System.Collections;

public class ItemWheelControl : MonoBehaviour {

	public GameObject[] items; //Array of items on the itemwheel
	public int itemNumber = 0; //arrayNumber for item
	public int rotatorNumber = 0; 
	private float lSpeed = 60.0f; //Speed of spin
	private GameObject empty;
	private GameObject player;
	private GameObject uiRoot;
	private GameObject itemButton;
	private UISprite uiSprite;
	private Itemization itemization;
	
	
	void Start () 
	{
		uiRoot = GameObject.Find("UI Root");
		itemButton = uiRoot.transform.FindChild("ItemButton").gameObject;
		uiSprite = itemButton.GetComponent<UISprite>();
		player = GameObject.FindWithTag("Player");
		items = new GameObject[8];
		empty = GameObject.Find("Empty");
		items[0] = empty;
		items[1] = empty;
		items[2] = empty;
		items[3] = empty;
		items[4] = empty;
		items[5] = empty;
		items[6] = empty;
		items[7] = empty;
		//Set Current item to 0
	}
	
	void Update ()
	{
			if(Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				rotatorNumber = (rotatorNumber + 1);
				if (rotatorNumber > items.Length - 1)
				{
					rotatorNumber = (items.Length - items.Length);
				}
				
				itemNumber = rotatorNumber;
				transform.Rotate (Vector3.forward * -45);
				itemization = items[itemNumber].GetComponent<Itemization>();
				uiSprite.spriteName = itemization.SpriteIconName;

			}
			if(Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				rotatorNumber = (rotatorNumber - 1);
				
				if (rotatorNumber < (items.Length - items.Length))
				{
					rotatorNumber = items.Length - 1;
				}
				
				itemNumber = rotatorNumber;
				transform.Rotate (Vector3.forward * 45);
				itemization = items[itemNumber].GetComponent<Itemization>();
				uiSprite.spriteName = itemization.SpriteIconName;
				
			}
	
		if (Input.GetKeyDown(KeyCode.F))
		{
			UseItem();
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			DropItem();
		}
	}
	
	public void SetItem(GameObject newItem)
	{

		for (int i = 0; i < items.Length; i++)
		{
			if(items[i].Equals(empty))
			{
				items[i] = newItem;
				itemization = items[i].GetComponent<Itemization>();
				uiSprite.spriteName = itemization.SpriteIconName;
				break;
			}
		}
			
		
	}
	
	public void DropItem()
	{
		if(!items[itemNumber].Equals(empty))
		{
			items[itemNumber].transform.position = player.transform.position;
			items[itemNumber] = empty;
			itemization = items[itemNumber].GetComponent<Itemization>();
			uiSprite.spriteName = itemization.SpriteIconName;
		}
	}
	
	public void UseItem()
	{
		if(!items[itemNumber].Equals(empty))
		{
			items[itemNumber].transform.parent = player.transform;
			items[itemNumber].transform.position = new Vector3(3, 0, -3);
			items[itemNumber] = empty;
			itemization = items[itemNumber].GetComponent<Itemization>();
			uiSprite.spriteName = itemization.SpriteIconName;
			
			
		}
	}
	
	
}
