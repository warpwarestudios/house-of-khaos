using UnityEngine;
using System.Collections;

public class PlayerHealthScreen : MonoBehaviour {
	
	public Health health;
	
	// sign up for the event when you're enabled
	void OnEnable () {
		if (health) health.modifyHealth += ChangeHealth;
	}
	
	// stop listening when you're disabled
	void OnDisable () {
		if (health) health.modifyHealth -= ChangeHealth;
	}
	
	void ChangeHealth () {
		UIDamage.SetMinScreenAlpha(1 - (health.currentHealth / health.maxHealth));
	}
	
	
	
}
