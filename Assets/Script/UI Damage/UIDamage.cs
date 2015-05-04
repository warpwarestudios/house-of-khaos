using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Example Usage:
/// 
/// UIDamage.instance.OnDamage(ownerTransform, showScreenFlash, minimunAlpha, shake);
/// 
/// Parameters:
/// 1. Attacker Position: position of the enemy.
/// 2. Show scren flash: true or false
/// 3. Minimun Alpha: if #2 is true then you maybe would like to display the screen flash a bit more when you are dying, 
/// IE: your player is at 40% health then lets make the alpha so it doesnt go below certain amount. 
/// Example math: (health / maxUnitHealth) <= 0.4f ? 1f - (health / maxUnitHealth) : 0
/// 4. Shake camera effect: true or false
/// 
/// </summary>

public class UIDamage : MonoBehaviour
{

	#region Internal Classes

	[System.Serializable]
	public class ShakeEffect
	{
		public Transform camera;
		public int repeats = 20;
		public float speed = 40;
		public Vector2 distance = new Vector2(0.2f, 0.2f);
		[HideInInspector]
		public int currentRepeats = 0;
		[HideInInspector]
		public Vector3 mCameraPos;
		[HideInInspector]
		public Vector3 shakePos = Vector3.zero;
	}

	#endregion

	#region Public Variables

	/// <summary>
	/// Get instance.
	/// </summary>

	static public UIDamage instance { get { if (mInst == null) mInst = GameObject.FindObjectOfType(typeof(UIDamage)) as UIDamage; return mInst; } }	

	/// <summary>
	/// The target that UIDamageIndicator is going to follow.
	/// </summary>

	public Transform target;

	/// <summary>
	/// The target can be found using this tag.
	/// </summary>

	public string targetTag = "Player";

	/// <summary>
	/// Prefab that is going to be shown when OnDamage is called.
	/// </summary>
	
	public GameObject damageIndicator;

	/// <summary>
	/// Widget that is going to be used for screen flash effect.
	/// </summary>

	public UIWidget screenFlash;

	/// <summary>
	/// How fast the damage indicators should be faded out. Greater > faster.
	/// </summary>

	public float damageFadeSpeed = 0.2f;

	/// <summary>
	/// How fast the screen flash should be faded out. Greater > faster.
	/// </summary>

	public float screenFadeSpeed = 1f;

	/// <summary>
	/// The minimun alpha that is going to use the screen flash on fadeout. 
	/// Increase this value for example if your player is dying and is at 30% health set this to 0.6f to make the screen flash stay.
	/// </summary>

	public float minAlpha = 0f;

	/// <summary>
	/// How often the widgets should be updated.
	/// </summary>
	
	public float refreshRate = 0.01f;

	public ShakeEffect shakeEffect = new ShakeEffect();

	#endregion

	#region Private Variables

	static UIDamage mInst;

	static BetterList<UIDamageIndicator> mList = new BetterList<UIDamageIndicator>();
	static BetterList<UIDamageIndicator> mUnused = new BetterList<UIDamageIndicator>();	
	static int counter = 0;
	static bool updatingDamage = false;
	static bool updatingFlash = false;
	static bool shaking = false;

	#endregion

	#region Private Methods

	/// <summary>
	/// Create a new entry, reusing an old entry if necessary.
	/// </summary>

	static UIDamageIndicator Create()
	{
		// See if an unused entry can be reused
		if (mUnused.size > 0)
		{
			UIDamageIndicator ent = mUnused[mUnused.size - 1];
			mUnused.RemoveAt(mUnused.size - 1);
			NGUITools.SetActive(ent.gameObject, true);
			mList.Add(ent);
			return ent;
		}

		// New entry
		GameObject go = NGUITools.AddChild(instance.gameObject, instance.damageIndicator);
		UIDamageIndicator ne = go.GetComponent<UIDamageIndicator>();
		mList.Add(ne);
		++counter;
		return ne;
	}

	/// <summary>
	/// Delete the specified entry, adding it to the unused list.
	/// </summary>

	void Delete(UIDamageIndicator ent)
	{
		mList.Remove(ent);
		mUnused.Add(ent);
		NGUITools.SetActive(ent.gameObject, false);
	}

	/// <summary>
	/// Find a sprite for the screen flash.
	/// </summary>

	void Start()
	{
		shakeEffect.currentRepeats = shakeEffect.repeats;
		if (shakeEffect.camera == null) shakeEffect.camera = Camera.main.transform;
		shakeEffect.mCameraPos = shakeEffect.camera.localPosition;
		if (screenFlash == null) screenFlash = GetComponentInChildren<UISprite>();
		if (screenFlash != null)
		{
			screenFlash.alpha = 0;
			UIStretch st = screenFlash.gameObject.GetComponent<UIStretch>();
			if (st == null) st = screenFlash.gameObject.AddComponent<UIStretch>();
			if (st != null) st.style = UIStretch.Style.Both;
		}
	}

	/// <summary>
	/// Update what's necessary.
	/// </summary>

	void Update()
	{
		if (target == null)
		{
			if (GameObject.FindGameObjectWithTag(targetTag) != null) target = GameObject.FindGameObjectWithTag(targetTag).transform;
			enabled = false;
		}
	}

	/// <summary>
	/// Clean up.
	/// </summary>
	
	void OnDestroy() 
	{
		mList.Clear();
		mUnused.Clear();
	}	

	/// <summary>
	/// Fades out screen flash
	/// </summary>

	IEnumerator ShowScreenFlash()
	{
		while (updatingFlash)
		{
			screenFlash.alpha -= Time.deltaTime * screenFadeSpeed;
			if (screenFlash.alpha <= minAlpha) updatingFlash = false;
			yield return new WaitForSeconds(refreshRate);
		}
	}

	/// <summary>
	/// Fades out and move damage indicators
	/// </summary>
	
	IEnumerator UpdateDamage()
	{
		while (updatingDamage)
		{
			for (int i = 0, imax = mList.size; i < imax; i++)
			{				
				UIDamageIndicator di = mList[i];
				if (di != null)
				{
					di.UpdateDamage();
					if (di.alpha <= 0) Delete(di);
				}				
			}
			if (mList.size <= 0) updatingDamage = false;
			yield return new WaitForSeconds(refreshRate);
		}
	}

	IEnumerator ShakeCamera()
	{
		shakeEffect.mCameraPos = shakeEffect.camera.localPosition;

		while (shaking)
		{
			shakeEffect.currentRepeats--;
			//Calculate percentage of blend between stationary and bobbing camera positions
			float percentage = 1;// Mathf.Min(1, Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal")));

			//Calculate desired x position
			float desiredPosX = shakeEffect.distance.x * Mathf.Sin(Time.time * shakeEffect.speed + Mathf.PI / 2);
			//Blend between stationary and desired x position
			float newX = ((1 - percentage)) + (desiredPosX * (percentage));

			//Calculate desired y position
			float desiredPosY = shakeEffect.distance.y * Mathf.Sin(Time.time * 2 * shakeEffect.speed);
			//Blend between stationary and desired y position
			float newY = ((1 - percentage)) + (desiredPosY * (percentage));

			shakeEffect.shakePos.x = shakeEffect.mCameraPos.x + newX;
			shakeEffect.shakePos.y = shakeEffect.mCameraPos.y + newY;
			shakeEffect.shakePos.z = shakeEffect.mCameraPos.z;

			shakeEffect.camera.localPosition = shakeEffect.shakePos;

			if (shakeEffect.currentRepeats <= 0)
			{
				if (shakeEffect.camera.localPosition != shakeEffect.mCameraPos) shakeEffect.camera.localPosition = shakeEffect.mCameraPos;
				shaking = false;
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Shows a damage indicator from the give direction.
	/// </summary>
	/// <param name="attackDirection"></param>
	
	static public void Show(Vector3 attackDirection)
	{
		Show(attackDirection, true, 0, true);
	}

	/// <summary>
	/// Shows a damage indicator from the give direction.
	/// </summary>
	/// <param name="attackDirection"></param>
	/// <param name="showScreenFlash"></param>
	/// <param name="screenAlpha"></param>
	/// <param name="shake"></param>

	static public void Show(Vector3 attackDirection, bool showScreenFlash, float minimunScreenAlpha, bool shake)
	{
		if (instance.target == null)
		{
			if (Application.isPlaying) Debug.LogWarning("No 'Target' has been defined");
			else instance.target = instance.transform;
			return;
		}

		if (instance.damageIndicator == null)
		{
			Debug.LogWarning("No 'Damage Indicator Prefab' has been defined");
			return;
		}

		instance.minAlpha = minimunScreenAlpha;

		if (showScreenFlash)		
			ScreenFlash();

		UIDamageIndicator di = Create();
		di.target = instance.target;
		di.attackDirection = attackDirection;
		di.damageFadeSpeed = instance.damageFadeSpeed;
		di.alpha = 1.0f;

		if (!updatingDamage)
		{
			updatingDamage = true;
			instance.StartCoroutine(instance.UpdateDamage());
		}

		if (shake) Shake();
	}

	/// <summary>
	/// Sets the minimun screen splah alpha
	/// </summary>

	static public void SetMinScreenAlpha(float alpha)
	{
		instance.minAlpha = alpha;
		if (!updatingFlash)
		{
			updatingFlash = true;
			instance.StartCoroutine(instance.ShowScreenFlash());
		}
	}

	/// <summary>
	/// Use this if you want to display the screen flash only.
	/// </summary>

	static public void ScreenFlash()
	{
		if (instance.screenFlash != null)
		{
			if (instance.screenFlash.alpha != 1)
			{
				instance.screenFlash.alpha = 1;
				if (!updatingFlash)
				{
					updatingFlash = true;
					instance.StartCoroutine(instance.ShowScreenFlash());
				}
			}
		}
	}

	/// <summary>
	/// Use this method if you want to fade out the screen flash.
	/// IE: your player has recovered more than half health.
	/// </summary>

	static public void ClearScreen()
	{
		instance.minAlpha = 0;
		if (instance.screenFlash != null)
		{
			if (instance.screenFlash.alpha > 0)
			{
				if (!updatingFlash)
				{
					updatingFlash = true;
					instance.StartCoroutine(instance.ShowScreenFlash());
				}
			}
		}

		for (int i = 0, imax = mList.size; i < imax; i++)
		{
			if (i < mList.size)
			{
				UIDamageIndicator di = mList[i];
				if (di != null)				
					di.alpha = 0;				
			}
		}
	}

	/// <summary>
	/// Shake camera
	/// </summary>

	static public void Shake()
	{
		if (!shaking)
		{
			instance.shakeEffect.currentRepeats = instance.shakeEffect.repeats;
			shaking = true;
			instance.StartCoroutine(instance.ShakeCamera());
		}
	}

	#endregion
}