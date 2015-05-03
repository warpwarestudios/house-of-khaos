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
			room.Add(cell);
			cell.transform.parent = room.transform;
		}
	}
	public void ChangeSettings(MapRoomSettings newSettings)
	{
		settings = newSettings;

		foreach (Cell cell in cells) 
		{
			cell.ChangeColor();
		}
	}

	public void InitializeTextures()
	{
		foreach(Cell cell in cells.ToArray())
		{
			foreach(Transform child in cell.transform)
			{
				if(child.name == "Floor")
				{
					child.GetComponent<Renderer>().material = settings.floorMaterial;
				}
				if(child.name == "Wall(Clone)")
				{
					child.transform.Find ("Wall").GetComponent<Renderer>().material = settings.wallMaterial;
				}
				
				if(child.name == "Roof")
				{
					child.transform.GetComponent<Renderer>().material = settings.wallMaterial;
				}
				
				if(child.name == "Wall_Lamp(Clone)")
				{
					child.transform.Find ("Wall").GetComponent<Renderer>().material = settings.wallMaterial;
				}
				
				if(child.name == "Wall_Window(Clone)")
				{
					child.transform.Find ("Left").GetComponent<Renderer>().material = settings.wallMaterial;
					child.transform.Find ("Top").GetComponent<Renderer>().material = settings.wallMaterial;
					child.transform.Find ("Right").GetComponent<Renderer>().material = settings.wallMaterial;
					child.transform.Find ("Bottom").GetComponent<Renderer>().material = settings.wallMaterial;
				}
				
				if(child.name == "Door(Clone)")
				{
					child.transform.Find ("Left").GetComponent<Renderer>().material = settings.wallMaterial;
					child.transform.Find ("Top").GetComponent<Renderer>().material = settings.wallMaterial;
					child.transform.Find ("Right").GetComponent<Renderer>().material = settings.wallMaterial;
				}
			}
		}
	}

	public List<Cell> returnOutsideWallsList(Map map)
	{
		List<Cell> activeCells = new List<Cell> ();
		foreach (Cell cell in cells) 
		{

			//Debug.Log (cell.name);
			//check if it borders the outside
			//check each direction
			for(int i = 0; i < 4; i++)
			{
				MapDirection direction = (MapDirection)i;
				if(cell.GetEdge(direction).GetType() == typeof(Wall))
				{
					IntVector2 coordinates = cell.coordinates + direction.ToIntVector2();
					//if neighbor exists then...
					if(coordinates.x < map.size.x  && coordinates.z < map.size.z && coordinates.x > 0 && coordinates.z > 0)
					{


						Cell neighbor = map.GetCell(coordinates);

						//if neighbor does not exist in grid
						if (neighbor == null || neighbor.room != this)
						{
							//add to active cells
							activeCells.Add(cell);
							break;
						}
					}
				}
				
			}
			//add to active cells
		}
		return activeCells;
	}

	public Cell GetRandomPosition()
	{
		return cells[Random.Range(0, cells.Count - 1)];
	}

	public bool InRoom (Cell cell) {
		//if cell is in list, return true
		if (cells.Contains (cell)) {
			return true;
		};
		//otherwise return false
		return false;
	}

	public void UpdateSpawnPoints()
	{
		foreach (Cell cell in cells) 
		{
			if(cell.itemSpawn.GetComponent<ItemSpawn>().hasSpawned == true)
			{
				Destroy(cell.playerSpawn);
				Destroy(cell.waypoint);
			}
			else
			{
				//Destroy(cell.itemSpawn);
			}
		}
	}
}
