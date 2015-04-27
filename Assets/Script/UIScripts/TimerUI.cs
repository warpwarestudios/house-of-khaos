using UnityEngine;
using System.Collections;


[RequireComponent (typeof(PhotonView))]
public class TimerUI : Photon.MonoBehaviour
{	
	private float theStartTime = 900;
	private float theTimer; 
	private int seconds;
	private int minutes;
	public static readonly string updateTimeRPC = "UpdateTime";

	void Start() 
	{
		theTimer = theStartTime;
	}
	
	void Update() 
	{
		this.photonView.RPC("UpdateTime", PhotonTargets.All);
	}
	
	[RPC]
	public void UpdateTime(PhotonMessageInfo mi)
	{
		theTimer -= Time.deltaTime;
		
		if (theTimer < 10) 
		{
			Debug.Log("TEN SECONDS LEFT !");
		}
		
		if (theTimer <= 0) 
		{
			Debug.Log("OUT OF TIME");
			theTimer = 0;
		}
		
		minutes = (int) (theTimer / 60.0 );
		seconds = Mathf.RoundToInt((int) (theTimer % 60.0 ));
		
		this.GetComponent<UILabel>().text = string.Format( "{0:00}:{1:00}", minutes.ToString("00"), seconds.ToString("00")); 	
	}
	
			
}

