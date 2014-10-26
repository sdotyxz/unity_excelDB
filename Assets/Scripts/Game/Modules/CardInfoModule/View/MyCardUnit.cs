using UnityEngine;
using System.Collections;
using Config;
using PureMVC.Patterns;

public class MyCard
{
	public string no;
	public cfgcard cardcfg { get { return CardInfoManager.instance.GetCardCfgByNo(no); } }
	public int num;

	public MyCard()
	{}

	public MyCard(string cardno , int cardnum)
	{
		no = cardno;
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
	public MyCard Card { get { return myCard; } }

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnRemove).onClick = OnClickbtnRemove;
		Pool.GetComponent<UIEventListener>(btnShowCard).onClick = OnClickbtnShowCard;
	}

	void OnClickbtnShowCard (GameObject go)
	{
		if(myCard != null)
		{
			Facade.Instance.SendNotification(CardInfoNotes.CARDINFO_SHOW_INFO, myCard.cardcfg);
		}
	}

	void OnClickbtnRemove (GameObject go)
	{
		if(myCard != null)
		{
			Facade.Instance.SendNotification(CardInfoNotes.CARDINFO_REMOVE_CARD, myCard.no);
		}
	}

	public void UpdateMyCardUnit(MyCard card)
	{
		if(myCard != card)
		{
			myCard = card;
			string[] namepre = myCard.no.Split('-');
			string imagepath = Resconfig.RES_CARD_IMAGE + namepre[0] + "/" + card.cardcfg.img;
			Texture2D tex = Resources.Load(imagepath) as Texture2D;
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
