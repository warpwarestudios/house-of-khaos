using UnityEngine;
using System.Collections;

public class ZombieSounds : MonoBehaviour {

	public AudioSource zombie;

	public AudioClip moan;
	public AudioClip getShot;
	private bool waiting = false;

	// Use this for initialization
	void Start () {
	
		zombie.clip = moan;
	}
	
	// Update is called once per frame
	void Update () {
		if (!waiting) 
		{
			waiting = true;
			StartCoroutine("Moan");
		}
	}

	public void GetShot()
	{
		zombie.PlayOneShot (getShot);
		zombie.clip = moan;
	}

	private IEnumerator Moan()
	{
		yield return new WaitForSeconds(Random.Range(1, 60));

		zombie.Play ();
		waiting = false;
	}
}
