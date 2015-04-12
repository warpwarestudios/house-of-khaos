using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
	public float turnSmoothing = 15f;   // A smoothing value for turning the player.
	public float speedDampTime = 0.1f;  // The damping for the speed parameter

	private Animator anim;              // Reference to the animator component.
	
	
	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
	}
	
	
	void Update ()
	{
		// Cache the inputs.
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		//bool sneak = Input.GetButton("Sneak");
		
		MovementManagement(h, v/*, sneak*/);
	}
	
	void MovementManagement (float horizontal, float vertical/*, bool sneaking*/)
	{
		// Set the sneaking parameter to the sneak input.
		//anim.SetBool("Sneak", sneaking);
		
		// If there is some axis input...
		if(horizontal != 0f || vertical != 0f)
		{
			// ... set the players rotation and set the speed parameter to 5.5f.
			Rotating(horizontal, vertical);
			anim.SetFloat("Speed", 5.5f, speedDampTime, Time.deltaTime);
		}
		else
			// Otherwise set the speed parameter to 0.
			anim.SetFloat("Speed", 0);
	}
	
	
	void Rotating (float horizontal, float vertical)
	{
		// Create a new vector of the horizontal and vertical inputs.
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		
		// Create a rotation based on this new vector assuming that up is the global y axis.
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.forward);
		
		// Create a rotation that is an increment closer to the target rotation from the player's rotation.
		Quaternion newRotation = Quaternion.Lerp(this.GetComponent<Rigidbody>().transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		newRotation.x = 0;
		// Change the players rotation to this new rotation.
		this.GetComponent<Rigidbody>().transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, turnSmoothing * Time.deltaTime);
	}
}