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
			CardInfoNotes.CARDINFO_SHOW_INFO,
			CardInfoNotes.CARDINFO_SHOW_GROUP,
			CardInfoNotes.CARDINFO_REMOVE_CARD
        };
    }

    public override void HandleNotification(PureMVC.Interfaces.INotification notification)
    {
        switch (notification.Name)
        {
		case CardInfoNotes.CARDINFO_SHOW_INFO:
			OnShowInfo((CardInfo)notification.Body);
			break;
		case CardInfoNotes.CARDINFO_SHOW_GROUP:
			OnShowGroup((MyCardGroup)notification.Body);
			break;
		case CardInfoNotes.CARDINFO_REMOVE_CARD:
			OnRemoveInfo((CardInfo)notification.Body);
			break;
            default:
                break;
        }
    }

	void OnShowGroup (MyCardGroup myCardGroup)
	{
		if(_UI != null)
		{
			_UI.ShowGroup(myCardGroup);
		}
	}

	void OnShowInfo (CardInfo cardInfo)
	{
		if(_UI != null)
		{
			_UI.ShowCard(cardInfo);
		}
	}

	void OnRemoveInfo (CardInfo cardInfo)
	{
		if(_UI != null)
		{
			_UI.RemoveMyCard(cardInfo);
		}
	}

    public override void OnRegister()
    {
    }

    public override void OnRemove()
    {

    }
}
