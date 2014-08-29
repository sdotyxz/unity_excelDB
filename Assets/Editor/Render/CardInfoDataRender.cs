using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Config;
using UnityEditor;
using UnityEngine;
class CardInfoDataRender
{
	static public CardInfoDataEntity Render()
	{
		CardInfoDataEntity entity = AssetUtility.CreateInstance<CardInfoDataEntity> ();
		List<CardInfo> data = new List<CardInfo>();
		RenderExcelData(data);
		entity.data = data;
		return entity;
	}

	private static void RenderExcelData(List<CardInfo> data)
	{
		DataSet datasource = ExcelReader.XLSX("CardInfo.xlsx");
		for(int t = 0; t < datasource.Tables.Count; t++)
		{
			int columnnum = datasource.Tables[t].Columns.Count;
			for(int r = 1; r < datasource.Tables[t].Rows.Count; r++)
			{
				CardInfo info = new CardInfo();
				for(int c = 0; c < columnnum; c++)
				{
					FieldInfo field = info.GetType().GetField(datasource.Tables[t].Rows[0][c].ToString());
					Type type = field.FieldType;
					object columnvalue = Convert.ChangeType(datasource.Tables[t].Rows[r][c], type);
					field.SetValue(info, columnvalue);
				}
				data.Add(info);
			}
		}
	}
}