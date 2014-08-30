using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JZWLEngine.Managers;
using Config;
using System.Linq;

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
}

