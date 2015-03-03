using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {

	private UIInput playerNameHolder;
	private UIInput joinNameHolder;
	private UIInput joinNameHolderListed;
	private UIInput createNameHolder;

	private GameObject RoomObject;

	// Use this for initialization
	void Start () 
	{
		if (!PhotonNetwork.connected) 
		{
			PhotonNetwork.ConnectUsingSettings("0.01");
		}
			
		// loading screen exit
		//if (!PhotonNetwork.connected) {}

		// start up
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		playerNameHolder = GameObject.Find ("PlayerNameInput").GetComponent <UIInput>();
		joinNameHolder = GameObject.Find ("JoinRoomInput").GetComponent <UIInput>();
		createNameHolder = GameObject.Find ("CreateRoomInput").GetComponent <UIInput>();
		RoomObject = (GameObject)Resources.Load ("Lobby Prefab/BrowserRoom");

		//Load name from PlayerPrefs
        PhotonNetwork.playerName = PlayerPrefs.GetString("playerName", "Guest" + Random.Range(1, 9999));
	}

	void OnJoinedLobby()
	{
		foreach (RoomInfo game in PhotonNetwork.GetRoomList())
        {
			GameObject browseRoom = Instantiate (RoomObject) as GameObject;
			browseRoom.transform.parent = GameObject.Find ("List").transform;
			// name
			browseRoom.transform.FindChild("RoomName").GetComponent <UILabel>().text = game.name;
			// size
			browseRoom.transform.FindChild("RoomName").GetComponent <UILabel>().text = game.playerCount +"/"+ game.maxPlayers;
        }
	}

	// Randomly joins a room out of available rooms, if space is available or room exists
	public void JoinRand()
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	public void NamePlayer()
	{
		PhotonNetwork.playerName = playerNameHolder.value;
		// possible name save
		PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		
		Debug.Log ("Bounce: " + PhotonNetwork.playerName);
	}
	
	public void JoinSpecRoom()
	{
		PhotonNetwork.JoinRoom (joinNameHolder.value);
	}

	public void JoinRoomListed()
	{
		joinNameHolderListed = GameObject.Find ("RoomName").GetComponent <UIInput>();
		PhotonNetwork.JoinRoom (joinNameHolderListed.value);
	}
	
	public void CreateSpecRoom()
	{
		// using null as TypedLobby parameter will also use the default lobby
		PhotonNetwork.CreateRoom(createNameHolder.value, new RoomOptions() { maxPlayers = 6 }, TypedLobby.Default);
	}

	// call back for failed to join
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		// should relay why the join failed, either too many players in room or no rooms available
		// *TESTING*
		Debug.Log(PhotonNetworkingMessage.OnPhotonRandomJoinFailed.ToString());
		string roomName = ("Room" + Random.Range (1, 9999));
		PhotonNetwork.CreateRoom(createNameHolder.value, new RoomOptions() { maxPlayers = 6 }, TypedLobby.Default);;
	}

	void OnJoinedRoom()
	{
		Application.LoadLevel ("GameScreen");
	}
	
	void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }

	/*IEnumerator OnLeftRoom()
    {
        //Easy way to reset the level: Otherwise we'd manually reset the camera

        //Wait untill Photon is properly disconnected (empty room, and connected back to main server)
        while(PhotonNetwork.room!=null || PhotonNetwork.connected==false)
            yield return 0;

        Application.LoadLevel(Application.loadedLevel);

    }*/

}
