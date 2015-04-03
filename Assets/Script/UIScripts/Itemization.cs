using UnityEngine;
using System.Collections;

public class Itemization : MonoBehaviour {
	
//	public string iName;
	//public string sIconName;
	//public string sName = "None";
	
	public string ItemName{
		get; 
	    set;}
	
	public string SpriteIconName{
		get;
		set;}
		
	public string SkillName{
		get;
		set;}
		
	public float DuraSaniAmmo{
		get;
		set;}
		
	public float Damage{
		get;
		set;}
		
	public float Heal{
		get;
		set;}
	
	public Vector3 Position{
		get;
		set;}
		
	public Quaternion Rotation{
		get;
		set;}
		
	public Vector3 Scale{
		get;
		set;}
}
