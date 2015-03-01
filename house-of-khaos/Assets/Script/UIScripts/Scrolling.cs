using UnityEngine;
using System.Collections;

public class Scrolling : MonoBehaviour {

	public GameObject target;
	public UIScrollBar scroll;
	
	void Update ()
	{
		if(Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			OnScroll (1f);	
		}
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			OnScroll (-1f);	
		}
	
	}
	void OnScroll (float delta)
	{
//		//target.SendMessage("OnScroll", delta, SendMessageOptions.DontRequireReceiver);
		scroll = target.GetComponent<UIScrollBar>();
		scroll.value = scroll.value + delta * 0.1f;
	}
}
