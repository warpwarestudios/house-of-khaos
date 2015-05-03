using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

	public float wayPointProbability;

	public IntVector2 size;
	public float scale;
	public int numRooms;
	public Cell cellPrefab;
	public MapRoom roomPrefab;
	public Passage passagePrefab;
	public Wall wallPrefab;
	public Wall wallWindowPrefab;
	public Wall wallLampPrefab;
	public Door doorPrefab;
	public Door doorFramePrefab;
	public GameObject player;
	public float lampProbability;
	public float windowProbability;
	public MapRoomSettings[] roomSettings;
	public IntVector2 minRoomSize;
	public IntVector2 maxRoomSize;
	private MapRoom connectedRegion;


	public IntVector2 RandomCoordinates {
		get {
			return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
		}
	}

	public Cell[,] cells;
	public List<MapRoom> rooms = new List<MapRoom>();
	public List<MapRoom> halls = new List<MapRoom>();

	private List<Cell> connectors = new List<Cell>();

	private void Start () {
		this.transform.localScale = new Vector3 (scale, scale, scale);
	}

	public List<Cell> activeCells = new List<Cell> ();

	public void Awake()
	{
	}


	public void Generate () {

		cells = new Cell[size.x, size.z];
		//create active cell list

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

		while (rooms.Count != 1)
		{
			DoNextConnectionStep(connectedRooms,activeCells);

		}

		//remove unconnected halls
		foreach (MapRoom hall in halls) 
		{
			DestroyImmediate(hall.gameObject);
		}

		//randomly add windows
		//step 1: get all edges that open to the outside
		//step 1a: clear out cells in map that do not exist
		Cell removedCell;

		for (int x = 0; x < size.x; x++) {
			for (int z = 0; z < size.z; z++) {
				removedCell = GetCell(new IntVector2(x,z));
				//if the cell does not exist in the map anymore, set it to null
				if(!rooms[0].InRoom(removedCell))
				{
					cells[x,z] = null;
				}
			}
		}

		//step 1b: call function to return all outside walls
		activeCells = rooms[0].returnOutsideWallsList(this);


		//step 2: randomly create windows on that list of edges
		foreach(Cell cell in activeCells)
		{
			for(int i = 0; i < 4; i++)
			{
				MapDirection direction = (MapDirection)i;
				if(cell.GetEdge(direction).GetType() == typeof(Wall))
				{
					IntVector2 coordinates = cell.coordinates + direction.ToIntVector2();
					//if neighbor exists then...
					if(coordinates.x < size.x  && coordinates.z < size.z && coordinates.x > 0 && coordinates.z > 0)
					{
						Cell neighbor = GetCell(coordinates);
						//if neighbor is null
						if(neighbor == null)
						{
							CreateWindowInWall(cell,neighbor, direction);
						}
					}
				}
				
			}
		}


		//remove dead ends
		foreach (Cell cell in cells) 
		{
			if(cell != null)
			{
				RemoveDeadEnd(cell);
			}
		}

		foreach (MapRoom room in connectedRooms) 
		{
			room.InitializeTextures();

			//delete all spawn points and way points from item cells
			room.UpdateSpawnPoints();
		
			//use list of outside walls to remove spawn points and item spawns from edges
			activeCells = room.returnOutsideWallsList(this);

			foreach (Cell cell in activeCells) 
			{
				GameObject spawnPoint = cell.transform.FindChild ("Spawn Point").gameObject;
				GameObject itemSpawn = cell.transform.FindChild ("Item Spawn").gameObject;
				DestroyImmediate (itemSpawn.gameObject);
				DestroyImmediate (spawnPoint.gameObject);
			}
		}
		

		//put player in random room
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");

		GameObject playerSpawn = spawnPoints[Random.Range(0,spawnPoints.Length - 1)];

		Vector3 playerPos = new Vector3(playerSpawn.transform.position.x * scale, 0.5f, playerSpawn.transform.position.z * scale);

		player = PhotonNetwork.Instantiate("Mafioso", playerPos , Quaternion.identity,0);

		//Debug.Log ("Player Spawn Parent: " + playerSpawn.transform.parent.name);
		//Debug.Log ("Player Spawn: X = " + (playerSpawn.transform.position.x * scale) + " Z = " +  (playerSpawn.transform.position.z * scale));
		//Debug.Log ("Player: X = " + player.transform.position.x + " Z = " + player.transform.position.z);
		//Debug.Log ("Player Spawn Offset: X = " + (playerSpawn.transform.position.x - player.transform.position.x) + " Z = " +  (playerSpawn.transform.position.z - player.transform.position.z));
		//player.transform.parent = playerSpawn.transform;
		//player.transform.localPosition = new Vector3(0,0,0);
		//player.transform.parent = null;
		PhotonView pv = player.GetComponent<PhotonView>();
		if (pv.isMine) {
			MouseLook mouselook  = player.GetComponent<MouseLook>();
			mouselook.enabled = true;
			FPSInputController controller  = player.GetComponent<FPSInputController>();
			controller.enabled = true;
			CharacterMotor charactermotor = player.GetComponent<CharacterMotor>(); 
			charactermotor.enabled = true;
			Transform playerCam = player.transform.Find ("Main Camera");
			playerCam.gameObject.active = true;
			GameObject.Find("UI Root").transform.FindChild("Camera").GetComponent<Camera>().enabled = true;
		}
		//destroy all player spawn points
		foreach(GameObject spawn in spawnPoints)
		{
			//spawn.transform.DetachChildren();
			//Destroy(spawn);
		}

		//Waypoint cleanup
		GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
		//destroy all player spawn points
		foreach(GameObject waypoint in wayPoints)
		{
			if(Random.value < wayPointProbability)
			{
				waypoint.GetComponent<Waypoint>().canSpawn = true;
			}
			else
			{
				DestroyImmediate(waypoint);
			}
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
				newHallway.settingsIndex = 0;
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

					DestroyImmediate(next.itemSpawn.gameObject);
					DestroyImmediate(next.playerSpawn.gameObject);

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

	//add random room to connected region
	private void DoFirstConnectionStep(List<MapRoom> connectedRooms)
	{
		//create region
		connectedRegion = Instantiate (roomPrefab) as MapRoom;
		connectedRegion.size = new IntVector2 (0,0);
		connectedRegion.settingsIndex = 0;
		connectedRegion.settings = roomSettings[connectedRegion.settingsIndex];
		connectedRegion.name = "Connected Region";
		connectedRegion.transform.parent = transform;
		connectedRegion.transform.localPosition = new Vector3 (0, 0, 0);
	}

	//search each possible connection for a room connection...keep those, discard the rest
	//once all possible rooms are added, remove rooms from list, loop, choose new room to connect
	//repeat until possible rooms are down to 1
	private void DoNextConnectionStep(List<MapRoom> connectedRooms, List<Cell> activeCells)
	{
		//get last room
		MapRoom room = rooms[rooms.Count - 1];
		bool done = false;
		//get all connecting cells in that room
		GetConnectors (room, activeCells);
		room.MergeInto (connectedRegion);

		//search all connections for halls to rooms, and create doors
		foreach(Cell door in activeCells)
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
						if(neighbor.room != connectedRegion)
						{
							//if it is a hallway add to region
							if(halls.Contains(neighbor.room))
							{
								//get connectors and attach to new room
								Cell newCell = ConnectHallsToRooms(neighbor.room);
								if(newCell != null)
								{
									rooms.Remove(newCell.room);
									CreateDoorInWall(door, neighbor, direction);
									connectedRooms.Add(newCell.room);
									neighbor.room.MergeInto(connectedRegion);
									newCell.room.MergeInto(connectedRegion);

								}
							}
							//if it is a room add to connected rooms
							else
							{

								rooms.Remove(neighbor.room);
								CreateDoorInWall(door, neighbor, direction);
								connectedRooms.Add(neighbor.room);
								neighbor.room.MergeInto(connectedRegion);
							}
						}
					}
				}
			}

		}
		rooms.Remove(room);
		rooms.Add(connectedRegion);
		activeCells.Clear();
	}

	//takes all connectors in the hall and connects them to all rooms
	private Cell ConnectHallsToRooms(MapRoom room)
	{
		List<Cell> active = new List<Cell> ();
		GetConnectors (room, active);
		bool done = false;
		Cell neighbor = null;

		foreach(Cell door in active)
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
						neighbor = GetCell(coordinates);
						//if the neighbor cell has a room, and is not connected
						if(neighbor.room != connectedRegion && rooms.Contains (neighbor.room))
						{
							CreateDoorInWall(door, neighbor, direction);
							return neighbor;
						}
					}
				}
			}
		}

		return null;
	}

	private void CreateDoorInWall(Cell door, Cell neighbor, MapDirection direction)
	{
		Destroy(door.GetEdge(direction).gameObject);
		Destroy(neighbor.GetEdge(direction.GetOpposite()).gameObject);
		CreateDoor(door, neighbor, direction);
		
	}

	private void CreateWindowInWall(Cell window, Cell neighbor, MapDirection direction)
	{
		if (Random.value <= windowProbability) 
		{
			//create window

			Destroy(window.GetEdge(direction).gameObject);

			//Destroy(neighbor.GetEdge(direction.GetOpposite()).gameObject);
			CreateWindow(window, neighbor, direction);
		}
		
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
			new Vector3 (coordinates.x - size.x * 0.5f + 0.5f,  0f, coordinates.z - size.z * 0.5f + 0.5f);

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
		passage.transform.localScale = new Vector3 (1, 1, 1);
		passage = Instantiate(doorFramePrefab) as Passage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
		passage.transform.localScale = new Vector3 (1, 1, 1);
	}

	private void CreatePassageInSameRoom (Cell cell, Cell otherCell, MapDirection direction) {
		Passage passage = Instantiate(passagePrefab) as Passage;
		passage.Initialize(cell, otherCell, direction);
		passage = Instantiate(passagePrefab) as Passage;
		passage.Initialize(otherCell, cell, direction.GetOpposite());
	}

	private void CreateWall (Cell cell, Cell otherCell, MapDirection direction) {
		Wall wall;
		
		//random chance to create wall with lamp
		if (Random.value <= lampProbability) 
		{
			//create lamp
			wall = Instantiate (wallLampPrefab) as Wall;
		} else 
		{
			wall = Instantiate(wallPrefab) as Wall;
		}
		wall.Initialize(cell, otherCell, direction);
		if (otherCell != null) {
			if (Random.value <= lampProbability) 
			{
				//create lamp
				wall = Instantiate (wallLampPrefab) as Wall;
			} else 
			{
				wall = Instantiate(wallPrefab) as Wall;
			}
			wall.Initialize(otherCell, cell, direction.GetOpposite());
		}
	}
	private void CreateWindow (Cell cell, Cell otherCell, MapDirection direction) {
		Wall wall = Instantiate (wallWindowPrefab) as Wall;
		wall.Initialize(cell, otherCell, direction);
	}

	private void CreateRoom () {
		//generate rooms, unless it overlaps. Destroy on overlap.
		MapRoom newRoom = Instantiate (roomPrefab) as MapRoom;
		newRoom.size = new IntVector2 (Random.Range(minRoomSize.z,maxRoomSize.x), Random.Range(minRoomSize.z, maxRoomSize.z));
		bool overlap = false;
		//TODO: add randomization elements
		newRoom.settingsIndex = Random.Range(1, roomSettings.Length -1);
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

	private void RemoveDeadEnd(Cell cell)
	{
		int wallCount = 0;
		MapDirection openDirection = new MapDirection ();
		Cell newCell;
		//check each direction
		for(int i = 0; i < 4; i++)
		{
			MapDirection direction = (MapDirection)i;
			if(cell.GetEdge(direction).GetType() == typeof(Wall))
			{
				wallCount++;
			}
			if(cell.GetEdge(direction).GetType() == typeof(Passage))
			{
				openDirection = direction;
			}
		}

		//if wall count = 3, move through passage
		if (wallCount == 3) 
		{
			newCell = cell.GetEdge(openDirection).otherCell;

			//Create wall between new cell and old cell
			CreateWall(newCell,cell,openDirection.GetOpposite());
			//delete old cell
			DestroyImmediate(cell.gameObject);

			//continue with new cell
			RemoveDeadEnd(newCell);
		}

		//stops when walls != 3
	}

}