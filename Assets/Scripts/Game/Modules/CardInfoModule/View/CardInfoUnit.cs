using UnityEngine;
using System.Collections;
using Config;
using JZWLEngine.Managers;
using PureMVC.Patterns;

public class CardInfoUnit : MonoBehaviour 
{
	public UISprite spColor;
	public UILabel txtNo;
	public UILabel txtName;
	public UILabel txtType;
	public UILabel txtLevel;
	public UILabel txtFrame;
	public UILabel txtPower;
	public UILabel txtGuard;

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
			txtNo.text = mCardInfo.CardNo;
			txtName.text = mCardInfo.CardName;
			string frame = mCardInfo.Linkframe;
			if(frame == "null") frame = "  ";
			txtType.text = mCardInfo.Type;
			txtLevel.text = mCardInfo.Level.ToString();
			txtFrame.text = frame;
			txtPower.text = mCardInfo.Power.ToString();
			txtGuard.text = mCardInfo.GuardPoint.ToString();
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
