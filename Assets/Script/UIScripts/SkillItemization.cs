using UnityEngine;
using System.Collections;

public class SkillItemization : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		//    GAMEOBJECT			NAME			ICONNAME			SKILLNAME			POSITION						ROTATION						SCALE                 
		Skill("EmptySlot",			"Empty",		"WhiteBackground",	"None",				new Vector3(0f,0f,0f),			Quaternion.Euler(0,0,0),		new Vector3(0,0,0));
		Skill("Gun",				"Pistol",		"BlueCube",			"Shoot",			new Vector3(0.4f,-0.4f,1.2f), 	Quaternion.Euler(0.0f,82,0),	new Vector3(0.32f,0.27f,0.27f));
		Skill("Rosary",				"Rosary",		"RedCube",			"None",				new Vector3(0.4f,-0.4f,0.65f),	Quaternion.Euler(0.0f,100,0),	new Vector3(1,1,1));
	 	
	}
	
	//This is the placeholder for empty skills on hotbar
	void Skill(string gameObject, string name, string icon, string skill, Vector3 pos, Quaternion rot, Vector3 sca)
	{
		GameObject item = GameObject.Find(gameObject);
		Itemization id = item.GetComponent<Itemization>();
		
		id.ItemName = name;
		id.SpriteIconName = icon;
		id.SkillName = skill;
		id.Position = pos;
		id.Rotation = rot;
		id.Scale = sca;

	}
	
}
