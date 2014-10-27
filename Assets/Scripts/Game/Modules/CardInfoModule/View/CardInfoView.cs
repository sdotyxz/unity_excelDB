using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;
using JZWLEngine.Managers;
using System.IO;

public class CardInfoView : MonoBehaviour
{
	public CardDesc carddesc;
	public CardStorage cardStorage;
	public CardGroupStorage groupStorage;
	public CardSearchField searchField;

	public UIButton btnScreenshot;
	public UIButton btnAddCard;
	public UIButton btnSaveGroup;
	public UIButton btnDeleteGroup;
	public UIButton btnClearTemp;
	public UIButton btnExportCardGroup;

	public UIInput inputGroup;
	private List<MyCard> tempcardlist = new List<MyCard>();

	private GameDataManager gameDataManager;

	public UILabel txtGroupRule1;
	public UILabel txtGroupRule2;
	public UILabel txtGroupRule3;
	public UITable tableRule;

	private int storagesize = 80;

	private string GroupDesFileName = "GroupDes.txt";

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnScreenshot).onClick = OnClickbtnScreenShot;

		Pool.GetComponent<UIEventListener>(btnAddCard).onClick = OnClickbtnAddCard;
		Pool.GetComponent<UIEventListener>(btnDeleteGroup).onClick = OnClickbtnDeleteGroup;
		Pool.GetComponent<UIEventListener>(btnSaveGroup).onClick = OnClickbtnSaveGroup;
		Pool.GetComponent<UIEventListener>(btnClearTemp).onClick = OnClickbtnClearTemp;
		Pool.GetComponent<UIEventListener>(btnExportCardGroup).onClick = OnClickbtnExportCardGroup;

		gameDataManager = GameObject.Find("GameDataManager").GetComponent<GameDataManager>();
		UpdateGroupList();
		cardStorage.Build(storagesize);
	}

	void OnClickbtnExportCardGroup (GameObject go)
	{
		string filepath = Application.dataPath + "/" + GroupDesFileName;
		if(File.Exists(filepath))
		{
			File.Delete(filepath);
		}

		FileStream fs = File.Create(filepath);

		StreamWriter sw = new StreamWriter(fs);

		string content = "";

		Dictionary<string, int> colordic = new Dictionary<string, int>();
		Dictionary<int, int> leveldic = new Dictionary<int, int>();
		Dictionary<string, int> kinddic = new Dictionary<string, int>();
		Dictionary<string, int> framedic = new Dictionary<string, int>();

		int boostnum = 0;
		int total = 0;

		foreach(MyCard card in tempcardlist)
		{
			content += card.no + ":" + card.cardcfg.name + "  " + card.num.ToString() + OTManager.instance.GetOT("COMMON_NUM") + "\n";

			if(colordic.ContainsKey(card.cardcfg.color)) colordic[card.cardcfg.color] += card.num;
			else colordic.Add(card.cardcfg.color, card.num);

			if(leveldic.ContainsKey(card.cardcfg.level)) leveldic[card.cardcfg.level] += card.num;
			else leveldic.Add(card.cardcfg.level, card.num);

			if(kinddic.ContainsKey(card.cardcfg.kind)) kinddic[card.cardcfg.kind] += card.num;
			else kinddic.Add(card.cardcfg.kind, card.num);

			if(framedic.ContainsKey(card.cardcfg.frame)) framedic[card.cardcfg.frame] += card.num;
			else framedic.Add(card.cardcfg.frame, card.num);

			if(card.cardcfg.boost == "boost") boostnum += card.num;

			total += card.num;
		}

		content += "\n" + OTManager.instance.GetOT("GROUP_DES_COLOR");

		foreach(string color in colordic.Keys)
		{
			content += "[" + color + "]" + "-" + colordic[color].ToString() + OTManager.instance.GetOT("COMMON_NUM") + "  ";
		}

		content += "\n" + OTManager.instance.GetOT("GROUP_DES_LEVEL");

		foreach(int level in leveldic.Keys)
		{
			content += "[LV" + level.ToString() + "]" + "-" + leveldic[level].ToString() + OTManager.instance.GetOT("COMMON_NUM") + "  ";
		}

		content += "\n" + OTManager.instance.GetOT("GROUP_DES_KIND");

		foreach(string kind in kinddic.Keys)
		{
			content += "[" + kind + "]" + "-" + kinddic[kind].ToString() + OTManager.instance.GetOT("COMMON_NUM") + "  ";
		}

		content += "\n" + OTManager.instance.GetOT("GROUP_DES_FRAME");//"Frame:";

		foreach(string frame in framedic.Keys)
		{
			content += OTManager.instance.GetOT("CARD_FRAME_" + frame) + "-" + framedic[frame].ToString() + OTManager.instance.GetOT("COMMON_NUM") + "  ";
		}

		content += "\n" + OTManager.instance.GetOT("GROUP_DES_BOOST") + boostnum.ToString() + OTManager.instance.GetOT("COMMON_NUM") + "\n" 
			+ OTManager.instance.GetOT("GROUP_DES_TOTAL") + total.ToString() + OTManager.instance.GetOT("COMMON_NUM");

		sw.Write(content);

		sw.Close();
	}

	void OnClickbtnClearTemp (GameObject go)
	{
		tempcardlist = new List<MyCard>();
		cardStorage.HideAllSlot();
		CheckGroupRule(tempcardlist);
	}

	void OnClickbtnScreenShot (GameObject go)
	{
		Application.CaptureScreenshot("test.png");
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			OnClickbtnAddCard(null);
		}

		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			int index = searchField.SearchList.FindIndex(e => e.no == carddesc.desCfg.no);
			if(index != -1)
			{
				if(index + 1 < searchField.SearchList.Count) searchField.previewindex = index + 1;
			}
			if(searchField.SearchList.Count != 0) ShowCard(searchField.SearchList[searchField.previewindex]);
		}

		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			int index = searchField.SearchList.FindIndex(e => e.no == carddesc.desCfg.no);
			if(index != -1)
			{
				if(index - 1 > 0) searchField.previewindex = index - 1;
			}
			if(searchField.SearchList.Count != 0) ShowCard(searchField.SearchList[searchField.previewindex]);
		}
	}

	void OnClickbtnAddCard (GameObject go)
	{
		if(tempcardlist.Count < storagesize)
		{
			if(carddesc.desCfg != null)
			{
				int index = tempcardlist.FindIndex(e => e.no == carddesc.desCfg.no);
				if(index != -1)
				{
					if(tempcardlist[index].num < 4) tempcardlist[index].num ++;
				}
				else
				{
					MyCard card = new MyCard(carddesc.desCfg.no, 1);
					tempcardlist.Add(card);
				}
			}

			cardStorage.Fill(tempcardlist);
			CheckGroupRule(tempcardlist);

			int moveindex = cardStorage.CardUnitList.FindIndex(e => e.Card.no == carddesc.desCfg.no);
			if(moveindex != -1) cardStorage.MoveToIndex(moveindex);
		}
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
			if(card.cardcfg.frame == "sigma" || card.cardcfg.frame == "omega")
			{
				rule2count += card.num;
			}
			if(card.cardcfg.level == 0 && card.cardcfg.kind == "PG")
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

	public void ShowCard(cfgcard cfg)
	{
		carddesc.UpdateCardDes(cfg);
	}

	public void RemoveMyCard(string cardno)
	{
		int index = tempcardlist.FindIndex(e => e.no == cardno);
		if(index != -1)
		{
			tempcardlist[index].num --;
			if(tempcardlist[index].num == 0)
			{
				tempcardlist.RemoveAt(index);
			}
		}
		cardStorage.Fill(tempcardlist);
		CheckGroupRule(tempcardlist);
	}
}
