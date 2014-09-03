using UnityEngine;
using System.Collections;
using Config;
using PureMVC.Patterns;

public class MyCardUnit : MonoBehaviour 
{
	public UIButton btnRemove;
	public UISprite cardICON;
	private CardInfo myCardInfo;


	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnRemove).onClick = OnClickbtnRemove;
	}

	void OnClickbtnRemove (GameObject go)
	{
		if(myCardInfo != null)
		{
			Facade.Instance.SendNotification(CardInfoNotes.CARDINFO_REMOVE_CARD, myCardInfo);
		}
	}

	public void UpdateMyCardUnit(CardInfo cardinfo)
	{
		if(myCardInfo != cardinfo)
		{
			myCardInfo = cardinfo;
			cardICON.spriteName = myCardInfo.CardNo;
		}
	}
}
