using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public RaycastHit hit;
	GameObject item;
	Interaction interaction;
	// Use this for initialization
	void Start () {
		item = GameObject.FindWithTag("Interaction");
		
		interaction = item.GetComponent<Interaction>();
		Screen.showCursor = false;
		Screen.lockCursor = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2, 0));
		if (Physics.Raycast (ray, out hit, 5) && hit.collider.gameObject.GetComponent<Interaction> () != null) 
		{
			hit.collider.gameObject.GetComponent<Interaction> ().OnLookEnter ();
		} else 
		{
			interaction.setSelected(false);	
		}
	}
}
