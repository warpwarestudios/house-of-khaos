﻿using UnityEngine;
using System.Collections;

public class Health: MonoBehaviour {
	
	public int maxHealth = 100;
	public int currentHealth = 100;
	
	public delegate void ModifyHealth ();
	public event ModifyHealth modifyHealth = delegate {};
	
	
	public void updateHealth(float health)
	{
		currentHealth += (int) health;
		Debug.Log ("Health: " + currentHealth + "/"  + maxHealth);

		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}

		if (currentHealth <= 0) {
			Destroy(this.gameObject);
		}

		modifyHealth();

	}
	
	
	
	
//	void OnCollisionEnter (Collision collision) {
//		Damage damage = collision.GetComponent<Damage>();
//		if (damage) {
//			currentHealth -= damage.amountOfDamage;
//			onDamageTaken();
//		}
//	}
}