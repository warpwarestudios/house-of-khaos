﻿using UnityEngine;
using System.Collections;

public class ZombieAI : MonoBehaviour {

	public GameObject player;
	public NavMeshAgent navAgent;
	public Animator animController;

	public float wanderSpeed = 3f;// The nav mesh agent's speed when chasing.
	public float chaseSpeed = 5f;// The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;// The amount of time to wait when the last sighting is reached.
	private float chaseTimer;// A timer for the chaseWaitTime.
	public Vector3 personalLastSighting = new Vector3(1000f, 1000f, 1000f);// The last global sighting of the player.
	public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);// The default position if the player is not in sight.
	
	// Use this for initialization
	void Start () {
		// Set up the references.
		player = GameObject.FindGameObjectWithTag ("Player");
		navAgent = GetComponent <NavMeshAgent> ();
		animController = GetComponent <Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		//navAgent.SetDestination (player.position);
		if ((player = GameObject.FindGameObjectWithTag ("Player"))!= null) 
		{
			personalLastSighting = player.transform.position;
		}
		else
		{
			navAgent.speed = wanderSpeed;
			IdleAnim();
		}

		// If the player has been sighted and isn't dead...
		if(player != null)
		{
			if (personalLastSighting != resetPosition && player.GetComponent<Health> ().currentHealth > 0f) 
			{
				// ... chase.
				Chasing();
			}
		}

		// If the player is in sight, in range and is alive...
		/*if (navAgent.remainingDistance <= navAgent.stoppingDistance && player.GetComponent<Health> ().currentHealth > 0f) 
		{

			StartCoroutine ("Attacking");
			// ... attack.
			//Attacking ();
			animController.SetBool ("Attack", false);
		}*/

		//set animation speed based on navAgent 'Speed' var
		// causes issues
		//animController.speed = navAgent.speed;
	}

	void OnAnimatorMove ()
	{
		//only perform if walking
		if (navAgent.hasPath)
		{
			//set the navAgent's velocity to the velocity of the animation clip currently playing
			navAgent.velocity = animController.deltaPosition / Time.deltaTime;
			
			//smoothly rotate the character in the desired direction of motion
			Quaternion lookRotation = Quaternion.LookRotation(navAgent.desiredVelocity);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, navAgent.angularSpeed * Time.deltaTime);
		}
	}

	IEnumerator Attacking ()
	{
		// Stop the enemy where it is.
		navAgent.SetDestination (this.transform.position);
		animController.SetBool ("Attack", true);
		yield return new WaitForSeconds (2.6f);
	}

	private void Chasing()
	{
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = personalLastSighting - transform.position;
		
		// If the the last personal sighting of the player is not close...
		//if(sightingDeltaPos.sqrMagnitude > 4f)
		// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
		navAgent.destination = personalLastSighting;
		
		// Set the appropriate speed for the NavMeshAgent.
		if (player != null) 
		{
			navAgent.speed = chaseSpeed;
		}
		
		// state machine AI control
		// chaseSpeed confirms player is targeted
		if(navAgent.hasPath && (navAgent.speed == chaseSpeed))
		{
			ChaseAnim();
		}
		
		
		// If near the last personal sighting...
		if(navAgent.remainingDistance < navAgent.stoppingDistance)
		{
			// ... increment the timer.
			chaseTimer += Time.deltaTime;
			
			// If the timer exceeds the wait time...
			if(chaseTimer >= chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				personalLastSighting = resetPosition;
				chaseTimer = 0f;
				navAgent.speed = wanderSpeed;
				IdleAnim();
			}
		}
		else
			// If not near the last sighting personal sighting of the player, reset the timer.
			chaseTimer = 0f;
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