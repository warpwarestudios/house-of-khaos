using UnityEngine;
using System.Collections;

public class KillCountUI : MonoBehaviour {
	
	public int countTotalEnemy = 0;
	public int enemyDeathCount = 0;
	public int totalEnemies;
	public int enemiesLeft;
	public bool gameFinished;
	
	void Update()
	{
		totalEnemies = countTotalEnemy;
		enemiesLeft = (totalEnemies + enemyDeathCount);
		UpdateKillCount();
	}
	
	public void UpdateKillCount()
	{
		this.gameObject.GetComponent<UILabel>().text = (enemiesLeft + "/" + totalEnemies);
		
		if(enemiesLeft <= 0)
		{
			gameFinished = true;
		}
		if(gameFinished == true)
		{
			GameObject.Find("UI Root").transform.FindChild("Message").GetComponent<UILabel>().text = "Escape the House of Khaos! Quickly";
		}
	}
	
	public void countEnemy()
	{
		countTotalEnemy++;
	}
	
	public void EnemyDeathCount()
	{
		enemyDeathCount--;
	}
}
