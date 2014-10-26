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

	//private List<CardInfo> searchResult = new List<CardInfo>();

	private List<cfgcard> searchResult2 = new List<cfgcard>();

	public List<cfgcard> SearchList { get { return searchResult2; } }
	public int previewindex = 0;

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnSearch).onClick = OnClickbtnSearch;
		Pool.GetComponent<UIEventListener>(btnNextPage).onClick = OnClickbtnNextPage;
		Pool.GetComponent<UIEventListener>(btnLastPage).onClick = OnClickbtnLastPage;
		resultStorage.Build(pagenum);
	}

	public void UpdateSearchField()
	{
		CardInfoManager.instance.ColorList.ForEach(e => {
			popColor.AddItem(e);
		});

		CardInfoManager.instance.LevelList.ForEach(e => {
			popLevel.AddItem(e.ToString());
		});

		CardInfoManager.instance.KindList.ForEach(e => {
			popType.AddItem(e);
		});

		CardInfoManager.instance.FrameList.ForEach(e => {
			if(e != "null") popFrame.AddItem(e);
		});

		CardInfoManager.instance.SerialsList.ForEach(e => {
			popSerials.AddItem(e);
		});
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
		
//		Dictionary<string, object> querydic = new Dictionary<string, object>();
//		querydic.Add("Color", color);
//		querydic.Add("Level", level);
//		querydic.Add("Type", type);
//		querydic.Add("Linkframe", frame);
//		querydic.Add("Serials", serials);
//		
//		List<CardInfo> infolist = CardInfoManager.instance.GetCardListByMultipleSearch(querydic);
//		
//		List<CardInfo> finallist = infolist.FindAll(e => e.CardName.Contains(cardname));
//		finallist = finallist.FindAll(e => e.CardNo.Contains(cardNo));
//
//		searchResult = finallist;

//		currentPage = 1;
//		maxPage = CalMaxPage(searchResult.Count, pagenum);
//		ShowResult(1);

		Dictionary<string, object> querydic2 = new Dictionary<string, object>();
		querydic2.Add("color", color);
		querydic2.Add("level", level);
		querydic2.Add("kind", type);
		querydic2.Add("frame", frame);
		querydic2.Add("serials", serials);

		List<cfgcard> infolist2 = CardInfoManager.instance.GetCardCfgListByMultipleSearch(querydic2);

		List<cfgcard> finallist2 = infolist2.FindAll(e => e.name.Contains(cardname));
		finallist2 = finallist2.FindAll(e => e.no.Contains(cardNo));

		searchResult2 = finallist2;

		currentPage = 1;
		maxPage = CalMaxPage(searchResult2.Count, pagenum);
		ShowResult(currentPage);
	}

	void ShowResult(int page)
	{
		int shownum = WorkOutPageNum(page, searchResult2.Count);
		List<cfgcard> showlist = searchResult2.GetRange((page - 1) * pagenum, shownum);
		resultStorage.Fill(showlist);
		txtPage.text = page.ToString() + "/" + maxPage.ToString();
	}

	int WorkOutPageNum(int page, int totallength)
	{
		int num = pagenum;
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












