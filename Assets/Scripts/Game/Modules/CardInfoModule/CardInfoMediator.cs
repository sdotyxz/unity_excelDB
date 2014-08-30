using JZWLEngine.Managers;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using JZWLEngine.Loader;
using Config;

public class CardInfoMediator : Mediator
{
    new public static string NAME = "CardInfoMediator";
    private CardInfoScene _scene;
    private CardInfoView _UI;

    public CardInfoMediator(object data)
        : base(NAME)
    {
           if (Scene.Instance == null)
           {
               throw new UnityException("App Error: Not exist Scene's instance.");
           }

           if (!(Scene.Instance is CardInfoScene))
           {
                throw new UnityException("App Error: Scene instance is not a CardInfoScene.");
           }
           _scene = Scene.Instance as CardInfoScene;

           GameObject go = _scene.birthPos.gameObject;
           _UI = go.GetComponent<CardInfoView>();
		   _UI.ShowCardInfo();
    }

    public override IList<string> ListNotificationInterests()
    {
        return new List<String>()
        {
			CardInfoNotes.CARDINFO_SHOW_INFO
        };
    }

    public override void HandleNotification(PureMVC.Interfaces.INotification notification)
    {
        switch (notification.Name)
        {
		case CardInfoNotes.CARDINFO_SHOW_INFO:
			OnShowInfo((CardInfo)notification.Body);
			break;
            default:
                break;
        }
    }

	void OnShowInfo (CardInfo cardInfo)
	{
		if(_UI != null)
		{
			_UI.ShowCard(cardInfo);
		}
	}

    public override void OnRegister()
    {
    }

    public override void OnRemove()
    {

    }
}
