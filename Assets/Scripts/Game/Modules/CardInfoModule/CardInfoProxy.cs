using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using JZWLEngine.Managers;

public class CardInfoProxy:Proxy
{
    public new static string NAME = "CardInfoProxy";
    public CardInfoProxy()
        : base(NAME)
    {

    }

    public override void OnRegister()
    {

    }
}
