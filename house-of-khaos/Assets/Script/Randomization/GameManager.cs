using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Map mapPrefab;
	
	private Map mapInstance;

	private void Start () {
		BeginGame();
	}
	
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			RestartGame();
		}
	}
	

	private void BeginGame () {
		mapInstance = Instantiate(mapPrefab) as Map;
	    StartCoroutine(mapInstance.Generate ());
		//mapInstance.Generate ();
	}
	
	private void RestartGame () {
		StopAllCoroutines ();
		Destroy(mapInstance.gameObject);
		BeginGame();

	}
}
