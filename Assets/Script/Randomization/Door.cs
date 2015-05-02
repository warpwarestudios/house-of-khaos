using UnityEngine;
using System.Collections;

public class Door : Passage {

	public Transform hinge;

	public override void Initialize (Cell cell, Cell otherCell, MapDirection direction) {
		base.Initialize(cell, otherCell, direction);


		foreach (Transform child in cell.transform) 
		{
			if(child.name == "Item Spawn")
			{
				GameObject itemSpawn = cell.transform.FindChild ("Item Spawn").gameObject;
				Destroy (itemSpawn);
			}
		}


		foreach (Transform child in otherCell.transform) 
		{
			if(child.name == "Item Spawn")
			{
				GameObject otherItemSpawn = otherCell.transform.FindChild ("Item Spawn").gameObject;
				Destroy (otherItemSpawn);
			}
		}

	}
}
