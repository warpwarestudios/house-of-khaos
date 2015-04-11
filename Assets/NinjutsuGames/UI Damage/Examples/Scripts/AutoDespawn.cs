using UnityEngine;
using System.Collections;

public class AutoDespawn : MonoBehaviour {

	public float time = 5f;

	IEnumerator Start () {
		yield return new WaitForSeconds(time);
		Spawner.Destroy(gameObject);
	}
}
