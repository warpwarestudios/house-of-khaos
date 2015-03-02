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
	public MapRoom connectedRegion;

	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	private Cell[,] cells;
	public List<MapRoom> rooms = new List<MapRoom>();
	public List<MapRoom> halls = new List<MapRoom>();

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
		//get all connectors between rooms and hallways
		GetAllConnectors ();
		//join hallways together and add them to the list of rooms
		CreateHallways ();
		//add first room to connectedRooms
		DoFirstConnectionStep (connectedRooms);

		Debug.Log ("Rooms: " + rooms.Count);
		while (rooms.Count != connectedRooms.Count) 
		{
			yield return delay;
			DoNextConnectionStep(connectedRooms,activeCells);
			Debug.Log ("Connected Rooms: " + connectedRooms.Count);
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
			//if room to halls, or room to different room create wall
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

	private void GetAllConnectors()
	{
		foreach (Cell cell in cells) 
		{
			//check each direction
			for(int i = 0; i < 4; i++)
			{
				MapDirection direction = (MapDirection)i;
				if(cell.GetEdge(direction).GetType() == typeof(Wall))
				{
					IntVector2 coordinates = cell.coordinates + direction.ToIntVector2();
					if(coordinates.x < size.x  && coordinates.z < size.z && coordinates.x > 0 && coordinates.z > 0)
					{
						Cell neighbor = GetCell(coordinates);
						//if neighbor exists in grid, and the two cells are either different rooms
						//or hallway and room
						if (cell.room != neighbor.room)
						{
							if(!connectors.Contains(cell))
							{
								connectors.Add(cell);
							}
							if (!connectors.Contains(neighbor))
							{
								connectors.Add(neighbor);
							}
						}
					}

				}
			}
		}

	}
	private void GetConnectors(MapRoom room, List<Cell> activeCells)
	{
		foreach (Cell cell in connectors) 
		{
			if(room == cell.room)
			{
				activeCells.Add(cell);
			}
		}
	}
	private void RemoveConnectors(List<Cell> remove)
	{
		foreach (Cell cell in remove) 
		{
			connectors.Remove(cell);
		}
	}
	private void CreateHallways()
	{
		int count = 1;
		foreach (Cell cell in cells) 
		{
			if(cell.room == null)
			{
				//generate hallway
				MapRoom newHallway = Instantiate (roomPrefab) as MapRoom;
				newHallway.size = new IntVector2 (0,0);
				newHallway.settingsIndex = 2;
				newHallway.settings = roomSettings[newHallway.settingsIndex];
				newHallway.name = "Hallway " + count;
				newHallway.transform.parent = transform;
				newHallway.transform.localPosition = new Vector3 (0, 0, 0);

				cell.Initialize(newHallway);

				AddCellToHallway(cell,null,newHallway);

				halls.Add(newHallway);
				count++;
			}
		}


	}

	private Cell AddCellToHallway(Cell start, Cell last, MapRoom hallway)
	{		
		if (start == null) 
		{
			return last;
		} 
		else 
		{
			for(int i = 0; i < 4; i++)
			{
				MapDirection direction = (MapDirection)i;
				if(start.GetEdge(direction).GetType() == typeof(Passage))
				{
					IntVector2 coordinates = start.coordinates + direction.ToIntVector2();
					//if neighbor exists then...
					Cell next = GetCell(coordinates);
					if(next.room != hallway)
					{
						next.Initialize(hallway);
					}
					else
					{
						next = null;
					}
					AddCellToHallway(next, start, hallway);
				}
			}
		}
		return start;
		
	}


	private void DoFirstConnectionStep(List<MapRoom> connectedRooms)
	{
		//create region
		connectedRegion = Instantiate (roomPrefab) as MapRoom;
		connectedRegion.size = new IntVector2 (0,0);
		connectedRegion.settingsIndex = 1;
		connectedRegion.settings = roomSettings[connectedRegion.settingsIndex];
		connectedRegion.name = "Connected Region";
		connectedRegion.transform.parent = transform;
		connectedRegion.transform.localPosition = new Vector3 (0, 0, 0);
		//TODO: Randomize room selection
		MapRoom first = rooms[0];
		connectedRooms.Add (first);


	}

	private void DoNextConnectionStep(List<MapRoom> connectedRooms, List<Cell> activeCells)
	{
		//get last room
		MapRoom room = connectedRooms[connectedRooms.Count - 1];
		bool done = false;
		//get all connecting cells in that room
		GetConnectors (room, activeCells);
		room.MergeInto (connectedRegion);

		//generate random cell to receive door
		Cell door = activeCells[Random.Range (0, activeCells.Count - 1)];
		while (!CanCreateDoor(door) && !done) 
		{
			//remove current failure
			activeCells.Remove(door);
			//randomize from remaining
			if (activeCells.Count == 0)
			{
				done = true;
			}
			else
			{
				door = activeCells[Random.Range (0, activeCells.Count - 1)];
			}
		}
		for(int i = 0; i < 4; i++)
		{
			MapDirection direction = (MapDirection)i;
			if(door.GetEdge(direction).GetType() == typeof(Wall))
			{
				IntVector2 coordinates = door.coordinates + direction.ToIntVector2();
				//if neighbor exists then...
				if(coordinates.x < size.x  && coordinates.z < size.z && coordinates.x > 0 && coordinates.z > 0)
				{
					Cell neighbor = GetCell(coordinates);
					if(neighbor.room != connectedRegion)
					{
						Destroy(door.GetEdge(direction).gameObject);
						Destroy(neighbor.GetEdge(direction.GetOpposite()).gameObject);
						CreateDoor(door, neighbor, direction);
						//if it is a hallway add to region
						if(halls.Contains(neighbor.room))
						{
							//get connectors and attach to new room
							Cell newCell = ConnectHallToRoom(neighbor.room);
							if(newCell != null)
							{
								neighbor.room.MergeInto(connectedRegion);
								connectedRooms.Add(newCell.room);
							}
						}
						//if it is a room add to connected rooms
						else
						{
							connectedRooms.Add(neighbor.room);
						}
					}
				}
			}
		}

		activeCells.Clear();
	}

	private Cell ConnectHallToRoom(MapRoom room)
	{
		List<Cell> active = new List<Cell> ();
		GetConnectors (room, active);
		bool done = false;
		Cell neighbor = null;
		Cell door = active[Random.Range (0, active.Count - 1)];

		while (!CanCreateDoor(door) && !done) 
		{
			//remove current failure
			active.Remove(door);
			//randomize from remaining
			if (active.Count == 0)
			{
				done = true;
			}
			else
			{
				door = active[Random.Range (0, active.Count - 1)];
			}
		}
		for(int i = 0; i < 4; i++)
		{
			MapDirection direction = (MapDirection)i;
			if(door.GetEdge(direction).GetType() == typeof(Wall))
			{
				IntVector2 coordinates = door.coordinates + direction.ToIntVector2();
				//if neighbor exists then...
				if(coordinates.x < size.x  && coordinates.z < size.z && coordinates.x > 0 && coordinates.z > 0)
				{
					neighbor = GetCell(coordinates);
					if(neighbor.room != connectedRegion && rooms.Contains (neighbor.room))
					{
						Destroy(door.GetEdge(direction).gameObject);
						Destroy(neighbor.GetEdge(direction.GetOpposite()).gameObject);
						CreateDoor(door, neighbor, direction);
						return neighbor;
					}
				}
			}
		}
		return null;
	}


	private bool CanCreateDoor(Cell door)
	{
		for(int i = 0; i < 4; i++)
		{
			MapDirection direction = (MapDirection)i;
			if(door.GetEdge(direction).GetType() == typeof(Wall))
			{
				IntVector2 coordinates = door.coordinates + direction.ToIntVector2();
				//if neighbor exists then...
				if(coordinates.x < size.x  && coordinates.z < size.z && coordinates.x > 0 && coordinates.z > 0)
				{
					Cell neighbor = GetCell(coordinates);
					if(neighbor.room != connectedRegion && rooms.Contains(neighbor.room))
					{
						//if room is not connected and isn't a hallway
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}
		return false;
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
		passage = Instantiate(doorPrefab) as Passage;
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
						//newCell.transform.parent = newRoom.transform;
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
