using UnityEngine;
using System.Collections;

public class Wall : CellEdge {

	public override void Initialize (Cell cell, Cell otherCell, MapDirection direction) {
		base.Initialize(cell, otherCell, direction);
		if(cell != null)
		{
			//transform.Find("Wall").GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
		}

	}
}
