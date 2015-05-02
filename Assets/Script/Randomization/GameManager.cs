using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Map mapPrefab;
	
	private GameObject mapInstance;

	private void Start () {
		BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			//RestartGame();
		}
	}
	

	private void BeginGame () {
		mapInstance = PhotonNetwork.Instantiate("Map", new Vector3(0,0,0),Quaternion.identity, 0) as GameObject;
	    mapInstance.GetComponent<Map>().Generate();
	}
	
	private void RestartGame () {
		StopAllCoroutines ();
		Destroy(mapInstance.gameObject);
		BeginGame();

	}
}
