using UnityEngine;
using System.Collections;

public class BasicShooterController : MonoBehaviour {
	private Animator anim;

	void Start () {
		anim = this.transform.GetComponent<Animator>();
		anim.SetLayerWeight(1, 1);
		anim.SetLayerWeight(2, 1);
	}

	void OnGUI () {
		GUILayout.Label("CONTROLS");
		GUILayout.Label("Movement: W A S D");
		GUILayout.Label("Turn: Q E");
		GUILayout.Label("Shoot: Mouse 1");
		GUILayout.Label("Reload: R");
		GUILayout.Label("Granade: G");
		GUILayout.Label("Take Damage: H");
		GUILayout.Label("Jump: Spacebar");
	}

	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		anim.SetFloat("Speed", vertical);
		anim.SetFloat("Direction", horizontal);

		//Procedural rotation input, applied while moving. This allows turning without the need for turning animations.
		if (vertical > 0.05f){
			if(horizontal > 0.05f)
				this.transform.Rotate(Vector3.up * (Time.deltaTime + 2), Space.World);
			if(horizontal < -0.05f)
				this.transform.Rotate(Vector3.up * (Time.deltaTime + -2), Space.World);
		}

		else if (vertical < -0.05f){
			if(horizontal > 0.05f)
				this.transform.Rotate(Vector3.up * (Time.deltaTime + -2), Space.World);
			if(horizontal < -0.05f)
				this.transform.Rotate(Vector3.up * (Time.deltaTime + 2), Space.World);
		}

		//Procedural rotation input for stationary turning
		if(Input.GetKey(KeyCode.Q)){
			anim.SetFloat("Turn", -1, 0.1f, Time.deltaTime);
			this.transform.Rotate(Vector3.up * (Time.deltaTime + -2), Space.World);
		}

		else if (Input.GetKey(KeyCode.E)){
			anim.SetFloat("Turn", 1, 0.1f, Time.deltaTime);
			this.transform.Rotate(Vector3.up * (Time.deltaTime + 2), Space.World);
		}

		else { anim.SetFloat("Turn", 0, 0.1f, Time.deltaTime); }


		// Clicking the mouse will cause the character to shoot, letting go returns to aiming
		if (Input.GetButton("Fire1")){
			anim.SetBool("Fire", true);
		} else { anim.SetBool("Fire", false); }

		//Pressing the R key will reload the weapon
		if (Input.GetKeyDown(KeyCode.R)){
			StartCoroutine(TriggerAnimatorBool("Reload"));
		}

		//Pressing the G Key will cause the character to throw a Granade
		if (Input.GetKeyDown(KeyCode.G)){
			StartCoroutine(TriggerAnimatorBool("Granade"));
		}
		
		if (Input.GetButtonDown("Jump")){
			StartCoroutine(TriggerAnimatorBool("Jump"));
		}

		//Pressing the H key to simulate getting hit
		if (Input.GetKeyDown(KeyCode.H)){
			anim.SetLayerWeight(2, Random.Range(0.4f, 1f));
			StartCoroutine(TriggerAnimatorBool("Hit"));
		}
	}
	
	///Triggers the bool of the provided name in the animator.
	///The bool is only active for a single frame to prevent looping.
	private IEnumerator TriggerAnimatorBool (string name){
		anim.SetBool(name, true);
		yield return null;
		anim.SetBool(name, false);
	}
}
