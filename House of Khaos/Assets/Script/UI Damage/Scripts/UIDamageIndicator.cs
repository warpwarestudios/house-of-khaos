using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDamageIndicator : MonoBehaviour
{
	/// <summary>
	/// The sprite that is going to be display
	/// </summary>
	
	public UIWidget sprite;

	/// <summary>
	/// Target that is going to be follow
	/// </summary>

	public Transform target;

	/// <summary>
	/// Where the attack is coming.
	/// </summary>

	public Vector3 attackDirection;

	/// <summary>
	/// Cache transform for speed
	/// </summary>
	
	public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }

	/// <summary>
	/// How fast should be this widget faded out
	/// </summary>

	public float damageFadeSpeed = 0.2f;

	/// <summary>
	/// Alpha of this widget
	/// </summary>

	public float alpha = 1.0f;

	float rotationOffset = 0.0f;
	Transform mTrans;
	int mAttacker;

	/// <summary>
	/// Updates the position and alpha of this widget
	/// </summary>

	public void UpdateDamage()
	{
		alpha -= Time.deltaTime * damageFadeSpeed;

		Vector3 damageFrom = attackDirection - target.position;
		damageFrom.y = 0;
		damageFrom.Normalize();

		Vector3 cameraForward = Camera.main.transform.forward;
		float direction = Vector3.Dot(cameraForward, damageFrom);

		if (Vector3.Cross(cameraForward, damageFrom).y > 0)
			rotationOffset = (1.0f - direction) * -90;
		else
			rotationOffset = (1.0f - direction) * 90;

		cachedTransform.localRotation = Quaternion.Euler(0f, 0f, (rotationOffset));

		if(sprite != null) 
			sprite.alpha = alpha;
	}
}