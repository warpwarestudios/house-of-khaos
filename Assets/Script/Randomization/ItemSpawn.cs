﻿using UnityEngine;
using System.Collections;

public class ItemSpawn : MonoBehaviour {

	public GameObject[] items;
	public GameObject playerSpawnPoint;
	public float itemSpawnProbability;
	public GameObject item;
	public bool hasSpawned = false;

	// Use this for initialization
	void Start () {
		if( Random.value <= itemSpawnProbability)
		{
			item = Instantiate(items[0].gameObject, transform.position , Quaternion.identity) as GameObject;
			item.name = "Pistol";
			playerSpawnPoint.GetComponent<PlayerSpawn>().canSpawn = false;
			hasSpawned = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}