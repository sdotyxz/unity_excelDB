using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;

public class CardInfoView : MonoBehaviour
{
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

	public UIGrid cardgrid;
	public UIGrid grid;
	public GameObject template;
	private List<CardInfoUnit> infounitlist = new List<CardInfoUnit>();

	public GameObject iconTemplate;

	private GameDataManager gameDataManager;

	private CardInfo currentCardInfo;

	void Start()
	{
		gameDataManager = GameObject.Find("GameDataManager").GetComponent<GameDataManager>();
		List<string> cardnamelist = gameDataManager.gameData.cardname;
		foreach(string name in cardnamelist)
		{
			Debug.Log ("------ " + name + " ------");
		}
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
		txtCardName.text = info.CardName;
		Texture2D tex = Resources.Load(info.TextureResource) as Texture2D;
		texCardImage.mainTexture = tex;
		txtCardDes.text = info.DescribeText;
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


		Dictionary<string, object> querydic = new Dictionary<string, object>();
		querydic.Add("Color", color);
		querydic.Add("Level", level);
		querydic.Add("Type", type);
		querydic.Add("Linkframe", frame);
		querydic.Add("Serials", serials);

		List<CardInfo> infolist = CardInfoManager.instance.GetCardListByMultipleSearch(querydic);

		List<CardInfo> finallist = infolist.FindAll(e => e.CardName.Contains(cardname));
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
		}
		cardgrid.Reposition();
	}
}
