using UnityEngine;
using System.Collections;

public class UnitHealth : MonoBehaviour {

	public float maxHealth = 100;
	public float health = 100;
	public float regenerateSpeed = 0.0f;
	public bool invincible = false;
	public bool dead = false;
	public bool canRespawn = true;
	public float respawnTime = 5;

	public GameObject damagePrefab;
	public Transform damageEffectTransform;
	public float damageEffectMultiplier = 1.0f;
	public bool damageEffectCentered = true;

	public GameObject ragdollPrefab = null;

	public GameObject scorchMarkPrefab = null;
	GameObject scorchMark = null;

	public GameObject model;

	public ThirdPersonControllerDemo controller;

	ParticleSystem damageEffect;
	float damageEffectCenterYOffset;

	float colliderRadiusHeuristic = 1.0f;
	Vector3 respawnPosition;

	float lastDamageTime = 0;

	void Awake () {
		enabled = false;
		respawnPosition = transform.position;
		if (damagePrefab) {
			if (damageEffectTransform == null)
				damageEffectTransform = transform;

			GameObject effect = Spawner.Instantiate(damagePrefab, Vector3.zero, Quaternion.identity) as GameObject;
			effect.transform.parent = damageEffectTransform;
			effect.transform.localPosition = Vector3.zero;
			if (effect.GetComponent<AutoDespawn>() != null) Destroy(effect.GetComponent<AutoDespawn>());
#if UNITY_4
			damageEffect = effect.particleSystem;
#else
			damageEffect = effect.GetComponent<ParticleSystem>();
#endif
			damageEffect.Stop();
			Vector2 tempSize = new Vector2(GetComponent<Collider>().bounds.extents.x,GetComponent<Collider>().bounds.extents.z);
			colliderRadiusHeuristic = tempSize.magnitude * 0.5f;
			damageEffectCenterYOffset = GetComponent<Collider>().bounds.extents.y;		
		}

		if (scorchMarkPrefab)
		{
			scorchMark = Spawner.Instantiate(scorchMarkPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			NGUITools.SetActive(scorchMark, false);
		}
	}

	public void OnDamage (string killer, float amount, Vector3 fromDirection) {
		
		// Take no damage if invincible, dead, or if the damage is zero
		if(invincible)
			return;
		if (dead)
			return;
		if (amount <= 0)
			return;

		lastDamageTime = Time.time + 3f;

		// Decrease health by damage and send damage signals	
		health -= amount;
		if (health < 0) health = 0;		
	
		// Enable so the Update function will be called
		// if regeneration is enabled
		if (regenerateSpeed > 0)
			enabled = true;
	
		// Show damage effect if there is one
		if (damageEffect) {
			damageEffect.transform.rotation = Quaternion.LookRotation(fromDirection, Vector3.up);
			if(!damageEffectCentered) {
				Vector3 dir = fromDirection;
				dir.y = 0.0f;
				damageEffect.transform.position = (transform.position + Vector3.up * damageEffectCenterYOffset) + colliderRadiusHeuristic * dir;
			}
			damageEffect.Play();
		}

		if (health == 0) Die();
	}

	void Die()
	{
		if (!dead)
		{
			health = 0;
			dead = true;
			enabled = false;
			controller.enabled = false;

			if (model != null) NGUITools.SetActive(model, false);
			if (ragdollPrefab != null) Spawner.Instantiate(ragdollPrefab, transform.localPosition, transform.rotation);			

			// scorch marks
			if (scorchMark)
			{
				NGUITools.SetActive(scorchMark, true);
				// @NOTE: maybe we can justify a raycast here so we can place the mark
				// on slopes with proper normal alignments
				// @TODO: spawn a yield Sub() to handle placement, as we can
				// spread calculations over several frames => cheap in total
				Vector3 scorchPosition = GetComponent<Collider>().ClosestPointOnBounds(transform.position - Vector3.up * 100);
				scorchMark.transform.position = scorchPosition + Vector3.up * 0.1f;
				scorchMark.transform.eulerAngles = new Vector3(scorchMark.transform.eulerAngles.x, Random.Range(0.0f, 90.0f), scorchMark.transform.eulerAngles.z);
			}

			if (canRespawn) StartCoroutine(StartResurrection());
		}
	}

	IEnumerator StartResurrection()
	{
		yield return new WaitForSeconds(respawnTime);

		Resurrect();
	}

	void Resurrect()
	{
		transform.position = respawnPosition;
		controller.enabled = true;
		if (model != null) NGUITools.SetActive(model, true);
		Reset();
	}

	void Reset()
	{
		if(UIDamage.instance != null) UIDamage.ClearScreen();
		dead = false;
		health = maxHealth;
	}

	void OnEnable () {
		StartCoroutine(Regenerate());	
	}

	IEnumerator Regenerate () {
		if (regenerateSpeed > 0.0f) {
			while (enabled) {
				if (Time.time > lastDamageTime)
				{
					health += regenerateSpeed;

					// Modify the minimun alpha of the screen overlay if the health is above 40%
					if (health > (maxHealth * 0.4f)) UIDamage.SetMinScreenAlpha(1 - (health / maxHealth));

					if (health >= maxHealth)
					{
						health = maxHealth;
						enabled = false;
					}
				}
				yield return new WaitForSeconds (1.0f);
			}
		}
	}
}




