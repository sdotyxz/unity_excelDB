using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JZWLEngine.Managers;
using Config;

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
}

