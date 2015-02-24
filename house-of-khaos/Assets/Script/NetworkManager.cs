using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public bool createPlayer;


	// Use this for initialization
	void Start()
	{
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
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

		// sync the room for the player
		// *TESTING*
		PhotonNetwork.automaticallySyncScene = true;

		// debugging purpose: determination of master client for which all updates pass through
		// *TESTING*
		if (PhotonNetwork.isMasterClient) {
			Debug.Log("Current client is master client");
				}

		Debug.Log("Player ID:" + PhotonNetwork.player.ID);
		
	}

	// call back for failed to join
	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		// should relay why the join failed, either too many players in room or no rooms avaliable
		// *TESTING*
		Debug.Log(PhotonNetworkingMessage.OnPhotonRandomJoinFailed.ToString());
		PhotonNetwork.CreateRoom(null);
	}

}
