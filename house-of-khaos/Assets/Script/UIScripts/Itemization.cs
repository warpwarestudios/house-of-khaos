using UnityEngine;
using System.Collections;

public class Itemization : MonoBehaviour {
	
	public string itemName;
	public string spriteIconName;
	
	public string ItemName{
		get{return itemName;} 
		set{ItemName = itemName;}}
	
	public string SpriteIconName{
		get{return spriteIconName;} 
		set{SpriteIconName = spriteIconName;}}
	
}
