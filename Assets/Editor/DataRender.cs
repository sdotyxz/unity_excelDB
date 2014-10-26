using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Config;

public class DataRender : ScriptableObject {
	[MenuItem("ExcelDatabase/Render Data")]
	static void render()
	{
		AssetUtility.Init();
		CardInfoDataEntity cardinfodata = CardInfoDataRender.Render();
		AssetUtility.CreateAsset<CardInfoDataEntity>(cardinfodata);
		cfgcardDataEntity cfgcarddata = cfgcardDataRender.Render();
		AssetUtility.CreateAsset<cfgcardDataEntity>(cfgcarddata);
	}
}