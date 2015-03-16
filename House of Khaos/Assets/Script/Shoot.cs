using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	public Rigidbody projectile;
	public float shotForce = 1000f;

	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public void Fire()
	{
		Rigidbody shot = Instantiate(projectile, this.transform.position, this.transform.parent.rotation) as Rigidbody;
		shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * shotForce);
	}
}
