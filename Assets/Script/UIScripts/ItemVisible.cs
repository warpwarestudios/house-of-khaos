using UnityEngine;
using System.Collections;

public class ItemVisible : MonoBehaviour {

	public Renderer rend;
	
	
	public void Show() {
		rend.enabled = true;
	}
	public void Hide() {
		rend.enabled = false;
	}
	
	public void HideChildren()
	{
		if (transform.childCount > 0)
		{
			Renderer[] lChildRenderers = gameObject.GetComponentsInChildren<Renderer>();
			foreach ( Renderer lRenderer in lChildRenderers)
			{
				lRenderer.enabled = false;
			}
		}
	}
	public void ShowChildren()
	{
		if (transform.childCount > 0)
		{
			Renderer[] lChildRenderers = gameObject.GetComponentsInChildren<Renderer>();
			foreach ( Renderer lRenderer in lChildRenderers)
			{
				lRenderer.enabled = true;
			}
		}
	}


}
