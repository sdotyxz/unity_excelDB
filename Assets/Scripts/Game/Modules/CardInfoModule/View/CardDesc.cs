using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;
using JZWLEngine.Managers;

public class CardDesc : MonoBehaviour 
{
	public List<CardEffectUnit> effectlist = new List<CardEffectUnit>();
	public UITexture texCardImage;
	public UILabel txtCardName;
	public UILabel txtCardDes;
	public UITable tableEffect;

	private CardInfo mCardInfo;

	public void UpdateCardDes(CardInfo info)
	{
		if(mCardInfo != info)
		{
			mCardInfo = info;
			txtCardName.text = info.CardName;
			Texture2D tex = Resources.Load(info.TextureResource) as Texture2D;
			texCardImage.mainTexture = tex;
			txtCardDes.text = FormatCardDes(info);
			ShowCardEffect(info);
		}
	}

	private void ShowCardEffect(CardInfo info)
	{
		if(info.EffectText != "null")
		{
			effectlist[0].gameObject.SetActive(true);
			string[] effectgroup = info.EffectText.Split('&');
			string effect1 = effectgroup[0];
			string[] effectdes1 = effect1.Split('#');
			string effecttext = effectdes1[1];
			effectlist[0].spEffectDes.spriteName = "EF_" + effectdes1[0];
			effectlist[0].txtEffectDes.text = effecttext;
			tableEffect.Reposition();
		}
	}

	private string FormatCardDes(CardInfo info)
	{
		string carddes = "";
		carddes += "Power:" + info.Power.ToString() + "\n";
		carddes += "Guard Point:" + info.GuardPoint.ToString() + "\n";
		carddes += OTManager.instance.GetOT("CARD_COLOR_PROPERTY", info.Color) + "\n";
		carddes += "Card No.:" + info.CardNo + "\n";
		carddes += "Rare:" + info.Rare + "\n";
		carddes += OTManager.instance.GetOT("CARD_TYPE", info.Type) + "\n";
		carddes += "Level:" + info.Level.ToString() + "\n";
		if(info.Linkframe != "null")
		{
			carddes += "Frame:" + info.Linkframe + "\n";
		}
		carddes += "Strike:" + info.Strike.ToString() + "\n";
		carddes += info.Feature1 + "/" + info.Feature2 + "\n";
		return carddes;
	}
}
















