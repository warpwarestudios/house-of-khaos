using UnityEngine;
using System.Collections;

public class DoorActivator : MonoBehaviour {

	private Animator animator;
	
	void Awake () 
	{
		animator = GetComponent <Animator>();
	}

	void Update()
	{
		if (Input.GetButtonUp ("Fire1")) 
		{
			animator.SetBool ("Open", !animator.GetBool("Open") );
		}
			
	}
	
	void OnTriggerEnter (Collider other) 
	{
		if (other.gameObject.tag == "Player") 
		{
			animator.SetBool ("Open", true);
		}
	}
	
	void OnTriggerExit (Collider other) 
	{
		if (other.gameObject.tag == "Player") 
		{
			animator.SetBool ("Open", false);
		}
	}
}