using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {

	public GameObject room;
	public bool generate;
	public int hallNum = 5;
	public int roomNum = 10;
	public GameObject[] rooms;

	// Use this for initialization
	void Start () {
		generate = true;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (generate) 
		{
			rooms = GameObject.FindGameObjectsWithTag("Room");
			//destroy map
			foreach (GameObject room in rooms)
			{
				Destroy(room);
			}
			rooms = null;
			for(int i = 1; i <= roomNum; i++)
			{
				Vector3 pos = new Vector3(0,0,0);
				GameObject newRoom = (GameObject)PhotonNetwork.Instantiate("Room",pos, Quaternion.identity,0);
				newRoom.transform.parent = this.transform;
				newRoom.transform.position = pos;
				Vector3 scale = new Vector3(Random.Range(5,20),0,Random.Range(5,20));
				newRoom.transform.localScale = scale;
			}

			//get all rooms
			rooms = GameObject.FindGameObjectsWithTag("Room");
			//loop through each room and put it next to another one.
			int x = 0;
			int z = 0;
			for(int i = 1; i < rooms.Length; i++)
			{
				//calculate position:
				//1. find edge of last placed by finding the center + extents
				//2. add on the extents of the current box
				x = (int)(rooms[i - 1].GetComponentInChildren<BoxCollider>().bounds.center.x 
				              + rooms[i - 1].GetComponentInChildren<BoxCollider>().bounds.extents.x 
				              + rooms[i].GetComponentInChildren<BoxCollider>().bounds.extents.x);
				z = (int)(rooms[i].transform.position.z);
				rooms[i].transform.position = new Vector3(x,1,z);
			}
			generate = false;
		}
	}

}
