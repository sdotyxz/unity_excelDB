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

	public UIGrid grid;
	public GameObject template;
	private List<CardInfoUnit> infounitlist = new List<CardInfoUnit>();

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
}
