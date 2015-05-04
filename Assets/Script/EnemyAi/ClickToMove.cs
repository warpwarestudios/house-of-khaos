using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {
	
	public EnemyMovement charNav; //link to CharNav script on UnityGuy1
	
	void Update ()
	{
		//set character destination to point of mouse click
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit rayHit;
			
			if (Physics.Raycast(ray, out rayHit, 100f))
			{
//				EnemyMovement.navAgent.destination = rayHit.point;
			}

		}
	}
}
