using UnityEngine;
using System.Collections;

public class Brighten : MonoBehaviour {

	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = this.GetComponent<Renderer> ();

		rend.material.EnableKeyword ("_EMISSION");
 	}
	
	// Update is called once per frame
	void Update () {
		//DynamicGI.UpdateMaterials (GetComponent<Renderer>());
	}

	public void Highlight()
	{
		Debug.Log ("Highlighting " + this.name);

		Material mat = GetComponent<Renderer> ().material;

		mat.EnableKeyword ("_Emission");
		mat.SetColor("_Emission", Color.green);
		//DynamicGI.SetEmissive(GetComponent<Renderer>(), Color.white);
		//DynamicGI.SetEmissive (rend, (Color.yellow * 1000f));
		//DynamicGI.UpdateMaterials (rend);
		//DynamicGI.UpdateEnvironment();
	}
	
	public void Darken()
	{
		DynamicGI.SetEmissive (rend, (Color.white * 1f));
		DynamicGI.UpdateMaterials (rend);
	}
	

}
