using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class PhotonChat : Photon.MonoBehaviour 
{
	public List<string> messages = new List<string>();
	private string inputLine = "";
	
	public static readonly string ChatRPC = "Chat";
	
	public void Start()
	{}
	
	[RPC]
	public void Chat(string newLine, PhotonMessageInfo mi)
	{
		string senderName = "anonymous";
		
		if (mi != null && mi.sender != null)
		{
			if (!string.IsNullOrEmpty(mi.sender.name))
			{
				senderName = mi.sender.name;
			}
			else
			{
				senderName = "player " + mi.sender.ID;
			}
		}
		
		this.messages.Add(senderName +": " + newLine);
	}
}
