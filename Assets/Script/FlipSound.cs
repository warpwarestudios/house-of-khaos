using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class FlipSound : MonoBehaviour {

	public AudioClip firstSound;
	public AudioClip secondSound;

	public AudioSource source;

	// Use this for initialization
	void Start () {
		source = this.GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	}


}
