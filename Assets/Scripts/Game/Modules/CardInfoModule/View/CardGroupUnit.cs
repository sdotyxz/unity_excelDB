using UnityEngine;
using System.Collections;
using PureMVC.Patterns;

public class CardGroupUnit : MonoBehaviour 
{
	public UIButton btnGroup;
	public UILabel txtListName;
	private CardGroup myCardGroup;

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

	public void UpdateUnit(CardGroup group)
	{
		if(myCardGroup != group)
		{
			myCardGroup = group;
			txtListName.text = myCardGroup.groupname;
		}
	}
}
