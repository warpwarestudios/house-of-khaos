using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public RaycastHit hit;
	public float distanceToSee = 5f;
	public GameObject interactLabel;
	// Use this for initialization
	void Start () {
		interactLabel = GameObject.Find("InteractLabel");
		
		Cursor.visible = false;
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Debug.DrawRay(this.transform.position, this.transform.forward * distanceToSee, Color.magenta);
		if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, distanceToSee))
		{
			if(hit.collider.tag == "Interactable")
			{
				if(hit.collider.GetComponent<Interaction>().interactionType == Interaction.TestEnum.Item)
				{
					interactLabel.GetComponent<UILabel>().text = "'E' to pickup Item";
					Debug.Log("'E' to pick up Item");
					
					if (Input.GetKeyDown(KeyCode.E))
					{
						hit.collider.GetComponent<Interaction>().Item();
					}
				}
//				if(hit.collider.GetComponent<Interaction>().interactionType == Interaction.TestEnum.Door)
//				{
//					//if door open
//					interactLabel.GetComponent<UILabel>().text = "'E' to openDoor";
//					//if door closed
//				}
			}
		}
		else
		{
			interactLabel.GetComponent<UILabel>().text = "";
		}
	}	
}