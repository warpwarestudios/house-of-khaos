using UnityEngine;
using System.Collections;

public class Door : Passage {

	public Transform hinge;

	public override void Initialize (Cell cell, Cell otherCell, MapDirection direction) {
		base.Initialize(cell, otherCell, direction);
		
//		for (int i = 0; i < transform.childCount; i++) {
//			Transform child = transform.GetChild(i);
//			if (child != hinge) {
//				child.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
//			}
//		}
	}
}
