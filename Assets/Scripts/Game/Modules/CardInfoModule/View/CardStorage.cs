using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;

public class CardStorage : CommonStorage 
{
	private List<MyCardUnit> myCardUnitList = new List<MyCardUnit>();

	protected override void GetSlot (GameObject go)
	{
		MyCardUnit card = go.GetComponent<MyCardUnit>();
		if(card != null) myCardUnitList.Add(card);
	}

	protected override void ClearStorage ()
	{
		myCardUnitList = new List<MyCardUnit>();
	}

	public void Fill(List<MyCard> mycardlist)
	{
		HideAllSlot();
		for(int i = 0; i < mycardlist.Count; i ++)
		{
			myCardUnitList[i].gameObject.SetActive(true);
			myCardUnitList[i].UpdateMyCardUnit(mycardlist[i]);
		}
	}
}
