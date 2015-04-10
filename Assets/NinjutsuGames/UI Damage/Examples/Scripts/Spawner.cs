using UnityEngine;
using System.Collections;

[System.Serializable]
public class ObjectCache {
	public GameObject prefab;
	public int cacheSize = 10;
	
	GameObject[] objects;
	int cacheIndex = 0;
	
	public void Initialize () {
		objects = new GameObject[cacheSize];
		
		// Instantiate the objects in the array and set them to be inactive
		for (var i = 0; i < cacheSize; i++) {
			objects[i] = MonoBehaviour.Instantiate (prefab) as GameObject;
			NGUITools.SetActive(objects[i], false);
			objects[i].name = objects[i].name + i;

			// Set this transform as its parent
			objects[i].transform.parent = Spawner.spawner.transform;
		}
	}
	
	public GameObject GetNextObjectInCache () {
		GameObject obj = null;
		
		// The cacheIndex starts out at the position of the object created
		// the longest time ago, so that one is usually free,
		// but in case not, loop through the cache until we find a free one.
		for (int i = 0; i < cacheSize; i++) {
			obj = objects[cacheIndex];
			
			// If we found an inactive object in the cache, use that.
#if UNITY_3_5
			if (!obj.active)
				break;
#else
			if (!obj.activeInHierarchy)
				break;
#endif
			// If not, increment index and make it loop around
			// if it exceeds the size of the cache
			cacheIndex = (cacheIndex + 1) % cacheSize;
		}
		
		// The object should be inactive. If it's not, log a warning and use
		// the object created the longest ago even though it's still active.
#if UNITY_3_5
		if (!obj.active)
#else
		if (!obj.activeInHierarchy)
#endif
		{
			Debug.LogWarning (
				"Spawn of " + prefab.name +
				" exceeds cache size of " + cacheSize +
				"! Reusing already active object.", obj);
			Spawner.Destroy (obj);
		}
		
		// Increment index and make it loop around
		// if it exceeds the size of the cache
		cacheIndex = (cacheIndex + 1) % cacheSize;
		
		return obj;
	}
}

public class Spawner : MonoBehaviour {

	public static Spawner spawner;
	public ObjectCache[] caches;
	Hashtable activeCachedObjects;

	void Awake () {
		// Set the global variable
		spawner = this;	
	
		// Total number of cached objects
		int amount = 0;
	
		// Loop through the caches
		for (int i = 0, imax = caches.Length; i < imax; i++) {
			// Initialize each cache
			caches[i].Initialize();
		
			// Count
			amount += caches[i].cacheSize;
		}
	
		// Create a hashtable with the capacity set to the amount of cached objects specified
		activeCachedObjects = new Hashtable (amount);
	}

	public static GameObject Spawn (GameObject prefab, Vector3 position, Quaternion rotation) {
		ObjectCache cache = null;
	
		// Find the cache for the specified prefab
		if (spawner) {
			for (int i = 0, imax = spawner.caches.Length; i < imax; i++) {
				if (spawner.caches[i].prefab == prefab) {
					cache = spawner.caches[i];
				}
			}
		}
	
		// If there's no cache for this prefab type, just instantiate normally
		if (cache == null) {
			return Instantiate (prefab, position, rotation) as GameObject;
		}
	
		// Find the next object in the cache
		GameObject obj = cache.GetNextObjectInCache();

		// Set this transform as its parent
		obj.transform.parent = spawner.transform;
	
		// Set the position and rotation of the object
		obj.transform.position = position;
		obj.transform.rotation = rotation;
	
		// Set the object to be active
		NGUITools.SetActive(obj, true);
		spawner.activeCachedObjects[obj] = true;
	
		return obj;
	}

	public static void Destroy (GameObject objectToDestroy) {
		if (spawner && spawner.activeCachedObjects.ContainsKey (objectToDestroy)) {
			NGUITools.SetActive(objectToDestroy, false);
			spawner.activeCachedObjects[objectToDestroy] = false;
		}
		else {
			GameObject.Destroy(objectToDestroy);
		}
	}
}
