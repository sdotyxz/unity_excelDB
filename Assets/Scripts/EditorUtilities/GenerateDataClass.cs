using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Excel;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class GenerateDataClass : MonoBehaviour 
{
	private static DataSet classdatasource;
	private static string className;
	private static string fileName;
	private static string filePath;

	[MenuItem("XMLDatabase/Generate Database Class")]
	private static void GenerateDatabaseClassFile()
	{
		Debug.Log ("------ GenerateDatabaseClassFile ------");
		classdatasource = ExcelReader.XLSX(Resconfig.RES_EXCEL_FILE_1);
		className = ExcelReader.ExcelName(Resconfig.RES_EXCEL_FILE_1);
		fileName = className + ".cs";
		filePath = Application.dataPath + Resconfig.RES_DATA_SCRIPT + fileName;
		GenerateClassFile(filePath);
		FillClassFile(filePath);
		AssetDatabase.Refresh();
		Debug.Log ("------ Finish Class Generate------");
	}

	private static void GenerateClassFile(string path)
	{
		if(!File.Exists(path))
		{
			FileStream filestream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
			filestream.Close();
		}
	}
	
	private static void FillClassFile(string path)
	{
		if(File.Exists(path))
		{
			FileStream filestream = File.Open(path, FileMode.Open);
			StreamWriter stremaWrite = new StreamWriter(filestream);
			string DataClassTpl = getDataClassTpl(classdatasource);
			stremaWrite.Flush();
			stremaWrite.Write(DataClassTpl);
			stremaWrite.Close();
		}
	}

	private static string getDataClassTpl(DataSet datasource)
	{
		string dataClassTpl = 
				"using UnityEngine;\n" +
				"using System.Collections;\n" +
				"using System.Xml;\n" +
				"using System.Xml.Serialization;\n" +
				"\n" +
				"[XmlRoot]\n" +
				"public sealed class " + className + " : DatabaseEntry\n" +
				"{\n" +
				getClassProperty(datasource) +
				"}";
		return dataClassTpl;
	}

	private static string getClassProperty(DataSet dataSource)
	{
		int columns = dataSource.Tables[0].Columns.Count;
		string classPorperty = "";
		for(int i = 0; i < columns; i++)
		{
			var columndata= dataSource.Tables[0].Rows[1][i];
			string datatype = columndata.GetType().ToString();
			classPorperty += "    [XmlElement]\n";
			switch(datatype)
			{
			case "System.String":
				classPorperty += "    public string " + dataSource.Tables[0].Rows[0][i].ToString() + " { get; private set; }\n";
				break;
			case "System.Double":
				classPorperty += "    public int " + dataSource.Tables[0].Rows[0][i].ToString() + " { get; private set; }\n";
				break;
			}
		}
		return classPorperty;
	}
}
