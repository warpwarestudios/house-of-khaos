using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour {

	public GameObject player;
	
	void Start()
	{
		player = GameObject.Find("Player");
	}
	
	void Shoot()
	{
		player.transform.FindChild("Main Camera").transform.FindChild("Gun").GetComponent<Shoot>().Fire();
	}
}
