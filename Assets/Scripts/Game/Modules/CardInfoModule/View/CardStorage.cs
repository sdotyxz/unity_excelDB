using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;

public class CardStorage : CommonStorage 
{
	public UIScrollView scrollview;

	private List<MyCardUnit> myCardUnitList = new List<MyCardUnit>();
	public List<MyCardUnit> CardUnitList { get { return myCardUnitList; } }
	private int activeunit
	{
		get
		{
			int num = 0;
			foreach(MyCardUnit cu in myCardUnitList)
			{
				if(cu.gameObject.activeInHierarchy) num ++;
			}
			return num;
		}
	}

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

	public void MoveToIndex(int index)
	{
		int row = Mathf.CeilToInt((float)(index + 1) / (float)storageGrid.maxPerLine);
		int totalrow = Mathf.CeilToInt((float)activeunit / (float)storageGrid.maxPerLine);

		Vector3 target = new Vector3(0, (row - 1) * (storageGrid.cellHeight * 0.5f), 0);

		Vector3 offset = target - scrollview.transform.localPosition;

		scrollview.MoveRelative(offset);

		if(row == 1 || row == 0) 
		{
			scrollview.ResetPosition();
		}
		else if(row == totalrow)
		{
			scrollview.SetDragAmount(1, 1, false);
			scrollview.Drag();
		}
	}
}
