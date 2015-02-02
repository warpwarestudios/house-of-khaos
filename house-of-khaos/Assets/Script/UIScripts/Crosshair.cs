using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public RaycastHit hit;
	GameObject item;
	Interaction interaction;
	// Use this for initialization
	void Start () {
		interaction = GetComponent<Interaction>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2, 0));
		if(Physics.Raycast(ray, out hit, 5))
		{
			
			if(hit.collider.gameObject.GetComponent<Interaction>() != null)
			{
				Debug.Log("Hit");
				hit.collider.gameObject.GetComponent<Interaction>().OnLookEnter();
			}
			else
			{
				interaction.setSelected(false);
			}
		}
	}
}
