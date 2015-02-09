using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

	public GameObject room;
	public bool generate;

	// Use this for initialization
	void Start () {
		generate = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (generate) 
		{
			Destroy(GameObject.Find ("Room(Clone)"));
			GameObject newRoom = (GameObject)Instantiate(room, new Vector3(0,0,0), Quaternion.identity);
			this.transform.parent = newRoom.transform;
			Vector3 scale = new Vector3(Random.Range(5,20),0,Random.Range(5,20));
			room.transform.localScale = scale;
			generate = false;
		}
	}

}
