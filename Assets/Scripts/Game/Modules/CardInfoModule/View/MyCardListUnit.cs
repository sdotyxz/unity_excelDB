using UnityEngine;
using System.Collections;
using PureMVC.Patterns;

public class MyCardListUnit : MonoBehaviour 
{
	public UIButton btnGroup;
	public UILabel txtListName;
	private MyCardGroup myCardGroup;

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnGroup).onClick = OnClickbtnGroup;
	}

	void OnClickbtnGroup (GameObject go)
	{
		if(myCardGroup != null)
		{
			Facade.Instance.SendNotification(CardInfoNotes.CARDINFO_SHOW_GROUP, myCardGroup);
		}
	}

	public void UpdateUnit(MyCardGroup cardGroup)
	{
		if(myCardGroup != cardGroup)
		{
			myCardGroup = cardGroup;
			txtListName.text = myCardGroup.groupname;
		}
	}
}
