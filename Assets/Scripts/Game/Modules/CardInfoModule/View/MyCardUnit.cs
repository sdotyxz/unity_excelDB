using UnityEngine;
using System.Collections;
using Config;
using PureMVC.Patterns;

public class MyCard
{
	public CardInfo info;
	public int num;

	public MyCard()
	{}

	public MyCard(CardInfo cardinfo , int cardnum)
	{
		info = cardinfo;
		num = cardnum;
	}
}

public class MyCardUnit : MonoBehaviour 
{
	public UIButton btnRemove;
	public UIButton btnShowCard;

	public UITexture texCardImage;
	public UILabel txtCardNum;
	private CardInfo myCardInfo;

	private MyCard myCard;

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnRemove).onClick = OnClickbtnRemove;
		Pool.GetComponent<UIEventListener>(btnShowCard).onClick = OnClickbtnShowCard;
	}

	void OnClickbtnShowCard (GameObject go)
	{
		if(myCard != null)
		{
			Facade.Instance.SendNotification(CardInfoNotes.CARDINFO_SHOW_INFO, myCard.info);
		}
	}

	void OnClickbtnRemove (GameObject go)
	{
		if(myCard != null)
		{
			Facade.Instance.SendNotification(CardInfoNotes.CARDINFO_REMOVE_CARD, myCard.info);
		}
	}

	public void UpdateMyCardUnit(MyCard card)
	{
		if(myCard != card)
		{
			myCard = card;
			Texture2D tex = Resources.Load(card.info.TextureResource) as Texture2D;
			texCardImage.mainTexture = tex;
			txtCardNum.text = myCard.num.ToString();
		}
		else
		{
			if(myCard != null)
			{
				txtCardNum.text = myCard.num.ToString();
			}
		}
	}
}
