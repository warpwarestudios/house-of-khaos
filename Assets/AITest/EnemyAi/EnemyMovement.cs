using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
	public Transform player;
	public NavMeshAgent navAgent;
	public Animator animController;

	public float chaseSpeed = 5f;// The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;// The amount of time to wait when the last sighting is reached.
	private float chaseTimer;// A timer for the chaseWaitTime.
	public Vector3 personalLastSighting = new Vector3(1000f, 1000f, 1000f);// The last global sighting of the player.
	public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);// The default position if the player is not in sight.

	//private Vector3 previousPosition;
	//public float curSpeed;
	
	void Awake ()
	{
		// Set up the references.
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		navAgent = GetComponent <NavMeshAgent> ();
		animController = GetComponent <Animator> ();
	}
	
	
	void Update ()
	{
		//navAgent.SetDestination (player.position);
		personalLastSighting = player.transform.position;

		//set animation speed based on navAgent 'Speed' var
		//animController.speed = navAgent.speed;

		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = personalLastSighting - transform.position;
		
		// If the the last personal sighting of the player is not close...
		//if(sightingDeltaPos.sqrMagnitude > 4f)
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			navAgent.destination = personalLastSighting;
		
		// Set the appropriate speed for the NavMeshAgent.
		navAgent.speed = chaseSpeed;
		
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
			}
		}
		else
			// If not near the last sighting personal sighting of the player, reset the timer.
			chaseTimer = 0f;




		//Vector3 curMove = transform.position - previousPosition;
		//curSpeed = curMove.magnitude / Time.deltaTime;
		//previousPosition = transform.position;
		//animController.SetFloat ("Speed", curSpeed);
	} 

	void OnAnimatorMove ()
	{
		//only perform if walking
		if (navAgent.hasPath)
		{
			//set the navAgent's velocity to the velocity of the animation clip currently playing
			//navAgent.velocity = animController.deltaPosition / Time.deltaTime;
			
			//smoothly rotate the character in the desired direction of motion
			Quaternion lookRotation = Quaternion.LookRotation(navAgent.desiredVelocity);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, navAgent.angularSpeed * Time.deltaTime);
		}
	}
}