using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public GameObject hitPrefab;
	public string hitTag = "Player";
	public int force = 100;
	public int damage = 25;
	public Transform ownerTransform;
	public string owner;
	public bool collided;
	//Spin mSpin;
	Transform mChild;

	void Awake()
	{
		//mSpin = GetComponentInChildren<Spin>();
		mChild = transform.GetChild(0);
	}

	void OnEnable()
	{
		StartCoroutine(Init());
	}

	IEnumerator Init()
	{
		//mSpin.enabled = true;
		collided = false;
		if (GetComponent<Rigidbody>() != null) Destroy(GetComponent<Rigidbody>());
		yield return new WaitForSeconds(5f);
		Spawner.Destroy(gameObject);
	}

	void Update()
	{
		if (ownerTransform != null && !collided)
			transform.position += Time.deltaTime * force * ownerTransform.forward;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (!collided)
		{
			collided = true;
			StartCoroutine(Collide(collision.gameObject));
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (!collided)
		{
			collided = true;
			StartCoroutine(Collide(col.gameObject));
		}
	}

	IEnumerator Collide(GameObject go)
	{		
		mChild.localPosition = Vector3.zero;
		collided = true;
		//mSpin.enabled = false;
		if (go.tag == hitTag)
		{
			UnitHealth h = go.GetComponent<UnitHealth>();
			h.OnDamage(owner, damage, -transform.forward);

			// Here we call UIDamage to show damage indicators and screen flash
			float screenFlashAlpha = (h.health / h.maxHealth) <= 0.4f ? 1f - (h.health / h.maxHealth) : 0;
			UIDamage.Show(ownerTransform.position, true, screenFlashAlpha, true);
		}
		if (gameObject.GetComponent<Rigidbody>() == null) gameObject.AddComponent<Rigidbody>();
		if (hitPrefab != null) Spawner.Instantiate(hitPrefab, transform.position, transform.rotation);
		yield return new WaitForSeconds(5f);
		Spawner.Destroy(gameObject);		
	}
}
