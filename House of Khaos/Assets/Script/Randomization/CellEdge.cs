using UnityEngine;
using System.Collections;

public abstract class CellEdge : MonoBehaviour {

	public Cell cell, otherCell;
	public MapDirection direction;

	public void Initialize (Cell cell, Cell otherCell, MapDirection direction) {
		this.cell = cell;
		this.otherCell = otherCell;
		this.direction = direction;
		cell.SetEdge(direction, this);
		transform.parent = cell.transform;
		transform.localPosition = Vector3.zero;
		transform.localRotation = direction.ToRotation ();
	}
}
