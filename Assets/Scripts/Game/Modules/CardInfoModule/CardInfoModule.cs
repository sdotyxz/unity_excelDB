using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JZWLEngine.Managers.Helper;
using JZWLEngine.Loader;
using JZWLEngine.Managers;

public class CardInfoModule : BaseModule
{
    public CardInfoModule(): base(SceneConfig.SceneName.CardInfo.ToString())
    {
    }

    protected override void _Start()
    {
        facade.RegisterProxy(new CardInfoProxy());
        facade.RegisterMediator(new CardInfoMediator(_data));
        facade.RegisterCommand(CardInfoCommand.NAME, typeof(CardInfoCommand));
    }

    protected override void _Dispose()
    {
        facade.RemoveProxy(CardInfoProxy.NAME);
        facade.RemoveMediator(CardInfoMediator.NAME);
        facade.RemoveCommand(CardInfoCommand.NAME);
    }
}
