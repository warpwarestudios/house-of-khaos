using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieAI : MonoBehaviour {

	public GameObject player;
	public NavMeshAgent navAgent;
	public Animator animController;
	public List<GameObject> AttackedObjects; // Hit Detection for zombie attack

	public int Damage = 10;// Damge dealt by melee attack
	public float wanderSpeed = 3f;// The nav mesh agent's speed when chasing.
	public float chaseSpeed = 5f;// The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;// The amount of time to wait when the last sighting is reached.
	private float chaseTimer;// A timer for the chaseWaitTime.
	
	// Use this for initialization
	void Start () {

		// Set up the references.
		//player = GameObject.FindGameObjectWithTag ("Player");
		navAgent = GetComponent <NavMeshAgent> ();
		animController = GetComponent <Animator> ();
		AttackedObjects = new List<GameObject> (); // Hit Detection for zombie attack
	}
	
	// Update is called once per frame
	void Update () {

		if(this.GetComponent<Health>().currentHealth <= 0)
		{
			if(animController.GetBool("Dead"))
			{
				animController.SetBool ("Attack", false);
				return;
			}

			animController.SetBool ("Attack", false);
			Dead ();

			Waypoint.CurrentNumberOFEnemies--;
			Waypoint.killCounter.AddEnemyDeath();
		}

		if (player == null) 
		{
			player = GameObject.FindGameObjectWithTag ("Player");
			if(player == null)
			{
				animController.SetBool ("Attack", false);
			}
		}
		else if(!navAgent.hasPath)
		{
			navAgent.speed = wanderSpeed;
			IdleAnim();
		}


		// If the player has been sighted and isn't dead...
		/*if(player != null)
		{
			if (player.GetComponent<Health> ().currentHealth > 0f) 
			{
				// ... chase.
				Chasing();
			}
		}*/

		if (player != null) 
		{
			if(animController.GetBool ("Attack"))
			{
				navAgent.SetDestination(player.transform.position);
				animController.SetBool ("Attack", false);
			}


			// If the player is in sight, in range and is alive...
			if (navAgent.hasPath && navAgent.remainingDistance <= navAgent.stoppingDistance+0.7f && player.GetComponent<Health> ().currentHealth > 0f) 
			{
				// ... attack.
				Attacking ();
			}

			// If the player has been sighted and isn't dead...
			if (player.GetComponent<Health> ().currentHealth > 0f && !animController.GetBool("Attack")) 
			{
				// ... chase.
				Chasing();
			}
		}
	}

	private void Attacking ()
	{
		// Stop the enemy where it is.
		navAgent.Stop ();
		IdleAnim ();
		animController.SetBool ("Attack", true);

	}

	private void Chasing()
	{
		// set the destination for the NavMeshAgent to the last personal sighting of the player.
		navAgent.SetDestination (player.transform.position);
		navAgent.Resume ();
		
		// Set the appropriate speed for the NavMeshAgent.
		if (player != null) 
		{
			navAgent.speed = chaseSpeed;
		}
		
		// state machine AI control
		// chaseSpeed confirms player is targeted
		if(navAgent.hasPath && navAgent.speed == chaseSpeed)
		{
			ChaseAnim();
		}
	}

	private void Dead ()
	{
		// Stop the enemy where it is.
		navAgent.velocity = new Vector3(0,0,0);
		navAgent.Stop();

		float rand;

		if(Random.value > 0.5f)
		{
			rand = 1f;
		}
		else
		{
			rand = 0f;
		}

		animController.SetFloat ("RandomDeath", rand);
		animController.SetBool ("Dead", true);

		animController.SetBool ("Attack", false);
		animController.SetBool("Idle", false);
		animController.SetBool("Chase", false);
		animController.SetBool("Wander", false);
		
	}

	// keep model turned right way
	void OnAnimatorMove ()
	{
		Quaternion lookRotation;

		//only perform if walking
		if (navAgent.hasPath) {

			if(animController.GetBool ("Attack") && player != null)
			{
				Vector3 direction = player.transform.position - transform.position;
				//float angle = Vector3.Angle(direction, transform.forward);
				lookRotation = Quaternion.LookRotation(direction);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, lookRotation, navAgent.angularSpeed * Time.deltaTime);
			}
			else if (navAgent.desiredVelocity != Vector3.zero)
			{
				//set the navAgent's velocity to the velocity of the animation clip currently playing
				navAgent.velocity = animController.deltaPosition / Time.deltaTime;
				
				//smoothly rotate the character in the desired direction of motion
				lookRotation = Quaternion.LookRotation (navAgent.desiredVelocity);
				transform.rotation = Quaternion.RotateTowards (transform.rotation, lookRotation, navAgent.angularSpeed * Time.deltaTime);
			}
		}
	}


	public void ZombieDamage()
	{
		foreach (GameObject attackedObject in AttackedObjects) 
		{
			if (attackedObject == null) {
				Debug.Log("No Hit Detected");
				return;
			}
			
			if (attackedObject.tag == "Player")
			{
				Debug.Log("Hit player!");
				Health health = attackedObject.GetComponent<Health>();
				health.damaged = true;
				health.updateHealth(-Damage);
				health.lastDamageTime = Time.time + 10f;
				float screenFlashAlpha = (health.currentHealth / health.maxHealth) <= 0.4f ? 1f - (health.currentHealth / health.maxHealth) : 0;
				UIDamage.Show(this.transform.position, true, screenFlashAlpha, true);
			}
		}

	}
	
	public void OnTriggerEnter(Collider col)
	{
		//add objects to the list
		AttackedObjects.Add (col.gameObject);
	}

	public void OnTriggerExit(Collider col)
	{
		//add objects to the list
		AttackedObjects.Remove(col.gameObject);
	}

	private void IdleAnim()
	{
		animController.SetBool("Idle", true);
		animController.SetBool("Chase", false);
		animController.SetBool("Wander", false);
	}

	private void WanderAnim()
	{
		animController.SetBool("Idle", false);
		animController.SetBool("Chase", false);
		animController.SetBool("Wander", true);
	}

	private void ChaseAnim()
	{
		animController.SetBool("Idle", false);
		animController.SetBool("Chase", true);
		animController.SetBool("Wander", false);
	}
}
