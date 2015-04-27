using UnityEngine;
using System.Collections;

public class AnimatePlayer : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () 
	{
		anim = this.transform.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Player locomotion, no weapon

		// run behavior
		// DO NOT USE
		/*if (Input.GetKey (KeyCode.LeftShift)) 
		{
			if (Input.GetKey(KeyCode.W)) 
			{
				if (Input.GetKey(KeyCode.A)) 
				{
					anim.Play ("LeftStrafeRun");
				}
				else if (Input.GetKey(KeyCode.D)) 
				{
					anim.Play ("RightStrafeRun");
				}
				else
				{
					anim.Play ("Run");
				}
			}
			else if (Input.GetKey(KeyCode.S)) 
			{
				anim.Play ("WalkBack");
			}
			else if (Input.GetKey(KeyCode.A)) 
			{
				anim.Play ("LeftStrafeRun");
			}
			else if (Input.GetKey(KeyCode.D)) 
			{
				anim.Play ("RightStrafeRun");
			}
			else 
			{
				anim.Play ("Idle");
			}
		}
		else
		{*/

		// normal walk behavior
			if (Input.GetKey(KeyCode.W)) 
			{
				if (Input.GetKey(KeyCode.A)) 
				{
					anim.Play ("LeftStrafe");
				}
				else if (Input.GetKey(KeyCode.D)) 
				{
					anim.Play ("RightStrafe");
				}
				else
				{
					anim.Play ("Walk");
				}
			}
			else if (Input.GetKey(KeyCode.S)) 
			{
				anim.Play ("WalkBack");
			}
			else if (Input.GetKey(KeyCode.A)) 
			{
				anim.Play ("LeftStrafe");
			}
			else if (Input.GetKey(KeyCode.D)) 
			{
				anim.Play ("RightStrafe");
			}
			else 
			{
				anim.Play ("Idle");
			}
		/*}*/


	}
}
