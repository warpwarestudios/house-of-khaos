using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {

	public GameObject playerNameHolder;
	public GameObject joinNameHolder;
	public GameObject createNameHolder;

	private GameObject joinNameHolderListed;
	private GameObject RoomObject;

	private PhotonPlayer[] playerList;
	private ArrayList listedPlayers = new ArrayList();
	private bool lobbyScreen = false;
	private bool inPlayerHub = false;
	
	

	// Use this for initialization
	void Start () 
	{
		if (!PhotonNetwork.connected) 
		{
			PhotonNetwork.ConnectUsingSettings("0.01");
			Debug.Log("Succesfully Connected to Photon");
		}
			
		// loading screen exit
		//if (!PhotonNetwork.connected) {}

		// start up
		PhotonNetwork.logLevel = PhotonLogLevel.Full;

		// Hold player name
		string tempName = PlayerPrefs.GetString ("playerName", "Guest" + Random.Range (1, 9999));
		//Load name from PlayerPrefs
		PhotonNetwork.playerName = tempName;


		//playerNameHolder = GameObject.Find ("PlayerNameInput").GetComponent <UIInput>();
		//joinNameHolder = GameObject.Find ("JoinRoomInput").GetComponent <UIInput>();
		//createNameHolder = GameObject.Find ("CreateRoomInput").GetComponent <UIInput>();
		//RoomObject = (GameObject)Resources.Load ("Lobby Prefab/BrowserRoom");
		RoomObject = (GameObject)Resources.Load ("Lobby Prefab/BrowserRoom");

		playerNameHolder.GetComponent <UIInput>().value = PhotonNetwork.playerName;

		listedPlayers = new ArrayList();

	}

	void Update()
	{
		if (lobbyScreen == true) 
		{
			foreach (RoomInfo game in PhotonNetwork.GetRoomList()) 
			{
				GameObject browseRoom = Instantiate (RoomObject) as GameObject;
				browseRoom.transform.parent = GameObject.Find ("List").transform;
				// name
				browseRoom.transform.FindChild ("RoomName").GetComponent <UILabel> ().text = game.name;
				// size
				browseRoom.transform.FindChild ("RoomName").GetComponent <UILabel> ().text = game.playerCount + "/" + game.maxPlayers;
			}
			lobbyScreen = false;
		}

		if(inPlayerHub)
		{
			if(playerList.Length != PhotonNetwork.playerList.Length)
			{
				playerList = PhotonNetwork.playerList;
				PopulateLobby();
			}
		}

	}

	public void LobbyEntered()
	{
		lobbyScreen = true;
	}

	public void LobbyExited()
	{
		if (lobbyScreen = true) 
		{
			lobbyScreen = false;
		}
	}

	// Randomly joins a room out of available rooms, if space is available or room exists
	public void JoinRand()
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	public void NamePlayer()
	{
		PhotonNetwork.playerName = playerNameHolder.GetComponent <UIInput>().value;
		// possible name save
		PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		
		Debug.Log ("Bounce: " + PhotonNetwork.playerName);
	}
	
	public void JoinSpecRoom()
	{
		PhotonNetwork.JoinRoom (joinNameHolder.GetComponent <UIInput>().value);
	}

	public void JoinRoomListed()
	{
		//joinNameHolderListed = GameObject.Find ("RoomName").GetComponent <UIInput>();
		PhotonNetwork.JoinRoom (joinNameHolderListed.GetComponent <UIInput>().value);
	}
	
	public void CreateSpecRoom()
	{
		// using null as TypedLobby parameter will also use the default lobby
		PhotonNetwork.CreateRoom(createNameHolder.GetComponent <UIInput>().value, new RoomOptions() { maxPlayers = 6 }, TypedLobby.Default);
	}

	// call back for failed to join
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		// should relay why the join failed, either too many players in room or no rooms available
		// *TESTING*
		Debug.Log(PhotonNetworkingMessage.OnPhotonRandomJoinFailed.ToString());
		Debug.Log("Attempting to Create Room");
		string roomName = ("Room" + Random.Range (1, 9999));
		PhotonNetwork.CreateRoom(createNameHolder.GetComponent <UIInput>().value, new RoomOptions() { maxPlayers = 6 }, TypedLobby.Default);;
	}

	void OnJoinedRoom()
	{
		// precondition: player has been properly switched to game lobby screen

		if (PhotonNetwork.playerName.ToLower() == "debug") 
		{
			PhotonNetwork.LoadLevel ("GameScreen");
		}

		inPlayerHub = true;
		PopulateLobby();

	}

	private void PopulateLobby()
	{
		GameObject lobbyPlayer;
		playerList = PhotonNetwork.playerList;

		// clear out list
		int i=1;
		foreach(PhotonPlayer player in listedPlayers)
		{
			lobbyPlayer = GameObject.Find("LobbyPlayer "+i);
			lobbyPlayer.transform.FindChild("PlayerName").GetComponent<UILabel>().text = "Open";
			i++;
		}

		// copy over player list
		foreach(PhotonPlayer player in playerList)
		{
			listedPlayers.Add(player);
		}

		//  place master client first
		foreach(PhotonPlayer player in listedPlayers)
		{
			if(player.isMasterClient)
			{
				lobbyPlayer = GameObject.Find("LobbyPlayer 1");
				lobbyPlayer.transform.FindChild("PlayerName").GetComponent<UILabel>().text = player.name;
				listedPlayers.Remove(player);
				break;
			}
		}

		// place remaining players
		i=2;
		foreach(PhotonPlayer player in listedPlayers)
		{
			lobbyPlayer = GameObject.Find("LobbyPlayer "+i);
			lobbyPlayer.transform.FindChild("PlayerName").GetComponent<UILabel>().text = player.name;
			i++;
		}
	}


	// start the game
	private void GameStart ()
	{
		if (PhotonNetwork.isMasterClient) 
		{
			PhotonNetwork.LoadLevel ("GameScreen");
		} 
		else 
		{
			//TODO: set player to ready state
		}

	}

	public void DisconnectFromGame()
	{
		PhotonNetwork.LeaveRoom ();
	}

	void OnPhotonPlayerConnected()
	{
		if(inPlayerHub)
		{
			PopulateLobby();
		}
	}

	void onLeftRoom()
	{
		inPlayerHub = false;
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
