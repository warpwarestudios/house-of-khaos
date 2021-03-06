﻿using UnityEngine;
using System.Collections;

public class LobbyManager : Photon.MonoBehaviour {

	public GameObject playerNameHolder;
	public GameObject joinNameHolder;
	public GameObject createNameHolder;
	public GameObject ErrorMessage;

	private MessageUI errorMessenger;
	private GameObject joinNameHolderListed;
	private GameObject RoomObject;

	private PhotonPlayer[] playerList;
	private ArrayList listedPlayers = new ArrayList();
	private bool lobbyScreen = false;
	private bool inPlayerHub = false;
	
	

	// Use this for initialization
	void Start () 
	{

		errorMessenger = ErrorMessage.GetComponent<MessageUI>();

		if (!PhotonNetwork.connected) 
		{
			PhotonNetwork.ConnectUsingSettings("0.01");
			Debug.Log("Succesfully Connected to Photon");
		}

		// start up
		//PhotonNetwork.logLevel = PhotonLogLevel.Full;

		// Hold player name
		string tempName = PlayerPrefs.GetString ("playerName", "Guest" + Random.Range (1, 9999));
		//Load name from PlayerPrefs
		PhotonNetwork.playerName = tempName;
		
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
				GameObject.Find ("List").gameObject.GetComponent<UIGrid>().enabled = true;
				// name
				browseRoom.transform.FindChild ("RoomName").GetComponent <UILabel> ().text = game.name;
				// size
				browseRoom.transform.FindChild ("RoomName").GetComponent <UILabel> ().text = game.playerCount + "/" + game.maxPlayers;
			}
			lobbyScreen = false;
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
	 public void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		// should relay why the join failed, either too many players in room or no rooms available
		// *TESTING*
		Debug.Log(PhotonNetworkingMessage.OnPhotonRandomJoinFailed.ToString());
		Debug.Log("Attempting to Create Room");
		string roomName = ("Room" + Random.Range (1, 9999));
		PhotonNetwork.CreateRoom(createNameHolder.GetComponent <UIInput>().value, new RoomOptions() { maxPlayers = 6 }, TypedLobby.Default);;
	}

	public void OnPhotonJoinRoomFailed() 
	{
		Debug.Log("Could not find room");
		errorMessenger.Message("Failed to Join", "Could not connect to specified room.");
	}

	public void OnPhotonCreateRoomFailed()
	{
		Debug.Log("Could not create room");
		errorMessenger.Message("Failed to create room", "Room name already in use");
	}

	public void OnJoinedRoom()
	{
		inPlayerHub = true;
		PopulateLobby();
	}

	private void PopulateLobby()
	{
		
		GameObject lobbyPlayer;
		playerList = PhotonNetwork.playerList;

		// clear out list
		listedPlayers.Clear();

		//reset UI to "Open"
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
			if(lobbyPlayer.transform.FindChild("PlayerName").GetComponent<UILabel>().text != player.name)
			{
				lobbyPlayer.transform.FindChild("PlayerName").GetComponent<UILabel>().text = player.name;
			}
			i++;
		}

		playerList = null;
	}


	// start the game
	private void GameStart ()
	{
		if (PhotonNetwork.isMasterClient) 
		{
			photonView.RPC ("PhotonChangeScenes", PhotonTargets.All);
		} 
		else 
		{
			photonView.RPC ("ReadyUp", PhotonTargets.All);
		}

	}

	// call to shift players to game screen
	[RPC]
	private void PhotonChangeScenes()
	{
		PhotonNetwork.LoadLevel ("GameScreen");
	}

	// call to apply ready up check
	[RPC]
	private void ReadyUp()
	{
		GameObject lobbyPlayer;

		for(int i=2; i<=6; i++)
		{
			lobbyPlayer = GameObject.Find("LobbyPlayer "+i);
			if(lobbyPlayer.transform.FindChild("PlayerName").GetComponent<UILabel>().text == PhotonNetwork.playerName)
			{
				bool status = lobbyPlayer.transform.FindChild("ReadyCheck").gameObject.GetActive();
				lobbyPlayer.transform.FindChild("ReadyCheck").gameObject.SetActive(!status);
			}
		}
	}

	// called if player leaves photon room, governed by button
	void DisconnectFromGame()
	{
		PhotonNetwork.LeaveRoom ();
	}

	// called if player enter photon room in hub
	void OnPhotonPlayerConnected ()
	{
		if(inPlayerHub)
		{
			playerList = PhotonNetwork.playerList;
			PopulateLobby();
		}
	}

	// called if player leaves photon room in hub
	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		if(inPlayerHub)
		{
			playerList = PhotonNetwork.playerList;
			Debug.Log("Player: " + otherPlayer.name +" ID:"+otherPlayer.ID+" left");
			PopulateLobby();
		}

		GameObject lobbyPlayer;
		// remove player from list
		for(int i=2; i<=6; i++)
		{
			lobbyPlayer = GameObject.Find("LobbyPlayer "+i);
			if(lobbyPlayer.transform.FindChild("PlayerName").GetComponent<UILabel>().text == otherPlayer.name)
			{
				lobbyPlayer.transform.FindChild("PlayerName").GetComponent<UILabel>().text = "Open";
			}
		}
	}

	// called if player leaves photon room
	void onLeftRoom()
	{
		inPlayerHub = false;
	}

	// called if fails to connect in the first place
	void OnFailedToConnectToPhoton()
	{
		PhotonNetwork.offlineMode = true;
		errorMessenger.Message("Connection Error", "Could not connect to photon.\n Playing in offline mode.");
	}

	// called if connection is interupted
	void OnConnectionFail ()
	{
		errorMessenger.Message("Connection Error", "Could not connect to photon.\n Playing in offline mode.");
		PhotonNetwork.offlineMode = true;
	}

	// called if disconnecting from photon
	void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("OnDisconnectedFromPhoton");
    }

}
