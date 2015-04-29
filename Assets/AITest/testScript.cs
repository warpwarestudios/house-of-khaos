using UnityEngine;
using System.Collections;

public class testScript : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		Instantiate(player,this.transform.position,Quaternion.identity);
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
