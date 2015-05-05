using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Map mapPrefab;
	public GameObject player;
	Map map;
	bool playerSpawned = false;
	
	private GameObject mapInstance;

	private void Start () {
		BeginGame();
		map = mapInstance.GetComponent<Map>();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			//RestartGame();
		}

		if(map.generationDone && !playerSpawned)
		{
			playerSpawned = true;
			StartCoroutine("PlayerSpawn");
		}
	}
	

	private void BeginGame () {

		if(PhotonNetwork.isMasterClient)
		{
			mapInstance = PhotonNetwork.Instantiate("Map", new Vector3(0,0,0),Quaternion.identity, 0) as GameObject;
			mapInstance.GetComponent<Map>().Generate();
		}
	}

	private IEnumerator PlayerSpawn()
	{
		yield return new WaitForSeconds (5);
		//put player in random room
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
		
		GameObject playerSpawn = spawnPoints[Random.Range(0,spawnPoints.Length - 1)];
		
		Vector3 playerPos = new Vector3(playerSpawn.transform.position.x /** map.scale*/, 0.5f, playerSpawn.transform.position.z /* map.scale*/);
		
		player = PhotonNetwork.Instantiate("Mafioso", playerPos , Quaternion.identity,0);
		
		//Debug.Log ("Player Spawn Parent: " + playerSpawn.transform.parent.name);
		//Debug.Log ("Player Spawn: X = " + (playerSpawn.transform.position.x * scale) + " Z = " +  (playerSpawn.transform.position.z * scale));
		//Debug.Log ("Player: X = " + player.transform.position.x + " Z = " + player.transform.position.z);
		//Debug.Log ("Player Spawn Offset: X = " + (playerSpawn.transform.position.x - player.transform.position.x) + " Z = " +  (playerSpawn.transform.position.z - player.transform.position.z));
		//player.transform.parent = playerSpawn.transform;
		//player.transform.localPosition = new Vector3(0,0,0);
		//player.transform.parent = null;
		PhotonView pv = player.GetComponent<PhotonView>();
		if (pv.isMine) {
			MouseLook mouselook  = player.GetComponent<MouseLook>();
			mouselook.enabled = true;
			FPSInputController controller  = player.GetComponent<FPSInputController>();
			controller.enabled = true;
			CharacterMotor charactermotor = player.GetComponent<CharacterMotor>(); 
			charactermotor.enabled = true;
			Transform playerCam = player.transform.Find ("Main Camera");
			playerCam.gameObject.active = true;
			GameObject.Find("UI Root").transform.FindChild("Camera").GetComponent<Camera>().enabled = true;
		}
		//destroy all player spawn points
		foreach(GameObject spawn in spawnPoints)
		{
			//spawn.transform.DetachChildren();
			//Destroy(spawn);
		}
	}
	
	private void RestartGame () {
		StopAllCoroutines ();
		Destroy(mapInstance.gameObject);
		BeginGame();

	}
}
