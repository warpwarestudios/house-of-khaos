using UnityEngine;
using System.Collections;

public class SelfDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, 5f);
	}

}