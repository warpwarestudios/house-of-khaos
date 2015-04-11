using UnityEngine;
using System.Collections;

public class PlayerSpawn : MonoBehaviour {

	public float probability;

	// Use this for initialization
	void Start () {
		if(Random.value < probability)
		{
			Destroy(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
