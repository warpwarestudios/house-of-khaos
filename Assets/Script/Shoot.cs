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
				if(this.gameObject.GetComponent<Itemization>().resourceAmount > 0)
				{
					//fire gun, subtract from current ammo
					StartCoroutine ("Fire");
					this.gameObject.GetComponent<Itemization>().resourceAmount--;
				}
				else
				{
					noAmmoSound.Play();
				}
				
			}
			if (transform.parent.tag == "MainCamera" && Input.GetKeyUp(KeyCode.R)) 
			{
				Debug.Log ("Reloading!");
				Reload();
			}
		}

	}
 
	void Reload()
	{

		// calculate the amount of missing ammo by doing maxAmmo - currentAmmo 
		float missingAmmo = this.gameObject.GetComponent<Itemization>().maxAmmo - this.gameObject.GetComponent<Itemization>().resourceAmount;

		//if the remaining ammo is greater than or equal to the missing ammo, reload
		if (this.gameObject.GetComponent<Itemization> ().resourceRemaining >= missingAmmo) {
			//subtract the amount of missing Ammo from the total number of bullets remaning
			this.gameObject.GetComponent<Itemization> ().resourceRemaining -= missingAmmo;
			//add the missing ammo to the current ammo
			this.gameObject.GetComponent<Itemization> ().resourceAmount += missingAmmo;
		}
		//if there is less, add remaining ammo to missing ammo and set remaining to zero
		else 
		{
			this.gameObject.GetComponent<Itemization> ().resourceAmount += this.gameObject.GetComponent<Itemization> ().resourceRemaining;
			this.gameObject.GetComponent<Itemization> ().resourceRemaining = 0;

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
