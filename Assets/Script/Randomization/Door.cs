using UnityEngine;
using System.Collections;

public class Door : Passage {

	public Transform hinge;

	public override void Initialize (Cell cell, Cell otherCell, MapDirection direction) {
		base.Initialize(cell, otherCell, direction);
		
		GameObject itemSpawn = cell.transform.FindChild ("Floor").transform.FindChild ("Item Spawn").gameObject;
		
		Destroy (itemSpawn);

		GameObject otherItemSpawn = otherCell.transform.FindChild ("Floor").transform.FindChild ("Item Spawn").gameObject;

		Destroy (otherItemSpawn);
	}
}
