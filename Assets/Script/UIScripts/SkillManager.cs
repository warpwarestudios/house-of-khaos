using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	public GameObject player;
	
	void Start()
	{
		player = GameObject.FindWithTag("Player");
	}
	
	void Shoot()
	{
//		player.transform.FindChild("Main Camera").transform.FindChild("Pistol").GetComponent<Shoot>().Fire();
	}
}
