using UnityEngine;
using System.Collections;

public class CardEffectUnit : MonoBehaviour 
{
	public UISprite spEffectDes;
	public UILabel txtEffectDes;

	public void UpdateEffect(string effect)
	{
		string[] effectdes = effect.Split('#');
		string effecttext = effectdes[1];
		spEffectDes.spriteName = "EF_" + effectdes[0];
		txtEffectDes.text = effecttext;
	}

	public void UpdateEffect(CardEffect effect)
	{
		spEffectDes.spriteName = "icon_" + effect.type;
		spEffectDes.MakePixelPerfect();
		txtEffectDes.text = effect.des;
	}
}
