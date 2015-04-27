using UnityEngine;
using System.Collections;

public class HotbarManager : MonoBehaviour {

	public GameObject [] hotbars = new GameObject[7];
	public GameObject [] items = new GameObject[7];
	public GameObject empty;
	
	public GameObject currentItem;
	
	public int hotbarNumber = 0;
	
	public GameObject player;
	
	public GameObject status;

	
	void Start()
	{
		
		empty = GameObject.Find("EmptySlot");
		status = GameObject.Find ("Status");
		
		for (int i = 0; i < items.Length; i++ )
		{
			hotbars[i] = GameObject.Find("Hotbar " + (i + 1));
		}
		
		
		for (int i = 0; i < items.Length; i++ )
		{
			items[i] = empty;
			hotbars[i].GetComponent<UIButton>().duration = 1f;
			hotbars[i].GetComponent<UISprite>().spriteName = items[i].GetComponent<Itemization>().skillIconName;
		}
		
		hotbars[0].GetComponent<UIButton>().SetState(UIButtonColor.State.Normal, true);
		currentItem = hotbars[0]; 
	}
	
	void Update()
	{	
		if(player == null)
			player = GameObject.FindWithTag("Player");
	
	
		ScrollWheeling();
		for (int i = 0; i < items.Length; i++ )
		{
			hotbars[i].GetComponent<UISprite>().spriteName = items[i].GetComponent<Itemization>().skillIconName;
		}
		
		
		FireButton();
		DropButton();	
	}
	
	void FireButton()
	{
		if(Input.GetButton("Fire1"))
		{
			UseItem();
		}
	}
	
	void DropButton()
	{
		if(Input.GetKey(KeyCode.R))
		{
			DropItem();
		}
	}
	
	public void SetItem(GameObject item)
	{
		if(items[hotbarNumber].Equals(empty))
		{
			EquipItem(item);
		}
		else
		{
			DropItem();
			EquipItem(item);
		}
		
		
	}
	
	void EquipItem(GameObject item)
	{
			items[hotbarNumber] = item;
			hotbars[hotbarNumber].GetComponent<UISprite>().spriteName = items[hotbarNumber].GetComponent<Itemization>().skillIconName;
			items[hotbarNumber].transform.parent = player.transform.FindChild("Main Camera").transform;
			items[hotbarNumber].transform.localPosition = items[hotbarNumber].GetComponent<Itemization>().position;
			items[hotbarNumber].transform.localRotation = items[hotbarNumber].GetComponent<Itemization>().rotation;
			items[hotbarNumber].transform.localScale = items[hotbarNumber].GetComponent<Itemization>().scale;
			items[hotbarNumber].GetComponent<Rigidbody>().useGravity = false;
			items[hotbarNumber].GetComponent<MeshCollider>().enabled = false;

	}
	
	void DropItem()
	{
		Vector3 playerPos = player.transform.position;
		Vector3 playerDirection = player.transform.forward;
		float spawnDistance = 2;	
		Vector3 spawnPos = playerPos + playerDirection*spawnDistance;
		
		items[hotbarNumber].transform.parent = null;
		items[hotbarNumber].transform.position = spawnPos;
		items[hotbarNumber].GetComponent<Rigidbody>().useGravity = true;
		items[hotbarNumber].GetComponent<MeshCollider>().enabled = true;
		items[hotbarNumber] = empty;
		hotbars[hotbarNumber].GetComponent<UISprite>().spriteName = empty.GetComponent<Itemization>().skillIconName;
		
	}
	
	public void UseItem()
	{
			GetComponent<SkillManager>().Invoke(items[hotbarNumber].GetComponent<Itemization>().skillName, 0);
	}
	
	
	void ScrollWheeling()
	{
		
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			hotbars[hotbarNumber].GetComponent<UIButton>().SetState(UIButtonColor.State.Normal, true); //hovers hot barto false
			items[hotbarNumber].GetComponent<ItemVisible>().HideChildren();
			currentItem = hotbars[hotbarNumber];
			status.GetComponent<UILabel>().text = items[hotbarNumber].GetComponent<Itemization>().itemName + "\n" + items[hotbarNumber].GetComponent<Itemization>().duraSaniAmmoRemaining + "/" + items[hotbarNumber].GetComponent<Itemization>().duraSaniAmmo;
			hotbarNumber++;
			
			
			if (hotbarNumber > items.Length - 1)
			{
				hotbarNumber = (items.Length - items.Length);
			}
			
			hotbars[hotbarNumber].GetComponent<UIButton>().SetState(UIButtonColor.State.Hover, true); //hovers on hotbar to true
			items[hotbarNumber].GetComponent<ItemVisible>().ShowChildren();
			currentItem = hotbars[hotbarNumber];
			status.GetComponent<UILabel>().text = items[hotbarNumber].GetComponent<Itemization>().itemName + "\n" + items[hotbarNumber].GetComponent<Itemization>().duraSaniAmmoRemaining + "/" + items[hotbarNumber].GetComponent<Itemization>().duraSaniAmmo;
			
			
		}
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			hotbars[hotbarNumber].GetComponent<UIButton>().SetState(UIButtonColor.State.Normal, true); //hovers hot barto false
			items[hotbarNumber].GetComponent<ItemVisible>().HideChildren();
			currentItem = hotbars[hotbarNumber];
			status.GetComponent<UILabel>().text = items[hotbarNumber].GetComponent<Itemization>().itemName + "\n" + items[hotbarNumber].GetComponent<Itemization>().duraSaniAmmoRemaining + "/" + items[hotbarNumber].GetComponent<Itemization>().duraSaniAmmo;			
			hotbarNumber--;
			
			if (hotbarNumber < (items.Length - items.Length))
			{
				hotbarNumber = items.Length - 1;
			}
			
			hotbars[hotbarNumber].GetComponent<UIButton>().SetState(UIButtonColor.State.Hover, true); //hovers on hotbar to true
			items[hotbarNumber].GetComponent<ItemVisible>().ShowChildren();
			currentItem = hotbars[hotbarNumber];
			status.GetComponent<UILabel>().text = items[hotbarNumber].GetComponent<Itemization>().itemName + "\n" + items[hotbarNumber].GetComponent<Itemization>().duraSaniAmmoRemaining + "/" + items[hotbarNumber].GetComponent<Itemization>().duraSaniAmmo;
		}
	}
	
	
}
