using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public AudioSource audio;
	public GameObject bulletHolePrefab;
	public ParticleSystem muzzleFlash;

	void Start()
	{

	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton ("Fire1")) 
		{
			Debug.Log ("Firing gun!");
			muzzleFlash.Play();
		}
	}
	
	public void Fire()
	{

	}
}
