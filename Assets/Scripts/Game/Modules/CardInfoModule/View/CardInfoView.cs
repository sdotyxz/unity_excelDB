using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;

public class CardInfoView : MonoBehaviour
{
	public CardDesc carddesc;
	public CardStorage cardStorage;
	public CardGroupStorage groupStorage;

	public UIPopupList popColor;
	public UIPopupList popLevel;
	public UIPopupList popKind;
	public UIPopupList popFrame;
	public UIPopupList popSerials;
	public UIInput inputName;
	public UIInput inputNo;

	public UIButton btnAddCard;
	public UIButton btnSaveGroup;
	public UIButton btnDeleteGroup;
	public UIInput inputGroup;
	private List<MyCard> tempcardlist = new List<MyCard>();


	public UIGrid groupgrid;
	public UIGrid cardgrid;
	public UIGrid grid;
	public GameObject template;
	private List<CardInfoUnit> infounitlist = new List<CardInfoUnit>();
	
	private List<CardGroupUnit> mygrouplist = new List<CardGroupUnit>();

	public GameObject iconTemplate;

	public GameObject groupTemplate;

	private GameDataManager gameDataManager;

	private CardInfo currentCardInfo;

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnAddCard).onClick = OnClickbtnAddCard;
		Pool.GetComponent<UIEventListener>(btnDeleteGroup).onClick = OnClickbtnDeleteGroup;
		Pool.GetComponent<UIEventListener>(btnSaveGroup).onClick = OnClickbtnSaveGroup;
		gameDataManager = GameObject.Find("GameDataManager").GetComponent<GameDataManager>();
		UpdateGroupList();
		cardStorage.Build(54);
	}

	void OnClickbtnAddCard (GameObject go)
	{
		if(carddesc.cardinfo != null)
		{
			int index = tempcardlist.FindIndex(e => e.info.CardNo == carddesc.cardinfo.CardNo);
			if(index != -1)
			{
				tempcardlist[index].num++;
			}
			else
			{
				MyCard card = new MyCard(carddesc.cardinfo, 1);
				tempcardlist.Add(card);
			}
		}
		cardStorage.Fill(tempcardlist);
	}

	private void OnClickbtnSaveGroup(GameObject go)
	{
		if(inputGroup.value != "")
		{
			CardGroup group = new CardGroup();
			group.groupname = inputGroup.value;
			group.cardlist = tempcardlist;
			int index = gameDataManager.gameData.CardGroupList.FindIndex(e => e.groupname == group.groupname);
			if(index != -1)
			{
				gameDataManager.gameData.CardGroupList.RemoveAt(index);
			}
			gameDataManager.gameData.CardGroupList.Add(group);
			gameDataManager.Save();
			UpdateGroupList();
		}
	}

	void UpdateGroupList ()
	{
		groupStorage.Clear();
		Pool.GetComponent<CommonStorage>(groupStorage).OnBuildFinish = OnCardGroupBuild;
		groupStorage.Build(gameDataManager.gameData.CardGroupList.Count);
	}

	public void ShowGroupCards(CardGroup group)
	{
		inputGroup.value = group.groupname;
		cardStorage.Fill(group.cardlist);
		tempcardlist = group.cardlist;
	}

	void OnCardGroupBuild(GameObject go)
	{
		groupStorage.Fill(gameDataManager.gameData.CardGroupList);
	}

	void OnClickbtnDeleteGroup (GameObject go)
	{
		if(inputGroup.value != "")
		{
			string groupname = inputGroup.value;
			int index = gameDataManager.gameData.CardGroupList.FindIndex(e => e.groupname == groupname);
			if(index != -1)
			{
				gameDataManager.gameData.CardGroupList.RemoveAt(index);
			}
			gameDataManager.Save();
		}
		UpdateGroupList();
	}

	public void ShowCard(CardInfo info)
	{
		carddesc.UpdateCardDes(info);
		currentCardInfo = info;
	}


	public void SearchCard()
	{
		string serachword = inputName.value.Replace("\t", "");
		CardInfo info = CardInfoManager.instance.GetCardInfoByName(serachword);
		if(info != null)
		{
			ShowCard(info);
		}
	}

	public void SearchCardWithColor()
	{
		List<CardInfo> infolist = CardInfoManager.instance.GetCardListByColor(popColor.value);
		if(infolist != null && template != null)
		{
			foreach(CardInfo info in infolist)
			{
				Debug.Log ("------ ------" + info.CardName);
				GameObject go =  NGUITools.AddChild(grid.gameObject,  template);
				CardInfoUnit ciu = go.GetComponent<CardInfoUnit>();
				infounitlist.Add(ciu);
				ciu.UpdateCardInfo(info);
			}
			grid.Reposition();
		}
	}

	public void SearchCardWithCandL()
	{
		string color = popColor.value;
		int level = -1;
		if(popLevel.value != "*")
		{
			level = int.Parse(popLevel.value);
		}
		string type = popKind.value;
		string frame = popFrame.value;
		string serials = popSerials.value;
		string cardname = inputName.value;
		string cardNo = inputNo.value;

		Dictionary<string, object> querydic = new Dictionary<string, object>();
		querydic.Add("Color", color);
		querydic.Add("Level", level);
		querydic.Add("Type", type);
		querydic.Add("Linkframe", frame);
		querydic.Add("Serials", serials);

		List<CardInfo> infolist = CardInfoManager.instance.GetCardListByMultipleSearch(querydic);

		List<CardInfo> finallist = infolist.FindAll(e => e.CardName.Contains(cardname));
		finallist = finallist.FindAll(e => e.CardNo.Contains(cardNo));
		if(finallist != null && template != null)
		{
			foreach(CardInfo info in finallist)
			{
				Debug.Log ("------ ------" + info.CardName);
				GameObject go =  NGUITools.AddChild(grid.gameObject,  template);
				CardInfoUnit ciu = go.GetComponent<CardInfoUnit>();
				infounitlist.Add(ciu);
				ciu.UpdateCardInfo(info);
			}
			grid.Reposition();
		}
	}

	public void RemoveMyCard(CardInfo info)
	{
		int index = tempcardlist.FindIndex(e => e.info.CardNo == info.CardNo);
		if(index != -1)
		{
			tempcardlist[index].num --;
			if(tempcardlist[index].num == 0)
			{
				tempcardlist.RemoveAt(index);
			}
		}
		cardStorage.Fill(tempcardlist);
	}
}
