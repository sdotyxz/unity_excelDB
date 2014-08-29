using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PureMVC.Patterns;

public class CardInfoCommand : SimpleCommand
{
   public static string NAME = "CardInfoCommand";
    public override void Execute(PureMVC.Interfaces.INotification notification)
    {
        CardInfoProxy proxy = Facade.RetrieveProxy(CardInfoProxy.NAME) as CardInfoProxy;
        switch (notification.Type)
        {
               case "":
               
               break;
        }
    }
}
