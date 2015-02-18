using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public bool createPlayer;
	private UIInput playerNameHolder;
	private UIInput joinNameHolder;
	private UIInput createNameHolder;

	// Use this for initialization
	void Start()
	{
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		playerNameHolder = GameObject.Find ("PlayerNameInput").GetComponent <UIInput>();
		joinNameHolder = GameObject.Find ("JoinRoomInput").GetComponent <UIInput>();
		createNameHolder = GameObject.Find ("CreateRoomInput").GetComponent <UIInput>();
	}
	
	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	// call back for succesful lobby join
	void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	// call back on succesful room join
	void OnJoinedRoom()
	{
		if (createPlayer) 
		{
			GameObject player = PhotonNetwork.Instantiate("Player", this.transform.position, Quaternion.identity, 0);
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
			}
		}

	}

	// call back for failed to join
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}

	public void JoinRand()
	{
		Application.LoadLevel ("GameScreen");
		PhotonNetwork.ConnectUsingSettings("0.01");
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
		PhotonNetwork.JoinRoom(joinNameHolder.value);
	}

	public void CreateSpecRoom()
	{
		// using null as TypedLobby parameter will also use the default lobby
		PhotonNetwork.CreateRoom(createNameHolder.value, new RoomOptions() 
		                         { maxPlayers = 6 }, TypedLobby.Default);
	}

	/*
	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			ShowConnectingGUI();
			return;   //Wait for a connection
		}
		
		
		if (PhotonNetwork.room != null)
			return; //Only when we're not in a Room
		
		
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		
		GUILayout.Label("Main Menu");
		
		//Player name
		GUILayout.BeginHorizontal();
		GUILayout.Label("Player name:", GUILayout.Width(150));
		PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
		if (GUI.changed)//Save name
			PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		GUILayout.EndHorizontal();
		
		GUILayout.Space(15);
		
		
		//Join room by title
		GUILayout.BeginHorizontal();
		GUILayout.Label("JOIN ROOM:", GUILayout.Width(150));
		roomName = GUILayout.TextField(roomName);
		if (GUILayout.Button("GO"))
		{
			PhotonNetwork.JoinRoom(roomName);
		}
		GUILayout.EndHorizontal();
		
		//Create a room (fails if exist!)
		GUILayout.BeginHorizontal();
		GUILayout.Label("CREATE ROOM:", GUILayout.Width(150));
		roomName = GUILayout.TextField(roomName);
		if (GUILayout.Button("GO"))
		{
			// using null as TypedLobby parameter will also use the default lobby
			PhotonNetwork.CreateRoom(roomName, new RoomOptions() { maxPlayers = 10 }, TypedLobby.Default);
		}
		GUILayout.EndHorizontal();
		
		//Join random room
		GUILayout.BeginHorizontal();
		GUILayout.Label("JOIN RANDOM ROOM:", GUILayout.Width(150));
		if (PhotonNetwork.GetRoomList().Length == 0)
		{
			GUILayout.Label("..no games available...");
		}
		else
		{
			if (GUILayout.Button("GO"))
			{
				PhotonNetwork.JoinRandomRoom();
			}
		}
		GUILayout.EndHorizontal();
		
		GUILayout.Space(30);
		GUILayout.Label("ROOM LISTING:");
		if (PhotonNetwork.GetRoomList().Length == 0)
		{
			GUILayout.Label("..no games available..");
		}
		else
		{
			//Room listing: simply call GetRoomList: no need to fetch/poll whatever!
			scrollPos = GUILayout.BeginScrollView(scrollPos);
			foreach (RoomInfo game in PhotonNetwork.GetRoomList())
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
				if (GUILayout.Button("JOIN"))
				{
					PhotonNetwork.JoinRoom(game.name);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}
		
		GUILayout.EndArea();
	}
	
	
	void ShowConnectingGUI()
	{
		GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
		
		GUILayout.Label("Connecting to Photon server.");
		GUILayout.Label("Hint: This demo uses a settings file and logs the server address to the console.");
		
		GUILayout.EndArea();
	}*/
}
