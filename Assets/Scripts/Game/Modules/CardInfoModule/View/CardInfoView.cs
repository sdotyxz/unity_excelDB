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

	void Start()
	{
	}

	public void ShowCardInfo()
	{
		List<CardInfo> cardinfolist = CardInfoManager.instance.GetAllCardInfo();
		string cardname = "";
		txtCardName.text = cardinfolist[curIndex].CardName;
		Texture2D tex = Resources.Load(cardinfolist[curIndex].TextureResource) as Texture2D;
		texCardImage.mainTexture = tex;
		txtCardDes.text = cardinfolist[curIndex].DescribeText;
	}

	public void SelectNextCard()
	{
		List<CardInfo> cardinfolist = CardInfoManager.instance.GetAllCardInfo();
		curIndex ++;
		if(curIndex >= cardinfolist.Count) curIndex = 0;
		txtCardName.text = cardinfolist[curIndex].CardName;
		Texture2D tex = Resources.Load(cardinfolist[curIndex].TextureResource) as Texture2D;
		texCardImage.mainTexture = tex;
		txtCardDes.text = cardinfolist[curIndex].DescribeText;
	}
}
