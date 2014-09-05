using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;

public class CardInfoView : MonoBehaviour
{
	public CardDesc carddesc;
	public UILabel txtCardEffect;
	public UILabel txtCardName;
	public UILabel txtCardDes;
	public UITexture texCardImage;
	private int curIndex = 0;

	public UIPopupList popColor;
	public UIPopupList popLevel;
	public UIPopupList popKind;
	public UIPopupList popFrame;
	public UIPopupList popSerials;
	public UIInput inputName;
	public UIInput inputNo;
	
	public UIButton btnSaveGroup;
	public UIButton btnDeleteGroup;
	public UIInput inputGroup;
	private List<CardInfo> tempinfolist = new List<CardInfo>();

	public UIGrid groupgrid;
	public UIGrid cardgrid;
	public UIGrid grid;
	public GameObject template;
	private List<CardInfoUnit> infounitlist = new List<CardInfoUnit>();

	private List<MyCardUnit> mycardlist = new List<MyCardUnit>();
	private List<MyCardListUnit> mygrouplist = new List<MyCardListUnit>();

	public GameObject iconTemplate;

	public GameObject groupTemplate;

	private GameDataManager gameDataManager;

	private CardInfo currentCardInfo;

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnDeleteGroup).onClick = OnClickbtnDeleteGroup;
		Pool.GetComponent<UIEventListener>(btnSaveGroup).onClick = OnClickbtnSaveGroup;
		gameDataManager = GameObject.Find("GameDataManager").GetComponent<GameDataManager>();
//		List<CardInfo> cardinfolist = gameDataManager.gameData.MyCardList;
//		foreach(CardInfo info in cardinfolist)
//		{
//			GameObject go = NGUITools.AddChild(cardgrid.gameObject, iconTemplate);
//			MyCardUnit mycu = go.GetComponent<MyCardUnit>();
//			mycu.UpdateMyCardUnit(info);
//		}
		UpdateGroupList();
	}

	private void OnClickbtnSaveGroup(GameObject go)
	{
		if(inputGroup.value != "")
		{
			MyCardGroup cardgroup = new MyCardGroup();
			cardgroup.groupname = inputGroup.value;
			cardgroup.cardlist = tempinfolist;
			int index = gameDataManager.gameData.CardGroupList.FindIndex(e => e.groupname == cardgroup.groupname);
			if(index != -1)
			{
				gameDataManager.gameData.CardGroupList.RemoveAt(index);
			}
			gameDataManager.gameData.CardGroupList.Add(cardgroup);
			gameDataManager.Save();
			UpdateGroupList();
		}
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
			UpdateGroupList();
		}
	}

	private void UpdateGroupList()
	{
		foreach(MyCardListUnit g in mygrouplist)
		{
			GameObject.Destroy(g.gameObject);
		}
		mygrouplist = new List<MyCardListUnit>();

		List<MyCardGroup> grouplist = gameDataManager.gameData.CardGroupList;
		foreach(MyCardGroup group in grouplist)
		{
			if(groupTemplate != null)
			{
				GameObject go = NGUITools.AddChild(groupgrid.gameObject, groupTemplate);
				MyCardListUnit unit = go.GetComponent<MyCardListUnit>();
				unit.UpdateUnit(group);
				mygrouplist.Add(unit);
			}
		}
		Invoke("DelayRepos", 0.1f);
	}

	private void DelayRepos()
	{
		groupgrid.Reposition();
	}


	public void ShowCardInfo()
	{
		List<CardInfo> cardinfolist = CardInfoManager.instance.GetAllCardInfo();
		ShowCard(cardinfolist[curIndex]);
	}

	public void SelectNextCard()
	{
		List<CardInfo> cardinfolist = CardInfoManager.instance.GetAllCardInfo();
		curIndex ++;
		if(curIndex >= cardinfolist.Count) curIndex = 0;
		ShowCard(cardinfolist[curIndex]);
	}

	public void ShowCard(CardInfo info)
	{
		carddesc.UpdateCardDes(info);
//
//		txtCardName.text = info.CardName;
//		Texture2D tex = Resources.Load(info.TextureResource) as Texture2D;
//		texCardImage.mainTexture = tex;
//		txtCardDes.text = info.DescribeText;
//		txtCardEffect.text = info.EffectText;
		currentCardInfo = info;
	}

	public void ShowGroup(MyCardGroup group)
	{
		inputGroup.value = group.groupname;
		tempinfolist = group.cardlist;
		UpdateMyCardList(tempinfolist);
	}

	private void UpdateMyCardList(List<CardInfo> infolist)
	{
		foreach(MyCardUnit card in mycardlist)
		{
			GameObject.Destroy(card.gameObject);
		}
		mycardlist = new List<MyCardUnit>();
		foreach(CardInfo info in infolist)
		{
			GameObject go = NGUITools.AddChild(cardgrid.gameObject, iconTemplate);
			MyCardUnit mycu = go.GetComponent<MyCardUnit>();
			mycu.UpdateMyCardUnit(info);
			mycardlist.Add(mycu);
		}
		Invoke("Repos", 0.1f);
	}

	void Repos()
	{
		cardgrid.Reposition();
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

	public void AddToMyCard()
	{
		if(currentCardInfo != null)
		{
			GameObject go = NGUITools.AddChild(cardgrid.gameObject, iconTemplate);
			MyCardUnit mycu = go.GetComponent<MyCardUnit>();
			mycu.UpdateMyCardUnit(currentCardInfo);
			tempinfolist.Add(currentCardInfo);
		}
		cardgrid.Reposition();
	}

	public void RemoveMyCard(CardInfo info)
	{
		int index = tempinfolist.FindIndex(e => e.CardNo == info.CardNo);
		if(index != -1)
		{
			tempinfolist.RemoveAt(index);
		}
		UpdateMyCardList(tempinfolist);
	}
}
