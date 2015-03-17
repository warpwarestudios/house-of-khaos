using UnityEngine;
using System.Collections;

public class SkillItemization : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Skill ("EmptySlot", "Empty", "WhiteBackground", "None");
		//Skill ("Gun", "Pistol", "BlueCube", "Shoot");
	
		
	 	
	 	
	}
	
	//This is the placeholder for empty skills on hotbar
	void Skill(string gameObject, string name, string icon, string skill)
	{
		GameObject item = GameObject.Find(gameObject);
		Itemization id = item.GetComponent<Itemization>();
		
		id.ItemName = name;
		id.SpriteIconName = icon;
		id.SkillName = skill;
				
	}
	
}
