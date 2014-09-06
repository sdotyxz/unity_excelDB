using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CommonStorage : MonoBehaviour 
{
	public GameObject storageTemplate;
	public UIGrid storageGrid;
	private List<GameObject> slotlist = new List<GameObject>();

	public void Build(int size)
	{
		if(storageTemplate != null)
		{
			for(int i = 0; i < size; i++)
			{
				GameObject go = NGUITools.AddChild(storageGrid.gameObject, storageTemplate);
				GetSlot(go);
				slotlist.Add(go);
			}
			StartCoroutine(Repos());
		}
	}

	IEnumerator Repos()
	{
		yield return new WaitForSeconds(0.1f);
		storageGrid.Reposition();
		HideAllSlot();
	}

	public void Clear()
	{
		foreach(GameObject slot in slotlist)
		{
			Destroy(slot);
		}
		slotlist = new List<GameObject>();
		ClearStorage();
	}

	public void HideAllSlot()
	{
		foreach(GameObject slot in slotlist)
		{
			slot.SetActive(false);
		}
	}

	protected virtual void GetSlot(GameObject go)
	{

	}

	protected virtual void ClearStorage()
	{

	}
}
