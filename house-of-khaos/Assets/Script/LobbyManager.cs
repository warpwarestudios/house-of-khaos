using UnityEngine;
using System.Collections;

public class LobbyManager : MonoBehaviour {

	private UIInput playerNameHolder;
	private UIInput joinNameHolder;
	private UIInput createNameHolder;

	// Use this for initialization
	void Start () 
	{
		// loading screen exit
		//if (PhotonNetworkingMessage.OnConnectedToPhoton) {

		//}

		// start up
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		playerNameHolder = GameObject.Find ("PlayerNameInput").GetComponent <UIInput>();
		joinNameHolder = GameObject.Find ("JoinRoomInput").GetComponent <UIInput>();
		createNameHolder = GameObject.Find ("CreateRoomInput").GetComponent <UIInput>();
	}


	// Randomly joins a room out of avaliable rooms, if space is availible or room exists
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

}
