using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour {

	public static int CurrentNumberOFEnemies;
	public GameObject enemy;
	public int maximumNumberOfEnemies;

	public bool canSpawn = false;

	public Transform UIKillCounter;
	public static KillCountUI killCounter;

	public static GameObject[] waypointList;

	// Use this for initialization
	void Start () {
		UIKillCounter = GameObject.Find ("UI Root").transform.FindChild ("EnemyLeft"); 
		if (UIKillCounter != null) {
			killCounter = UIKillCounter.GetComponent<KillCountUI> ();
		} else {
			Debug.Log("KILL COUNTER IS NULL!");
		}
		CurrentNumberOFEnemies = 0;
	}
	
	// Update is called once per frame
	void Update () {

		if (canSpawn) 
		{
			Spawn();
		}
	}

	public void Spawn()
	{
		if (CurrentNumberOFEnemies < maximumNumberOfEnemies) 
		{
			UpdateWaypointList();

			GameObject enemySpawn = waypointList[Random.Range(0,waypointList.Length - 1)];
			
			Vector3 enemyPos = new Vector3(enemySpawn.transform.position.x, 0.5f, enemySpawn.transform.position.z);
			
			enemy = PhotonNetwork.Instantiate("MaleZom", enemyPos , Quaternion.identity,0);

			Debug.Log ("Waypoint Parent: " + enemySpawn.transform.parent.name);
			Debug.Log ("Waypoint: X = " + (enemySpawn.transform.position.x) + " Z = " +  (enemySpawn.transform.position.z));
			Debug.Log ("Enemy: X = " + enemy.transform.position.x + " Z = " + enemy.transform.position.z);
			Debug.Log ("Enemy Spawn Offset: X = " + (enemySpawn.transform.position.x - enemy.transform.position.x) + " Z = " +  (enemySpawn.transform.position.z - enemy.transform.position.z));


			Debug.Log ("Creating Enemy!");

			killCounter.AddEnemy();

			CurrentNumberOFEnemies++;


		}
	}

	public void UpdateWaypointList()
	{
		waypointList = GameObject.FindGameObjectsWithTag("Waypoint");
	}
}
