using UnityEngine;
using System.Collections;

public class KillCountUI : MonoBehaviour {
	
	public int enemyDeathCount = 0;
	public int totalEnemies = 0;
	public bool gameFinished;
	
	void Update()
	{

		UpdateKillCount();
	}
	
	public void UpdateKillCount()
	{
		this.gameObject.GetComponent<UILabel>().text = (enemyDeathCount + "/" + totalEnemies);
		
		if(totalEnemies >= 100)
		{
			foreach(GameObject waypoint in Waypoint.waypointList)
			{
				waypoint.GetComponent<Waypoint>().canSpawn = false;
			}
			gameFinished = true;
		}
		if(gameFinished == true)
		{
			GameObject.Find("UI Root").transform.FindChild("Message").GetComponent<UILabel>().text = "You've survived! This time...";
		}

	}
	
	public void AddEnemy()
	{
		totalEnemies++;
	}
	
	public void AddEnemyDeath()
	{
		enemyDeathCount++;
	}
}
