using UnityEngine;
using System.Collections;
using Config;

public class MyCardUnit : MonoBehaviour 
{
	public UISprite cardICON;
	private CardInfo myCardInfo;

	public void UpdateMyCardUnit(CardInfo cardinfo)
	{
		if(myCardInfo != cardinfo)
		{
			myCardInfo = cardinfo;
			cardICON.spriteName = myCardInfo.CardNo;
		}
	}
}
