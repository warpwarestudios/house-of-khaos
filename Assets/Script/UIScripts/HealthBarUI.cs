using UnityEngine;
using System.Collections;

public class HealthBarUI : MonoBehaviour {
	
	public Health health;
	
	// Update is called once per frame
	void Update () {
		GameObject.Find("UI Root").transform.FindChild("HealthBar").GetComponent<UISlider>().value = ((health.currentHealth / health.maxHealth));
		GameObject.Find("UI Root").transform.FindChild("HealthBar").GetComponent<UISlider>().alpha = ((health.currentHealth / health.maxHealth));

		GameObject.Find("UI Root").transform.FindChild("HealthBar").transform.FindChild("HP").GetComponent<UILabel>().text = ((health.currentHealth / health.maxHealth) * 100) + "%";
	}
}
