﻿using UnityEngine;
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
		if (transform.parent != null) {
			if (transform.parent.tag == "MainCamera" && Input.GetButtonUp ("Fire1")) {
				StartCoroutine ("Fire");
			}
		}
	}
 

	IEnumerator Fire() 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit;
		
		if (Physics.Raycast(ray, out rayHit, 100f))
		{
			Debug.Log ("Enemy or wall to be hit!");
			if (rayHit.collider.tag == "Enemy")
			{
				Debug.Log("Enemy Hit!");
				Health health = rayHit.collider.GetComponent<Health>();
				health.updateHealth(-transform.GetComponent<Itemization>().damage);
				
			}
			else //bullet holes for environment
			{			
				var hitRotation = Quaternion.FromToRotation(Vector3.forward, rayHit.normal);
				Instantiate(bulletHolePrefab, rayHit.point, hitRotation);
				Debug.Log ("Bullet Hole made!");
				if(rayHit.rigidbody != null)
				{
					rayHit.rigidbody.AddForceAtPosition(Vector3.forward * 10, rayHit.point);
				}
			}
			
			
		}


		muzzleFlash.Play();
		audio.Play ();
		Debug.Log ("Firing gun!");

		yield return new WaitForSeconds(0.06f);
	}

}
