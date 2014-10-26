using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using JZWLEngine.Managers;
using Config;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class CardInfoManager : ManagerTemplate<CardInfoManager> 
{
	private CardInfoDataEntity _cardinfoEntity;
	private cfgcardDataEntity _cardcfgEntity;

	public List<cfgcard> cardcfglist
	{
		get { return _cardcfgEntity.data; }
	}

	private List<string> _colorlist = new List<string>();
	public List<string> ColorList { get { return _colorlist; } }

	private List<int> _levellist = new List<int>();
	public List<int> LevelList { get { return _levellist; }}

	private List<string> _kindlist = new List<string>();
	public List<string> KindList { get { return _kindlist; } }

	private List<string> _framelist = new List<string>();
	public List<string> FrameList { get { return _framelist; } }

	private List<string> _serialslist = new List<string>();
	public List<string> SerialsList { get { return _serialslist; } }

	protected override void OnInit ()
	{
		Debug.Log ("------ CardInfoManager ------");
		_cardinfoEntity = Resources.Load(Resconfig.CONFIG_CARDINFO) as CardInfoDataEntity;
		if(_cardinfoEntity == null)
		{
			Debug.LogError("Exception: Lost CardInfo Entity !!");
		}

		_cardcfgEntity = Resources.Load(Resconfig.CONIFG_CARDCFG) as cfgcardDataEntity;
		if(_cardcfgEntity == null)
		{
			Debug.LogError("Exception: Lost CardCfg Entity !!");
		}

		GetAllSearchElement();

		//CheckAllTexture();
	}

	void CheckAllTexture ()
	{
		foreach(cfgcard card in _cardcfgEntity.data)
		{
			string[] namepre = card.no.Split('-');
			string imagepath = Resconfig.RES_CARD_IMAGE + namepre[0] + "/" + card.img;
			Texture2D tex = Resources.Load(imagepath) as Texture2D;
			if(tex == null)
				Debug.Log(imagepath);
		}
	}

	public List<CardInfo> GetAllCardInfo()
	{
		return _cardinfoEntity.data;
	}

	private void GetAllSearchElement()
	{
		if(_cardcfgEntity != null)
		{
			foreach(cfgcard card in _cardcfgEntity.data)
			{
				if(!_colorlist.Exists(e => e == card.color)) _colorlist.Add(card.color);
				if(!_levellist.Exists(e => e == card.level)) _levellist.Add(card.level);
				if(!_kindlist.Exists(e => e == card.kind)) _kindlist.Add(card.kind);
				if(!_framelist.Exists(e => e == card.frame)) _framelist.Add(card.frame);
				if(!_serialslist.Exists(e => e == card.serials)) _serialslist.Add(card.serials);
			}
		}
	}

	public CardInfo GetCardInfoByName(string name)
	{
		return _cardinfoEntity.data.Find(e => e.CardName.Contains(name));
	}

	public cfgcard GetCardCfgByNo(string no)
	{
		return _cardcfgEntity.data.Find(e => e.no == no);
	}

	public List<CardInfo> GetCardListByColor(string color)
	{
		return _cardinfoEntity.data.FindAll(e => e.Color == color);
	}

	public List<CardInfo> GetCardListByMultipleSearch(Dictionary<string, object> queryDict)
	{
		IQueryable<CardInfo> queryableData = _cardinfoEntity.data.AsQueryable<CardInfo>();

		Expression left = Expression.Constant(1);
		Expression right = Expression.Constant(1);
		Expression predicateBody = Expression.Equal(left, right);
		ParameterExpression pe = Expression.Parameter(typeof(CardInfo), "cardinfo");

		foreach(KeyValuePair<string, object> keyvalue in queryDict)
		{
			if(keyvalue.Value.ToString() != "*" && keyvalue.Value.ToString() != "-1")
			{
				FieldInfo finfo = typeof(CardInfo).GetField(keyvalue.Key.ToString());
				left = Expression.Field(pe, finfo);
				right = Expression.Constant(keyvalue.Value);
				Expression e = Expression.Equal(left, right);
				predicateBody = Expression.And(predicateBody, e);
			}
		}

		MethodCallExpression whereCallExpression = Expression.Call(
			typeof(Queryable),
			"Where",
			new Type[] { queryableData.ElementType },
		queryableData.Expression,
		Expression.Lambda<Func<CardInfo, bool>>(predicateBody, new ParameterExpression[]{ pe }));

		IQueryable<CardInfo> results = queryableData.Provider.CreateQuery<CardInfo>(whereCallExpression);

		List<CardInfo> cardlist = results.ToList();

		return cardlist;
	}

	public List<cfgcard> GetCardCfgListByMultipleSearch(Dictionary<string, object> queryDict)
	{
		IQueryable<cfgcard> queryableData = _cardcfgEntity.data.AsQueryable<cfgcard>();

		Expression left = Expression.Constant(1);
		Expression right = Expression.Constant(1);
		Expression predicateBody = Expression.Equal(left, right);
		ParameterExpression pe = Expression.Parameter(typeof(cfgcard), "cfgcard");

		foreach(KeyValuePair<string, object> keyvalue in queryDict)
		{
			if(keyvalue.Value.ToString() != "*" && keyvalue.Value.ToString() != "-1")
			{
				FieldInfo finfo = typeof(cfgcard).GetField(keyvalue.Key.ToString());
				left = Expression.Field(pe, finfo);
				right = Expression.Constant(keyvalue.Value);
				Expression e = Expression.Equal(left, right);
				predicateBody = Expression.And(predicateBody, e);
			}
		}

		MethodCallExpression whereCallExpression = Expression.Call(
			typeof(Queryable),
			"Where",
			new Type[] { queryableData.ElementType },
		queryableData.Expression,
		Expression.Lambda<Func<cfgcard, bool>>(predicateBody, new ParameterExpression[]{ pe }));

		IQueryable<cfgcard> results = queryableData.Provider.CreateQuery<cfgcard>(whereCallExpression);

		List<cfgcard> cardcfglist = results.ToList();

		return cardcfglist;
	}

	public string CalCardColorString(cfgcard cfg)
	{
		if(cfg != null)
		{
			int index = ColorList.FindIndex(e => e == cfg.color);
			string color = "card_icon_color" + (index + 1).ToString();
			return color;
		}
		return "";
	}

	public List<CardEffect> CalCardEffectGroup(cfgcard cfg)
	{
		List<CardEffect> effectgroup = new List<CardEffect>();
		if(cfg != null)
		{
			effectgroup.Add(FormatCardEffect(cfg.effect1));
			effectgroup.Add(FormatCardEffect(cfg.effect2));
			effectgroup.Add(FormatCardEffect(cfg.effect3));
		}
		return effectgroup;
	}

	private CardEffect FormatCardEffect(string effect)
	{
		CardEffect cardeffect = new CardEffect();
		if(effect != "null")
		{
			if(effect.Contains("ji"))
			{
				cardeffect.type = "ji";
			}
			else if(effect.Contains("ki"))
			{
				cardeffect.type = "ki";
			}
			else if(effect.Contains("jyou"))
			{
				cardeffect.type = "jyou";
			}
			cardeffect.des = effect.Replace(cardeffect.type, "");
			if(cardeffect.des.Contains("fall")) cardeffect.des = cardeffect.des.Replace("fall", OTManager.instance.GetOT("CARD_DES_FALL"));
		}
		return cardeffect;
	}

	public string FormatCardDes(cfgcard cfg)
	{
		string carddes = "";
		carddes += OTManager.instance.GetOT("CARD_POWER", cfg.power.ToString()) + "\n";
		carddes += OTManager.instance.GetOT("CARD_GUARD", cfg.guard.ToString()) + "\n";
		carddes += OTManager.instance.GetOT("CARD_COLOR", cfg.color) + "\n";
		carddes += OTManager.instance.GetOT("CARD_NO", cfg.no) + "\n";
		carddes += OTManager.instance.GetOT("CARD_RARE", cfg.rare) + "\n";
		carddes += OTManager.instance.GetOT("CARD_TYPE", cfg.kind) + "\n";
		carddes += OTManager.instance.GetOT("CARD_LEVEL", cfg.level.ToString()) + "\n";
		if(cfg.frame != "null")
		{
			carddes += OTManager.instance.GetOT("CARD_FRAME", cfg.frame) + "\n";
		}
		carddes += OTManager.instance.GetOT("CARD_STRIKE", cfg.strike.ToString()) + "\n";
		carddes += OTManager.instance.GetOT("CARD_FEATURE", cfg.feature);
		return carddes;
	}
}

public class CardEffect
{
	public string type = "";
	public string des = "";
}















