using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	public IntVector2 coordinates;
	public MapRoom room;
	public Material defaultMat;

	public GameObject playerSpawn;
	public GameObject itemSpawn;
	public GameObject waypoint;

	private CellEdge[] edges = new CellEdge[MapDirections.Count];
	private int initializedEdgeCount;
	
	public bool IsFullyInitialized {
		get {
			return initializedEdgeCount == MapDirections.Count;
		}
	}

	public void Initialize (MapRoom room) {
		room.Add(this);
		transform.Find("Floor").GetComponent<Renderer>().material = room.settings.floorMaterial;
		transform.parent = room.transform;
	}

	public void ChangeColor()
	{	
		transform.Find("Floor").GetComponent<Renderer>().material = room.settings.floorMaterial;
	}

	public void ResetMaterial()
	{
		transform.Find("Floor").GetComponent<Renderer>().material = defaultMat;
	}

	public CellEdge GetEdge (MapDirection direction) {
		return edges[(int)direction];
	}
	
	public void SetEdge (MapDirection direction, CellEdge edge) {
		edges[(int)direction] = edge;
		initializedEdgeCount += 1;
	}
	public MapDirection RandomUninitializedDirection {
		get {
			int skips = Random.Range(0, MapDirections.Count - initializedEdgeCount);
			for (int i = 0; i < MapDirections.Count; i++) {
				if (edges[i] == null) {
					if (skips == 0) {
						return (MapDirection)i;
					}
					skips -= 1;
				}
			}
			throw new System.InvalidOperationException("Cell has no uninitialized directions left.");
		}
	}
}
