using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public GameObject weapon;
	public bool enemySpotted;
	public string enemyTag = "Player";
	public Transform enemy;
	public Transform weaponSpawnPoint;
	public int attackDistance = 5;
	public int minAttackRate = 1;
	public int maxAttackRate = 5;
	public int minDamage = 5;
	public int maxDamage = 10;

	Vector3 mRot = Vector3.zero;

	float nextAttack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (enemySpotted)
		{
			transform.LookAt(enemy, Vector3.up);
			mRot = transform.localEulerAngles;
			mRot.x = 0;
			mRot.z = 0;
			transform.localEulerAngles = mRot;

			if (Vector3.Distance(transform.position, enemy.position) <= attackDistance)
			{
				if (Time.time > nextAttack)
				{
					GameObject go = Spawner.Instantiate(weapon) as GameObject;
					go.transform.position = weaponSpawnPoint != null ? weaponSpawnPoint.position : transform.position;
					//go.transform.rotation = transform.rotation;
					Ball axe = go.GetComponent<Ball>();
					if (axe != null)
					{
						axe.ownerTransform = transform;
						axe.damage = NGUITools.RandomRange(minDamage, maxDamage);
					}
					nextAttack = Time.time + (float)NGUITools.RandomRange(minAttackRate, maxAttackRate);
				}
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.CompareTag(enemyTag))
		{
			enemy = col.transform;
			enemySpotted = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.CompareTag(enemyTag))
		{
			enemySpotted = false;
		}
	}
}
