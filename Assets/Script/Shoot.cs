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
		if (transform.parent != null) {
			if (transform.parent.parent.tag == "Player" && Input.GetButtonUp ("Fire1")) {
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
			var hitRotation = Quaternion.FromToRotation(Vector3.forward, rayHit.normal);
			Instantiate(bulletHolePrefab, rayHit.point, hitRotation);
		}


		muzzleFlash.Play();
		audio.Play ();
		Debug.Log ("Firing gun!");

		yield return new WaitForSeconds(0.06f);
	}

}
