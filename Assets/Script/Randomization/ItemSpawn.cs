using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour {

	public GameObject[] items;
	public float itemSpawnProbability;
	public GameObject item;
	public bool hasSpawned = false;

	// Use this for initialization
	void Start () {
		if( Random.value <= itemSpawnProbability)
		{
			GameObject chosen = items[Random.Range (0,items.Length - 1)].gameObject; 
			item = Instantiate(chosen, transform.position , Quaternion.identity) as GameObject;
			item.name = chosen.name;
			hasSpawned = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
