using UnityEngine;
using System.Collections;
using Config;
using JZWLEngine.Managers;
using PureMVC.Patterns;

public class CardInfoUnit : MonoBehaviour 
{
	public UISprite spColor;
	public UILabel txtNo;
	public UILabel txtName;
	public UILabel txtType;
	public UILabel txtLevel;
	public UILabel txtFrame;
	public UILabel txtPower;
	public UILabel txtGuard;

	private CardInfo mCardInfo;

	private cfgcard _cfg;

	public UIButton btnUnit;

	void Start()
	{
		Pool.GetComponent<UIEventListener>(btnUnit).onClick = OnClickUnit;
	}

	void OnClickUnit (GameObject go)
	{
		if(_cfg != null)
			Facade.Instance.SendNotification(CardInfoNotes.CARDINFO_SHOW_INFO, _cfg);
	}

	public void UpdateCardInfo(cfgcard cfg)
	{
		if(_cfg != cfg)
		{
			_cfg = cfg;
			spColor.spriteName = CardInfoManager.instance.CalCardColorString(_cfg);
			txtNo.text = _cfg.no;
			txtName.text = _cfg.name; 
			string frame = _cfg.frame;
			txtFrame.text = frame != "null" ? frame : "";
			txtType.text = _cfg.kind;
			txtLevel.text = _cfg.level.ToString();
			txtPower.text = _cfg.power.ToString();
			txtGuard.text = _cfg.guard.ToString();
		}
	}
}
