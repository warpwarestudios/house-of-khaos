using UnityEngine;
using System.Collections;

public class ItemWheelControl : MonoBehaviour {

	string[] items; //Array of items on the itemwheel
	string currentItem; //current item on showing on itemwheel
	string useButton = "E"; //use
	string pickUpButton = "R"; //pickup
	int itemNumber = 0; //arrayNumber for item 
	float lSpeed = 60.0f; //Speed of spin
	
	
	void Start () 
	{
		
		//Set Current weapon to 0
		//currentItem = items[0];
	}
	
	void Update ()
	{
		
		//Get Input From The Mouse Wheel
		// if mouse wheel gives a positive value add 1 to WeaponNumber
		// if it gives a negative value decrease WeaponNumber with 1
		//if(Input.GetButton("Mouse ScrollWheel"))
		//{
			if(Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				itemNumber = (itemNumber + 1);
				transform.Rotate (Vector3.forward * -60);
			}
			if(Input.GetAxis("Mouse ScrollWheel") < 0)
			{
				itemNumber = (itemNumber - 1);
				transform.Rotate (Vector3.forward * 60);
			}
			//currentItem = items[itemNumber];
		//}
		
	//	if (Input.GetKeyDown(useButton))
	//	{
			//Use Current Item
	//	}
	}
}
