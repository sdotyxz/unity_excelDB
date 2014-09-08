using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;

public class CardSearchField : MonoBehaviour 
{
	public UIPopupList popColor;
	public UIPopupList popLevel;
	public UIPopupList popType;
	public UIPopupList popFrame;
	public UIPopupList popSerials;
	public UIInput inputName;
	public UIInput inputCardNo;
	public UIButton btnSearch;
	public UIButton btnNextPage;
	public UIButton btnLastPage;
	public UILabel txtPage;

	public ResultStorage resultStorage;
	private int pagenum = 7;
	private int currentPage = 1;
	private int maxPage = 1;

	private List<CardInfo> searchResult = new List<CardInfo>();

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnSearch).onClick = OnClickbtnSearch;
		Pool.GetComponent<UIEventListener>(btnNextPage).onClick = OnClickbtnNextPage;
		Pool.GetComponent<UIEventListener>(btnLastPage).onClick = OnClickbtnLastPage;
		resultStorage.Build(pagenum);
	}

	void OnClickbtnNextPage (GameObject go)
	{
		if(currentPage < maxPage)
		{
			currentPage ++;
			ShowResult(currentPage);
		}
	}

	void OnClickbtnLastPage (GameObject go)
	{
		if(currentPage > 1)
		{
			currentPage --;
			ShowResult(currentPage);
		}
	}

	void OnClickbtnSearch (GameObject go)
	{
		string color = popColor.value;
		int level = -1;
		if(popLevel.value != "*")
		{
			level = int.Parse(popLevel.value);
		}
		string type = popType.value;
		string frame = popFrame.value;
		string serials = popSerials.value;
		string cardname = inputName.value;
		string cardNo = inputCardNo.value;
		
		Dictionary<string, object> querydic = new Dictionary<string, object>();
		querydic.Add("Color", color);
		querydic.Add("Level", level);
		querydic.Add("Type", type);
		querydic.Add("Linkframe", frame);
		querydic.Add("Serials", serials);
		
		List<CardInfo> infolist = CardInfoManager.instance.GetCardListByMultipleSearch(querydic);
		
		List<CardInfo> finallist = infolist.FindAll(e => e.CardName.Contains(cardname));
		finallist = finallist.FindAll(e => e.CardNo.Contains(cardNo));

		searchResult = finallist;

		currentPage = 1;
		maxPage = CalMaxPage(searchResult.Count, pagenum);
		ShowResult(1);
	}

	void ShowResult(int page)
	{
		int shownum = WorkOutPageNum(page);
		List<CardInfo> showlist = searchResult.GetRange((page - 1) * pagenum, shownum);
		resultStorage.Fill(showlist);
		txtPage.text = page.ToString() + "/" + maxPage.ToString();
	}

	int WorkOutPageNum(int page)
	{
		int num = pagenum;
		int totallength = searchResult.Count;
		if(page * pagenum > totallength)
		{
			num = totallength % pagenum;
		}
		return num;
	}

	int CalMaxPage(int total, int num)
	{
		return Mathf.CeilToInt((float)total / (float)num);
	}
}












