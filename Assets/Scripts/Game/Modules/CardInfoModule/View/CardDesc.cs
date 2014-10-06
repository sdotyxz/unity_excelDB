using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;
using JZWLEngine.Managers;

public class CardDesc : MonoBehaviour 
{
	public GameObject effectTemplate;
	public UITexture texCardImage;
	public UILabel txtCardName;
	public UILabel txtCardDes;
	public UITable tableEffect;
	public UILabel txtDes;
	private List<GameObject> effectlist = new List<GameObject>();

	private CardInfo mCardInfo;
	public CardInfo cardinfo
	{
		get
		{
			return mCardInfo;
		}
	}

	public void UpdateCardDes(CardInfo info)
	{
		if(mCardInfo != info)
		{
			mCardInfo = info;
			txtCardName.text = info.CardName;
			string[] namepre = info.CardNo.Split('-');
			string imagepath = Resconfig.RES_CARD_IMAGE + namepre[0] + "/" + info.CardNo + "." + info.ResourceID;
			Texture2D tex = Resources.Load(imagepath) as Texture2D;
			texCardImage.mainTexture = tex;
			txtCardDes.text = FormatCardDes(info);
			ShowCardEffect(info);
		}
	}

	private void ShowCardEffect(CardInfo info)
	{
		foreach(GameObject eu in effectlist)
		{
			Destroy(eu);
		}
		effectlist = new List<GameObject>();
		if(info.EffectText != "null")
		{
			string[] effectgroup = info.EffectText.Split('&');
			for(int i = 0; i < effectgroup.Length; i ++)
			{
				GameObject go = NGUITools.AddChild(tableEffect.gameObject, effectTemplate);
				effectlist.Add(go);
				CardEffectUnit efu = go.GetComponent<CardEffectUnit>();
				efu.UpdateEffect(effectgroup[i]);

			}
		}
		if(info.DescribeText != "null") txtDes.text = "\n" + info.DescribeText;
		StartCoroutine(DelayRepos());
	}

	IEnumerator DelayRepos()
	{
		yield return new WaitForEndOfFrame();
		tableEffect.Reposition();
	}

	private string FormatCardDes(CardInfo info)
	{
		string carddes = "";
		carddes += OTManager.instance.GetOT("CARD_POWER", info.Power.ToString()) + "\n";//"Power:" + info.Power.ToString() + "\n";
		carddes += OTManager.instance.GetOT("CARD_GUARD", info.GuardPoint.ToString()) + "\n";//"Guard Point:" + info.GuardPoint.ToString() + "\n";
		carddes += OTManager.instance.GetOT("CARD_COLOR", info.Color) + "\n";
		carddes += OTManager.instance.GetOT("CARD_NO", info.CardNo) + "\n";//"Card No.:" + info.CardNo + "\n";
		carddes += OTManager.instance.GetOT("CARD_RARE", info.Rare) + "\n";//"Rare:" + info.Rare + "\n";
		carddes += OTManager.instance.GetOT("CARD_TYPE", info.Type) + "\n";
		carddes += OTManager.instance.GetOT("CARD_LEVEL", info.Level.ToString()) + "\n";//"Level:" + info.Level.ToString() + "\n";
		if(info.Linkframe != "null")
		{
			carddes += OTManager.instance.GetOT("CARD_FRAME", info.Linkframe) + "\n";//"Frame:" + info.Linkframe + "\n";
		}
		carddes += OTManager.instance.GetOT("CARD_STRIKE", info.Strike.ToString()) + "\n";//"Strike:" + info.Strike.ToString() + "\n";
		carddes += OTManager.instance.GetOT("CARD_FEATURE", info.Feature);//info.Feature + "\n";
		return carddes;
	}
}
















