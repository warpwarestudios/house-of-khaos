using UnityEngine;
using System.Collections;

public class Wall : CellEdge {

	public override void Initialize (Cell cell, Cell otherCell, MapDirection direction) {
		base.Initialize(cell, otherCell, direction);
		if(cell.room != null)
		{
			Transform itemSpawn;
			//50/50 chance of clock or bookshelf
			if(Random.value < 0.9)
			{
				itemSpawn = this.transform.FindChild("Item Spawn - Bookshelf");
			}
			else
			{	
				itemSpawn = this.transform.FindChild("Item Spawn - Clock");
			}
			if(itemSpawn != null)
			{
				itemSpawn.GetComponent<ItemSpawn>().enabled = true;
			}
		}

	}

}
