using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;
using JZWLEngine.Managers;

public class CardInfoView : MonoBehaviour
{
	public CardDesc carddesc;
	public CardStorage cardStorage;
	public CardGroupStorage groupStorage;

	public UIButton btnAddCard;
	public UIButton btnSaveGroup;
	public UIButton btnDeleteGroup;
	public UIInput inputGroup;
	private List<MyCard> tempcardlist = new List<MyCard>();

	private GameDataManager gameDataManager;

	public UILabel txtGroupRule1;
	public UILabel txtGroupRule2;
	public UILabel txtGroupRule3;
	public UITable tableRule;
	

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnAddCard).onClick = OnClickbtnAddCard;
		Pool.GetComponent<UIEventListener>(btnDeleteGroup).onClick = OnClickbtnDeleteGroup;
		Pool.GetComponent<UIEventListener>(btnSaveGroup).onClick = OnClickbtnSaveGroup;
		gameDataManager = GameObject.Find("GameDataManager").GetComponent<GameDataManager>();
		UpdateGroupList();
		cardStorage.Build(54);
	}

	void OnClickbtnAddCard (GameObject go)
	{
		if(carddesc.cardinfo != null)
		{
			int index = tempcardlist.FindIndex(e => e.info.CardNo == carddesc.cardinfo.CardNo);
			if(index != -1)
			{
				if(tempcardlist[index].num <4)
				{
					tempcardlist[index].num++;
				}
			}
			else
			{
				MyCard card = new MyCard(carddesc.cardinfo, 1);
				tempcardlist.Add(card);
			}
		}
		cardStorage.Fill(tempcardlist);
		CheckGroupRule(tempcardlist);
	}

	private void OnClickbtnSaveGroup(GameObject go)
	{
		if(inputGroup.value != "")
		{
			CardGroup group = new CardGroup();
			group.groupname = inputGroup.value;
			group.cardlist = tempcardlist;
			int index = gameDataManager.gameData.CardGroupList.FindIndex(e => e.groupname == group.groupname);
			if(index != -1)
			{
				gameDataManager.gameData.CardGroupList.RemoveAt(index);
			}
			gameDataManager.gameData.CardGroupList.Add(group);
			gameDataManager.Save();
			UpdateGroupList();
		}
	}

	void UpdateGroupList ()
	{
		groupStorage.Clear();
		Pool.GetComponent<CommonStorage>(groupStorage).OnBuildFinish = OnCardGroupBuild;
		groupStorage.Build(gameDataManager.gameData.CardGroupList.Count);
	}

	public void ShowGroupCards(CardGroup group)
	{
		inputGroup.value = group.groupname;
		cardStorage.Fill(group.cardlist);
		tempcardlist = group.cardlist;
		CheckGroupRule(group.cardlist);
	}

	private void CheckGroupRule(List<MyCard> cardlist)
	{
		// Rule1 デッキの枚数は54枚です。
		// Rule2 ひとつのデッキの中に、レベル0でないリンクフレーム（Σ・Ω）を持つカードは合計16枚入れなくてはなりません。
		// それより多くなっても少なくなってもいけません。
		// Rule3 ひとつのデッキの中に、レベル0のプログレスは４枚入れなくてはなりません。
		//それより多くなっても少なくなってもいけません
		int rule1count = 0;
		int rule2count = 0;
		int rule3count = 0;
		string rule1 = "";
		string rule2 = "";
		string rule3 = "";
		foreach(MyCard card in cardlist)
		{
			rule1count += card.num;
			if(card.info.Linkframe == "Σ" || card.info.Linkframe == "Ω")
			{
				rule2count += card.num;
			}
			if(card.info.Level == 0)
			{
				rule3count += card.num;
			}
		}
		if(rule1count != 54)
		{
			rule1 += "[ff0000]";
		}
		rule1 += OTManager.instance.GetOT("GROUP_RULE_C_1", rule1count.ToString());
		txtGroupRule1.text = rule1;

		if(rule2count != 16)
		{
			rule2 += "[ff0000]";
		}
		rule2 += OTManager.instance.GetOT("GROUP_RULE_C_2", rule2count.ToString());
		txtGroupRule2.text = rule2;
		
		if(rule3count != 4)
		{
			rule3 += "[ff0000]";
		}
		rule3 += OTManager.instance.GetOT("GROUP_RULE_C_3", rule3count.ToString());
		txtGroupRule3.text = rule3;
		tableRule.Reposition();
	}

	void OnCardGroupBuild(GameObject go)
	{
		groupStorage.Fill(gameDataManager.gameData.CardGroupList);
	}

	void OnClickbtnDeleteGroup (GameObject go)
	{
		if(inputGroup.value != "")
		{
			string groupname = inputGroup.value;
			int index = gameDataManager.gameData.CardGroupList.FindIndex(e => e.groupname == groupname);
			if(index != -1)
			{
				gameDataManager.gameData.CardGroupList.RemoveAt(index);
			}
			gameDataManager.Save();
		}
		UpdateGroupList();
	}

	public void ShowCard(CardInfo info)
	{
		carddesc.UpdateCardDes(info);
	}

	public void RemoveMyCard(CardInfo info)
	{
		int index = tempcardlist.FindIndex(e => e.info.CardNo == info.CardNo);
		if(index != -1)
		{
			tempcardlist[index].num --;
			if(tempcardlist[index].num == 0)
			{
				tempcardlist.RemoveAt(index);
			}
		}
		cardStorage.Fill(tempcardlist);
	}
}
