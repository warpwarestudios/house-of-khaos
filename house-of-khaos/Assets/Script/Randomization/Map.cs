using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public IntVector2 size;
	public int numRooms;
	public Cell cellPrefab;
	public MapRoom roomPrefab;
	public Passage passagePrefab;
	public Wall wallPrefab;
	public Door doorPrefab;
	public float generationStepDelay;
	public float doorProbability;
	public MapRoomSettings[] roomSettings;
	public IntVector2 minRoomSize;
	public IntVector2 maxRoomSize;

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	private Cell[,] cells;
	private List<MapRoom> rooms = new List<MapRoom>();
	private List<Cell> connectors = new List<Cell>();

	public IEnumerator Generate () {
		WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
		cells = new Cell[size.x, size.z];
		//create active cell list
		List<Cell> activeCells = new List<Cell> ();
		//create all cells
		for (int x = 0; x < size.x; x++) {
			for (int z = 0; z < size.z; z++) {
				CreateCell(new IntVector2(x, z));
			}
		}

		//Step 1: create rooms
		for (int i = 0; i < numRooms; i++) 
		{
			CreateRoom();
		}

		//Step 2: generate maze in parts that aren't rooms
		DoFirstGenerationStep (activeCells);
		while (activeCells.Count > 0) 
		{
			DoNextGenerationStep(activeCells);
		}

		//Step 3: Generate connections between rooms
		activeCells.Clear();
		List<MapRoom> connectedRooms = new List<MapRoom> ();
		DoFirstConnectionStep (connectedRooms);
		//Choose a random room
		while (rooms.Count != connectedRooms.Count) 
		{
			yield return delay;
			DoNextConnectionStep(connectedRooms,activeCells);
		}


	}

	private void DoFirstGenerationStep (List<Cell> activeCells) {
		activeCells.Add(GetCell(RandomCoordinates));
	}
	
	private void DoNextGenerationStep (List<Cell> activeCells) {
		int currentIndex = activeCells.Count - 1;
		Cell currentCell = activeCells[currentIndex];
		if (currentCell.IsFullyInitialized) 
		{
			activeCells.RemoveAt(currentIndex);
			return;
		}
		MapDirection direction = currentCell.RandomUninitializedDirection;
		IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
		//make sure we are still in the bounds
		if (ContainsCoordinates(coordinates)) {
			Cell neighbor = GetCell(coordinates);

			//if hall to hall generate maze
			if (currentCell.room == null && neighbor.room == null)
			{
				//creates hall
				if (!activeCells.Contains(neighbor))
				{
					CreatePassage(currentCell, neighbor, direction);
					activeCells.Add(neighbor);
				}
				else
				{
					CreateWall(currentCell, neighbor, direction);
				}
			}
			//if room to same room create passage
			else if (currentCell.room == neighbor.room)
			{
				CreatePassageInSameRoom(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}
			//if room to halls, or room to different room create wall or door
			else
			{
				CreateWall(currentCell, neighbor, direction);
				activeCells.Add(neighbor);
			}

		}
		else {
			//create bounding walls
			CreateWall(currentCell, null, direction);
		}
	}

	private void DoFirstConnectionStep(List<MapRoom> connectedRooms)
	{
		MapRoom first = rooms[0];
		//setting color so I know it's been added
		first.settingsIndex = 1;
		first.ChangeColor (roomSettings[first.settingsIndex]);
		connectedRooms.Add (first);

	}

	private void DoNextConnectionStep(List<MapRoom> connectedRooms, List<Cell> activeCells)
	{
		//get last room
		MapRoom room = connectedRooms[connectedRooms.Count - 1];
		//get all connecting cells in that room
		foreach (Cell cell in connectors) 
		{
			if(room = cell.room)
			{
				activeCells.Add(cell);
			}
		}
		//choose a random connection and create a door there
		//if it is a hallway traverse the hallway and find a room
		//if it is another room add to connected region
		//temp to be sure you get out of while loop
		connectedRooms.Add (rooms [0]);
	}

	private void CreateCell (IntVector2 coordinates) 
	{
		Cell newCell = Instantiate (cellPrefab) as Cell;
		cells [coordinates.x, coordinates.z] = newCell;
		newCell.coordinates = coordinates;
		newCell.name = "Cell " + coordinates.x + ", " + coordinates.z;
		newCell.transform.parent = transform;
		newCell.room = null;
		newCell.transform.localPosition =
			new Vector3 (coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
	}

	private void CreatePassage (Cell cell, Cell otherCell, MapDirection direction) {
		Passage passage = Instantiate(passagePrefab) as Passage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as Passage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateDoor (Cell cell, Cell otherCell, MapDirection direction) {
		Passage passage = Instantiate(doorPrefab) as Passage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as Passage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreatePassageInSameRoom (Cell cell, Cell otherCell, MapDirection direction) {
		Passage passage = Instantiate(passagePrefab) as Passage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as Passage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall (Cell cell, Cell otherCell, MapDirection direction) {
		Wall wall = Instantiate(wallPrefab) as Wall;
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			wall = Instantiate(wallPrefab) as Wall;
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}

	private void CreateRoom () {
		//generate rooms, unless it overlaps. Destroy on overlap.
		MapRoom newRoom = Instantiate (roomPrefab) as MapRoom;
		newRoom.size = new IntVector2 (Random.Range(minRoomSize.z,maxRoomSize.x), Random.Range(minRoomSize.z, maxRoomSize.z));
		bool overlap = false;
		//TODO: add randomization elements
		newRoom.settingsIndex = 0;
		newRoom.settings = roomSettings[newRoom.settingsIndex];
		newRoom.name = "Room " + newRoom.size.x + " x " + newRoom.size.z;
		newRoom.transform.parent = transform;
		newRoom.transform.localPosition = new Vector3 (0, 0, 0);
		//randomize room starting position
		IntVector2 offset = new IntVector2 (Random.Range(0, size.x -1), Random.Range(0, size.x -1));
		//if the offset and the room size is larger than the size, randomize again
		while (offset.x + newRoom.size.x > size.x - 1) 
		{
			offset.x = Random.Range (0, size.x -1);
		}
		while (offset.z + newRoom.size.z > size.z - 1 ) 
		{
			offset.z = Random.Range (0, size.x -1);
		}
		//get all cells in room and add them as children and to list
		for (int i = 1 + offset.x; i <= offset.x + newRoom.size.x; i++) 
		{
			for (int j = 1 + offset.z; j <= offset.z + newRoom.size.z; j++) 
			{
				if (!overlap)
				{
					//find Cell to add to room
					Cell newCell = GetCell(new IntVector2(i,j));
					//check and see if cell already has room
					if(newCell.room != null)
					{
						overlap = true;
					}
					else
					{
						//initialize cell
						newCell.Initialize(newRoom);
						//parent cell to room
						newCell.transform.parent = newRoom.transform;
					}
				}
			}	
		}
		//if room overlapped, destroy it
		if (overlap) {
			newRoom.Cleanup(this.transform);
			Destroy (newRoom.gameObject);
		} else 
		{
			rooms.Add (newRoom);
		}
	}

	public bool ContainsCoordinates (IntVector2 coordinate) {
		return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
	}

	public Cell GetCell (IntVector2 coordinates) {
		return cells[coordinates.x, coordinates.z];
	}

}
