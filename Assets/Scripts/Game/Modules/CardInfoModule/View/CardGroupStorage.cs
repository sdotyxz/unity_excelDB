using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardGroupStorage : CommonStorage 
{
	private List<CardGroupUnit> unitlist = new List<CardGroupUnit>();

	protected override void GetSlot (GameObject go)
	{
		CardGroupUnit unit = go.GetComponent<CardGroupUnit>();
		unitlist.Add(unit);
	}

	protected override void ClearStorage ()
	{
		unitlist = new List<CardGroupUnit>();
	}

	public void Fill(List<CardGroup> grouplist)
	{
		HideAllSlot();
		for(int i = 0; i < grouplist.Count; i ++)
		{
			unitlist[i].gameObject.SetActive(true);
			unitlist[i].UpdateUnit(grouplist[i]);
		}
	}
}
