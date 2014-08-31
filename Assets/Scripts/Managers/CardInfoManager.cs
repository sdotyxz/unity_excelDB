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
	protected override void OnInit ()
	{
		Debug.Log ("------ CardInfoManager ------");
		_cardinfoEntity = Resources.Load(Resconfig.CONFIG_CARDINFO) as CardInfoDataEntity;
		if(_cardinfoEntity == null)
		{
			Debug.LogError("Exception: Lost CardInfo Entity !!");
		}
	}

	public List<CardInfo> GetAllCardInfo()
	{
		return _cardinfoEntity.data;
	}

	public CardInfo GetCardInfoByName(string name)
	{
		return _cardinfoEntity.data.Find(e => e.CardName.Contains(name));
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


}

