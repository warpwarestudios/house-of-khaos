using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISlider))]
public class UIHealth : MonoBehaviour {

	public UnitHealth health;
	public UILabel label;

	UISlider mSlider;
	float mVal = 0;

	void Awake()
	{
		mSlider = GetComponent<UISlider>();
	}

	void Update () {
		if (health != null && mSlider != null)
		{
			mVal = health.health / health.maxHealth;
			if (mSlider.value != mVal) mSlider.value = mVal;

			if (label != null)
			{
				label.text = health.health == 0 ? "Death" : health.health + "/" + health.maxHealth;
			}
		}		
	}
}
