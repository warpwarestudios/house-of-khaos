using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapRoom : MonoBehaviour {

	public IntVector2 size;

	public int settingsIndex;
	
	public MapRoomSettings settings;
	
	private List<Cell> cells = new List<Cell>();
	
	public void Add (Cell cell) {
		cell.room = this;
		cells.Add(cell);
	}

	//reset and reparent all cells in the current list
	public void Cleanup(Transform cellParent)
	{
		foreach (Cell cell in cells) 
		{
			cell.transform.parent = cellParent;
			cell.room = null;
			cell.ResetMaterial();
		}
	}

	public void MergeInto(MapRoom room)
	{
		//merges this room into the given room
		foreach (Cell cell in cells.ToArray()) 
		{
			cell.Initialize(room);
		}
	}
	public void ChangeColor(MapRoomSettings newSettings)
	{
		settings = newSettings;

		foreach (Cell cell in cells) 
		{
			cell.ChangeColor();
		}
	}

	public Cell GetRandomPosition()
	{
		return cells[Random.Range(0, cells.Count - 1)];
	}
}
