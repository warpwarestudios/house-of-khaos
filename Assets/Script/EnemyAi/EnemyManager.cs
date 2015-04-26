using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public float spawnTime = 10f;            // How long between each spawn.
	public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.
	
	
	void Start ()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		InvokeRepeating ("Spawn", 0, spawnTime);
	}
	
	
	void Spawn ()
	{	
		// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		// Make an enemy
		PhotonNetwork.Instantiate("Enemy", spawnPoints[spawnPointIndex].position, Quaternion.identity, 0);
	}
}