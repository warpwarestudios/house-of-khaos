using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour {

	//triggers a certain sound when the collision box is entered

	public string name; // name of gameobject to search for

	private Transform sound;
	private FlipSound flipSound;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) 
		{
			sound = other.transform.FindChild(name);
			flipSound = sound.GetComponent<FlipSound>();
			flipSound.source.clip = flipSound.secondSound;
			flipSound.source.loop = true;
			flipSound.source.Play();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag ("Player")) 
		{
			sound = other.transform.FindChild(name);
			flipSound = sound.GetComponent<FlipSound>();
			flipSound.source.clip = flipSound.firstSound;
			flipSound.source.loop = true;
			flipSound.source.Play();

		}
	}
}
