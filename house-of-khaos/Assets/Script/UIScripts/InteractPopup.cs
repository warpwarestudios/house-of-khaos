using UnityEngine;
using System.Collections;

public class InteractPopup : MonoBehaviour {

	UILabel font;
	// Use this for initialization
	void Start () {
		font = this.gameObject.GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
	 	//font.text = "";
	}
}
