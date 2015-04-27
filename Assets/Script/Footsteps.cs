using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {

	public AudioSource footstep;
	public float delay;
	private bool ready = true;

	private CharacterController controller;

	// Use this for initialization
	void Start () {
		controller = this.GetComponentInParent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine ("Step");
	}

	IEnumerator Step()
	{
		if (controller != null && ready) 
		{
			if(controller.isGrounded && controller.velocity.magnitude > 0.3f)
			{
				footstep.Play();
				ready = false;
				yield return new WaitForSeconds(delay/controller.velocity.magnitude);
				ready = true;
			}
			else
			{
				yield return 0;
			}
		}
	}
}
