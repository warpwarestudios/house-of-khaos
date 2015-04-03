using UnityEngine;
using System.Collections;

public class ScaleLighting : MonoBehaviour {

	public Light pointLight;

	// Use this for initialization
	void Start () {
		Debug.Log ("Local Scale: " + transform.localScale.x);

	}
	
	// Update is called once per frame
	void Update () {
		pointLight.range = 4;

	}
}
