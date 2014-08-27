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
		FillClassFile(filePath, classdatasource);
		AssetDatabase.Refresh();
		Debug.Log ("------ Finish Class Generate------");
	}

	[MenuItem("ExcelDatabase/Generate Database Class")]
	private static void GenerateExcelDatabaseClassFile()
	{
		Debug.Log ("------ GenerateExcelDatabaseClassFile ------");
		// Get Execl Info
		DirectoryInfo excelFolder = new DirectoryInfo(Application.dataPath + Resconfig.RES_EXCEL);
		List<FileInfo> excelfileInfolist = new List<FileInfo>();
		List<DataSet> execldatalist = new List<DataSet>();
		FileInfo[] excelfileInfo = excelFolder.GetFiles();
		foreach(FileInfo file in excelfileInfo)
		{
			if(file.Extension == ".xlsx")
			{
				excelfileInfolist.Add(file);
				string entityfileName = file.Name.Replace(file.Extension, "") + "DataEntity.cs";
				string entityfilePath = Application.dataPath + Resconfig.RES_SCRIPT_CONFIG + entityfileName;
				GenerateClassFile(entityfilePath);
				FillEntityClass(entityfilePath, file.Name.Replace(file.Extension, ""));

				string fileName = file.Name.Replace(file.Extension, "") + ".cs";
				string filePath = Application.dataPath + Resconfig.RES_SCRIPT_CONFIG + fileName;
				GenerateClassFile(filePath);
			}
		}
		AssetDatabase.Refresh();
	}

	private static void GenerateClassFile(string path)
	{
		if(!File.Exists(path))
		{
			FileStream filestream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
			filestream.Close();
		}
	}
	
	private static void FillClassFile(string path, DataSet datasource)
	{
		if(File.Exists(path))
		{
			FileStream filestream = File.Open(path, FileMode.Open);
			StreamWriter stremaWrite = new StreamWriter(filestream);
			string DataClassTpl = getDataClassTpl(datasource);
			stremaWrite.Flush();
			stremaWrite.Write(DataClassTpl);
			stremaWrite.Close();
		}
	}
	

	private static void FillEntityClass(string path, string entityName)
	{
		if(File.Exists(path))
		{
			FileStream filestream = File.Open(path, FileMode.Open);
			StreamWriter stremaWrite = new StreamWriter(filestream);
			string EntityClassTpl = getEntityClassTpl(entityName);
			stremaWrite.Flush();
			stremaWrite.Write(EntityClassTpl);
			stremaWrite.Close();
		}
	}

	private static string getEntityClassTpl(string entityName)
	{
		string entityClassTpl = 
				"using UnityEngine;\n" +
				"using System.IO;\n" +
				"using System.Collections.Generic;\n" +
				"namespace Config\n" +
				"{\n" +
				"  public class " + entityName + "DataEntity : ScriptableObject\n" +
				"  {\n" +
				"    public List<" + entityName + "> data;\n" +
				"  }\n" +
				"}";
		return entityClassTpl;
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
