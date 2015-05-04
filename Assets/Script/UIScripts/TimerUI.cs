using UnityEngine;
using System.Collections;


[RequireComponent (typeof(PhotonView))]
public class TimerUI : Photon.MonoBehaviour
{	
	private float theStartTime = 900;
	private float theTimer; 
	private int seconds;
	private int minutes;
	private string time;
	public static readonly string updateTimeRPC = "UpdateTime";

	void Start() 
	{
		theTimer = theStartTime;
	}
	
	void Update() 
	{
		if (PhotonNetwork.isMasterClient) 
		{
			theTimer -= Time.deltaTime;

			minutes = (int)(theTimer / 60.0);
			seconds = Mathf.RoundToInt ((int)(theTimer % 60.0));

			time = string.Format ("{0:00}:{1:00}", minutes.ToString ("00"), seconds.ToString ("00"));

			this.photonView.RPC ("UpdateTime", PhotonTargets.All);
<<<<<<< HEAD
			
			if (theTimer <= 0)
			{
				theTimer = 0;
				StartCoroutine(EndGameLose());
			}
=======
>>>>>>> origin/Development-Branch
		}
	}
	
	[RPC]
	public void UpdateTime(PhotonMessageInfo mi)
	{
		this.GetComponent<UILabel>().text = time; 	
	}
	
	public IEnumerator EndGameLose()
	{
		yield return new WaitForSeconds (5f);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Application.LoadLevel ("GameOverScene");
	}
	
			
}

