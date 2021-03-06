﻿using UnityEngine;
using System.Collections;

public class AnimatePlayer : MonoBehaviour {

	Animator anim;
	bool heldItem = false;

	// Use this for initialization
	void Start () 
	{
		anim = this.transform.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.transform.GetComponent<Health> ().currentHealth <= 0) 
		{
			if(anim.GetBool("DeadF") || anim.GetBool("DeadB"))
			{
				return;
			}
			if(Random.value < 0.5f)
			{
				anim.SetBool("DeadF", true);
			}
			else
			{
				anim.SetBool("DeadF", true);
			}

			return;
		}

		NormalLocomotion();
		
	}

	void NormalLocomotion()
	{
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
	}

	void PistolLocomotion()
	{
		// normal walk behavior
		if (Input.GetKey(KeyCode.W)) 
		{
			if (Input.GetKey(KeyCode.A)) 
			{
				anim.Play ("PistolLeftStrafe");
			}
			else if (Input.GetKey(KeyCode.D)) 
			{
				anim.Play ("PistolRightStrafe");
			}
			else
			{
				anim.Play ("PistolWalk");
			}
		}
		else if (Input.GetKey(KeyCode.S)) 
		{
			anim.Play ("PistolWalkBack");
		}
		else if (Input.GetKey(KeyCode.A)) 
		{
			anim.Play ("PistolLeftStrafe");
		}
		else if (Input.GetKey(KeyCode.D)) 
		{
			anim.Play ("PistolRightStrafe");
		}
		else 
		{
			anim.Play ("PistolIdle");
		}
	}

}
