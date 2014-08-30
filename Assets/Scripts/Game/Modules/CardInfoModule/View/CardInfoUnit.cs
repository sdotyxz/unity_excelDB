using UnityEngine;
using System.Collections;
using Config;
using JZWLEngine.Managers;
using PureMVC.Patterns;

public class CardInfoUnit : MonoBehaviour 
{
	public UISprite spColor;
	public UILabel txtNoName;
	public UILabel txtOtherInfo;

	private CardInfo mCardInfo;

	public UIButton btnUnit;

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnUnit).onClick = OnClickUnit;
	}

	void OnClickUnit (GameObject go)
	{
		Debug.Log ("------ ------");
		Facade.Instance.SendNotification(CardInfoNotes.CARDINFO_SHOW_INFO, mCardInfo);
	}

	public void UpdateCardInfo(CardInfo info)
	{
		if(mCardInfo != info)
		{
			mCardInfo = info;
			spColor.spriteName = CalCardColorString(mCardInfo);
			txtNoName.text = OTManager.instance.GetOT("CARD_NO_AND_NAME", mCardInfo.CardNo, mCardInfo.CardName);
			string frame = mCardInfo.Linkframe;
			if(frame == "null") frame = "  ";
			txtOtherInfo.text = OTManager.instance.GetOT("CARD_OTHER_INFO", mCardInfo.Type, mCardInfo.Level.ToString(), 
			                                             frame, mCardInfo.Power.ToString(), mCardInfo.GuardPoint.ToString());
		}
	}

	private string CalCardColorString(CardInfo info)
	{
		string color = "card_icon_color";
		if(info != null)
		{
			if(info.Color == OTManager.instance.GetOT("CARD_COLOR_1"))
			{
				color += "1";
			}
			else if (info.Color == OTManager.instance.GetOT("CARD_COLOR_2"))
			{
				color += "2";
			}
			else if (info.Color == OTManager.instance.GetOT("CARD_COLOR_3"))
			{
				color += "3";
			}
			else if (info.Color == OTManager.instance.GetOT("CARD_COLOR_4"))
			{
				color += "4";
			}
		}
		return color;
	}

}
