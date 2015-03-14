using UnityEngine;
using System.Collections;

public class HotbarManager : MonoBehaviour {

	public GameObject [] hotbars = new GameObject[7];
	public GameObject [] items = new GameObject[7];
	public GameObject empty;
	
	public int hotbarNumber = 0;
	
	public GameObject player;
	
	void Start()
	{
		for (int i = 0; i < items.Length; i++ )
		{
			items[i] = empty;
			hotbars[i].GetComponent<UIButton>().duration = 1f;
			
		}
	}
	
	void Update()
	{	
		ScrollWheeling();	
	}
	
	void FireButton()
	{
		if(Input.GetButton("Fire1"))
		{
			UseItem();
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
		hotbars[hotbarNumber].GetComponent<UISprite>().spriteName = items[hotbarNumber].GetComponent<Itemization>().SpriteIconName;
		items[hotbarNumber].transform.parent = player.transform;
		items[hotbarNumber].transform.localPosition = new Vector3(0,0,0);
	}
	
	void DropItem()
	{
		Vector3 playerPos = player.transform.position;
		Vector3 playerDirection = player.transform.forward;
		float spawnDistance = 2;	
		Vector3 spawnPos = playerPos + playerDirection*spawnDistance;
		
		items[hotbarNumber].transform.position = spawnPos;
		items[hotbarNumber] = empty;
		hotbars[hotbarNumber].GetComponent<UISprite>().spriteName = empty.GetComponent<Itemization>().SpriteIconName;
		items[hotbarNumber].transform.parent = player.transform;
		
	}
	
	public void UseItem()
	{
		Debug.Log("I use this item" + items[hotbarNumber].GetComponent<Itemization>().ItemName);		
	}
	
	
	void ScrollWheeling()
	{
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			hotbars[hotbarNumber].GetComponent<UIButton>().SetState(UIButtonColor.State.Normal, true); //hovers hot barto false 
			items[hotbarNumber].SetActive(false);
			hotbarNumber++;
			
			if (hotbarNumber > items.Length - 1)
			{
				hotbarNumber = (items.Length - items.Length);
			}
			
			hotbars[hotbarNumber].GetComponent<UIButton>().SetState(UIButtonColor.State.Hover, true); //hovers on hotbar to true
			items[hotbarNumber].SetActive(true);
		}
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			hotbars[hotbarNumber].GetComponent<UIButton>().SetState(UIButtonColor.State.Normal, true); //hovers hot barto false
			items[hotbarNumber].SetActive(false); 
			hotbarNumber--;
			
			if (hotbarNumber < (items.Length - items.Length))
			{
				hotbarNumber = items.Length - 1;
			}
			
			hotbars[hotbarNumber].GetComponent<UIButton>().SetState(UIButtonColor.State.Hover, true); //hovers on hotbar to true
			items[hotbarNumber].SetActive(true);
		}
	}
	
	
}
