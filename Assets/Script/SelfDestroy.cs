using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {

	public float time = 0.1f;

	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, time);
	}

}