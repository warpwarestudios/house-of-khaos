using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public AudioSource gunSound;
	public AudioSource noAmmoSound;
	public GameObject bulletHolePrefab;
	public ParticleSystem muzzleFlash;

	// Update is called once per frame
	void Update () 
	{
		if (transform.parent != null) {
			if (transform.parent.tag == "MainCamera" && Input.GetButtonUp ("Fire1")) {
				if(this.gameObject.GetComponent<Itemization>().duraSaniAmmoRemaining > 0)
				{
					StartCoroutine ("Fire");
					this.gameObject.GetComponent<Itemization>().duraSaniAmmoRemaining--;
				}
				else
				{
					noAmmoSound.Play();
				}
				
			}
			if (transform.parent.tag == "MainCamera" && Input.GetKeyDown(KeyCode.R)) 
			{
				Reload();
			}
		}

	}
 
	void Reload()
	{
		if(this.gameObject.GetComponent<Itemization>().duraSaniAmmo >= 1)
		{
			if(this.gameObject.GetComponent<Itemization>().duraSaniAmmoRemaining < this.gameObject.GetComponent<Itemization>().maxAmmo)
			{
				float missingAmmo = this.gameObject.GetComponent<Itemization>().maxAmmo - this.gameObject.GetComponent<Itemization>().duraSaniAmmoRemaining;
				this.gameObject.GetComponent<Itemization>().duraSaniAmmo -= missingAmmo;
				this.gameObject.GetComponent<Itemization>().duraSaniAmmoRemaining += missingAmmo;
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
				if(rayHit.collider.tag == "Enemy")
				{
					var hitRotation = Quaternion.FromToRotation(Vector3.forward, rayHit.normal);
					Instantiate(bulletHolePrefab, rayHit.point, hitRotation);
					Debug.Log ("Bullet Hole made!");
				}
				if(rayHit.rigidbody != null)
				{
					rayHit.rigidbody.AddForceAtPosition(Vector3.forward * 10, rayHit.point);
				}
			}
			
			
		}


		muzzleFlash.Play();
		gunSound.Play ();
		Debug.Log ("Firing gun!");

		yield return new WaitForSeconds(0.06f);
	}

}
