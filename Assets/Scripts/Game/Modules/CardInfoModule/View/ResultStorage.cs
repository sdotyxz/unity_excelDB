using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;

public class ResultStorage : CommonStorage 
{
	private List<CardInfoUnit> resultlist = new List<CardInfoUnit>();

	protected override void GetSlot (GameObject go)
	{
		CardInfoUnit unit = go.GetComponent<CardInfoUnit>();
		resultlist.Add(unit);
	}

	protected override void ClearStorage ()
	{
		resultlist = new List<CardInfoUnit>();
	}

	public void Fill(List<cfgcard> cfglist)
	{
		HideAllSlot();
		for(int i = 0; i < cfglist.Count; i++)
		{
			resultlist[i].gameObject.SetActive(true);
			resultlist[i].UpdateCardInfo(cfglist[i]);
		}
	}
}
