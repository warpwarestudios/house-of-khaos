using UnityEngine;

/// <summary>
/// Very simple example of how to use a TextList with a UIInput for chat.
/// </summary>

[RequireComponent(typeof(UIInput))]
[AddComponentMenu("NGUI/Examples/Chat Input")]
public class ChatInput : MonoBehaviour
{
	public UITextList textList;
	public GameObject chatArea; // to notifty photon

	UIInput mInput;

	/// <summary>
	/// Add some dummy text to the text list.
	/// </summary>

	void Start ()
	{
		mInput = GetComponent<UIInput>();
		mInput.label.maxLineCount = 1;
	}

	/// <summary>
	/// Submit notification is sent by UIInput when 'enter' is pressed or iOS/Android keyboard finalizes input.
	/// </summary>

	public void OnSubmit ()
	{
		if (textList != null)
		{
			// It's a good idea to strip out all symbols as we don't want user input to alter colors, add new lines, etc
			string text = NGUIText.StripSymbols(mInput.value);

			if (!string.IsNullOrEmpty(text))
			{
				//textList.Add(text);
				NotifyPhoton(text);
				mInput.value = "";
				mInput.isSelected = false;

			}
		}
	}

	public void NotifyPhoton(string text)
	{
		chatArea.GetComponent<PhotonChat> ().message = text;
		chatArea.GetPhotonView ().RPC ("Chat", PhotonTargets.All, text);
	}

}
