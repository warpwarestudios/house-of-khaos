using UnityEngine;
using System.Collections;

public class Health: MonoBehaviour {
	
	public int maxHealth = 100;
	public int currentHealth = 100;
	public float regenerateSpeed = 2f;
	public float lastDamageTime = 0;
	public delegate void ModifyHealth ();
	public event ModifyHealth modifyHealth = delegate {};
	public bool damaged = false;
	
	void Start()
	{
	 	regenerateSpeed = 2f;
	}
	
	public void updateHealth(float health)
	{
		currentHealth += (int) health;
		Debug.Log ("Health: " + currentHealth + "/"  + maxHealth);

		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}

		if (currentHealth <= 0) {
			StartCoroutine("TimeDisappear");
		}

		modifyHealth();

	}

	IEnumerator TimeDisappear() 
	{
		yield return new WaitForSeconds(10);
		Destroy(this.gameObject);
	}
	
	void Update()
	{
		if(damaged == true)
		{
			StartCoroutine(Regenerate());
		}
		
	}
	
	IEnumerator Regenerate () {
		if (regenerateSpeed > 0.0f) {
			while (damaged == true) {
				if (Time.time > lastDamageTime)
				{
					updateHealth(regenerateSpeed);
					
					// Modify the minimun alpha of the screen overlay if the health is above 40%
					if (currentHealth > (maxHealth * 0.4f)) UIDamage.SetMinScreenAlpha(1 - (currentHealth / maxHealth));
					
					if (currentHealth >= maxHealth)
					{
						currentHealth = maxHealth;
						damaged = false;
					}
				}
				yield return new WaitForSeconds (1.0f);
			}
		}
		
	}
}
