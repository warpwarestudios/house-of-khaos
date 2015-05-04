using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public RaycastHit hit;
	public float distanceToSee = 5f;
	public GameObject interactLabel;

	private Brighten lastObject;

	// Use this for initialization
	void Start () {
		interactLabel = GameObject.Find("InteractLabel");
		lastObject = null;
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.DrawRay(this.transform.position, this.transform.forward * distanceToSee, Color.magenta);
		if(Physics.Raycast(this.transform.position, this.transform.forward, out hit, distanceToSee))
		{
			if(hit.collider.tag == "Interactable")
			{
				if(hit.collider.GetComponent<Interaction>().interactionType == Interaction.TestEnum.Item)
				{
					interactLabel.GetComponent<UILabel>().text = "'E' to pickup " + hit.collider.name.ToLower();

					if (Input.GetKeyDown(KeyCode.E))
					{
						hit.collider.GetComponent<Interaction>().Item();
					}
				}
				if(hit.collider.GetComponent<Interaction>().interactionType == Interaction.TestEnum.Door)
				{
					//if door open
					interactLabel.GetComponent<UILabel>().text = "'E' to interact";

					//if door closed
					if (Input.GetKeyDown(KeyCode.E))
					{
						hit.collider.GetComponent<Interaction>().Door();
					}
				}
			}

		}
		else
		{
			interactLabel.GetComponent<UILabel>().text = "";
		}

		// cursor influence
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		// cursor influence
		if (Input.GetButtonDown ("Fire2")) 
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}	
}