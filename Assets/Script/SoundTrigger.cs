using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour {

	//triggers a certain sound when the collision box is entered

	public AudioClip triggeredSound;
	public AudioClip oldSound;
	public string name; // name of gameobject to search for

	private Transform sound;
	private AudioSource source;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Collision triggered!");
		if (other.gameObject.CompareTag ("Player")) 
		{
			sound = other.transform.FindChild(name);
			source = sound.GetComponent<AudioSource>();
			//save current sound
			oldSound = source.clip;
			//set sound to new sound
			source.clip = triggeredSound;
			source.loop = true;
			source.Play();
		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log ("Collision exited!");
		if (other.gameObject.CompareTag ("Player")) 
		{
			sound = other.transform.FindChild(name);
			source = sound.GetComponent<AudioSource>();
			//save current sound
			triggeredSound = source.clip;
			//set sound to new sound
			source.clip = oldSound;
			source.loop = true;
			source.Play();
		}
	}
}
