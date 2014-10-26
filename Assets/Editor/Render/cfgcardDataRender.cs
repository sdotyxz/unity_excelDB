using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using Config;
using UnityEditor;
using UnityEngine;
class cfgcardDataRender
{
	static public cfgcardDataEntity Render()
	{
		cfgcardDataEntity entity = AssetUtility.CreateInstance<cfgcardDataEntity> ();
		List<cfgcard> data = new List<cfgcard>();
		RenderExcelData(data);
		entity.data = data;
		return entity;
	}

	private static void RenderExcelData(List<cfgcard> data)
	{
		DataSet datasource = ExcelReader.XLSX("cfgcard.xlsx");
		for(int t = 0; t < datasource.Tables.Count; t++)
		{
			int columnnum = datasource.Tables[t].Columns.Count;
			for(int r = 1; r < datasource.Tables[t].Rows.Count; r++)
			{
				cfgcard info = new cfgcard();
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