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
	public UILabel txtDes;

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
		foreach(CardEffectUnit eu in effectlist)
		{
			eu.gameObject.SetActive(false);
		}
		if(info.EffectText != "null")
		{
			string[] effectgroup = info.EffectText.Split('&');
			for(int i = 0; i < effectgroup.Length; i ++)
			{
				effectlist[i].gameObject.SetActive(true);
				string effect = effectgroup[i];
				string[] effectdes = effect.Split('#');
				string effecttext = effectdes[1];
				effectlist[i].spEffectDes.spriteName = "EF_" + effectdes[0];
				effectlist[i].txtEffectDes.text = effecttext;
			}
		}
		txtDes.text = "\n" + info.DescribeText;
		tableEffect.Reposition();
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
















