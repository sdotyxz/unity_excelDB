using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Config;
using JZWLEngine.Managers;

public class CardDesc : MonoBehaviour 
{
	public GameObject effectTemplate;
	public UITexture texCardImage;
	public UILabel txtCardName;
	public UILabel txtCardDes;
	public UITable tableEffect;
	public UILabel txtDes;
	private List<GameObject> effectlist = new List<GameObject>();

	private cfgcard _desCfg;
	public cfgcard desCfg { get { return _desCfg; } }

	public void UpdateCardDes(cfgcard cfg)
	{
		if(_desCfg != cfg)
		{
			_desCfg = cfg;
			txtCardName.text = _desCfg.name;
			string[] namepre = _desCfg.no.Split('-');
			string impagepath = Resconfig.RES_CARD_IMAGE + namepre[0] + "/" + _desCfg.img;
			Texture2D tex = Resources.Load(impagepath) as Texture2D;
			texCardImage.mainTexture = tex;
			if(_desCfg.desc != "null") txtDes.text = _desCfg.desc;
			ShowCardEffect(CardInfoManager.instance.CalCardEffectGroup(_desCfg));
			txtCardDes.text = CardInfoManager.instance.FormatCardDes(_desCfg);
		}
	}

	private void ShowCardEffect(List<CardEffect> effectgroup)
	{
		foreach(GameObject eu in effectlist)
		{
			Destroy(eu);
		}
		effectlist = new List<GameObject>();
		int count = 0;
		foreach(CardEffect effect in effectgroup)
		{
			if(effect.type != "")
			{
				GameObject go = NGUITools.AddChild(tableEffect.gameObject, effectTemplate);
				effectlist.Add(go);
				go.name = "effect" + count.ToString();
				CardEffectUnit efu = go.GetComponent<CardEffectUnit>();
				efu.UpdateEffect(effect);
				count ++;
			}
		}
		StartCoroutine(DelayRepos());
	}

	IEnumerator DelayRepos()
	{
		yield return new WaitForEndOfFrame();
		tableEffect.Reposition();
	}
}
















