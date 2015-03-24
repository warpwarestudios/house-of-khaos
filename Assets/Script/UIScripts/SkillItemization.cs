using UnityEngine;
using System.Collections;

public class SkillItemization : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		//    GAMEOBJECT			NAME			ICONNAME			SKILLNAME			DURABILITY/SANITY/AMMO			DAMAGE					HEAL							POSITION						ROTATION						SCALE                 
		Skill("EmptySlot",			"Empty",		"WhiteBackground",	"None",				0,								0,						0,								new Vector3(0f,0f,0f),			Quaternion.Euler(0,0,0),		new Vector3(0,0,0));
		Skill("Gun",				"Pistol",		"BlueCube",			"Shoot",			10,								20,						0,								new Vector3(0.4f,-0.4f,1.2f), 	Quaternion.Euler(0.0f,82,0),	new Vector3(0.32f,0.27f,0.27f));
		Skill("Rosary",				"Rosary",		"RedCube",			"None",				10,								0,						30,								new Vector3(0.4f,-0.4f,0.65f),	Quaternion.Euler(0.0f,100,0),	new Vector3(1,1,1));
	 	
	}
	
	//This is the placeholder for empty skills on hotbar
	void Skill(string gameObject, string name, string icon, string skill, float duraSaniAmmo, float damage, float heal, Vector3 pos, Quaternion rot, Vector3 sca)
	{
		GameObject item = GameObject.Find(gameObject);
		Itemization id = item.GetComponent<Itemization>();
		
		id.ItemName = name;
		id.SpriteIconName = icon;
		id.SkillName = skill;
		id.DuraSaniAmmo = duraSaniAmmo;
		id.Damage = damage;
		id.Heal = heal;
		id.Position = pos;
		id.Rotation = rot;
		id.Scale = sca;

	}
	
}
