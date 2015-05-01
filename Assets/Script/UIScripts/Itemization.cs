using UnityEngine;
using System.Collections;

public class Itemization : MonoBehaviour {
	
	public string itemName;
	public string skillIconName;
	public string skillName = "None";
	public float resourceAmount;
	public float damage;
	public float heal;
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
	public float resourceRemaining;
	public bool usesAmmo;
	public float maxAmmo;
	
	
	void Start()
	{
		resourceRemaining = resourceAmount;
	}
	
//	public string ItemName{
//		get ; 
//	    set ;}
//	
//	public string SpriteIconName{
//		get;
//		set;}
//		
//	public string SkillName{
//		get;
//		set;}
//		
//	public float DuraSaniAmmo{
//		get;
//		set;}
//		
//	public float Damage{
//		get;
//		set;}
//		
//	public float Heal{
//		get;
//		set;}
//	
//	public Vector3 Position{
//		get;
//		set;}
//		
//	public Quaternion Rotation{
//		get;
//		set;}
//		
//	public Vector3 Scale{
//		get;
//		set;}
}
