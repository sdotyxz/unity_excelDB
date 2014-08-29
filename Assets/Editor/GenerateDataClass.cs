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
		List<string> classnamelist = new List<string>();
		foreach(FileInfo file in excelfileInfo)
		{
			if(file.Extension == ".xlsx")
			{
				DataSet datasource = ExcelReader.XLSX(file.Name);
				excelfileInfolist.Add(file);
				string entityfileName = file.Name.Replace(file.Extension, "") + "DataEntity.cs";
				string entityfilePath = Application.dataPath + Resconfig.RES_SCRIPT_CONFIG + entityfileName;
				GenerateClassFile(entityfilePath);
				FillEntityClass(entityfilePath, file.Name.Replace(file.Extension, ""));

				string fileName = file.Name.Replace(file.Extension, "") + ".cs";
				string filePath = Application.dataPath + Resconfig.RES_SCRIPT_CONFIG + fileName;
				GenerateClassFile(filePath);
				FillDataClass(filePath, datasource ,file.Name.Replace(file.Extension, ""));

				string renderfileName = file.Name.Replace(file.Extension, "") + "DataRender.cs";
				string renderfilePath = Application.dataPath +Resconfig.RES_EDITOR_RENDER + renderfileName;
				GenerateClassFile(renderfilePath);
				FillRenderClass(renderfilePath, datasource, file.Name.Replace(file.Extension, ""));

				classnamelist.Add(file.Name.Replace(file.Extension, ""));
			}
		}

		string DataRenderFilePath = Application.dataPath + Resconfig.RES_EDITOR + "DataRender.cs";
		GenerateClassFile(DataRenderFilePath);
		FillDataRender(DataRenderFilePath, classnamelist);
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

	private static void FillDataRender(string path, List<string> classnamelist)
	{
		if(File.Exists(path))
		{
			FileStream filestream = File.Open(path, FileMode.Open);
			StreamWriter streamWriter = new StreamWriter(filestream);
			string DataRenderTpl = getDataRenderTpl(classnamelist);
			streamWriter.Flush();
			streamWriter.Write(DataRenderTpl);
			streamWriter.Close();
		}
	}

	private static string getDataRenderTpl(List<string> classnamelist)
	{
		string dataRenderTpl = 
				"using UnityEngine;\n" +
				"using UnityEditor;\n" +
				"using System.IO;\n" +
				"using System.Collections;\n" +
				"using System.Collections.Generic;\n" +
				"using Config;\n" +
				"\n" +
				"public class DataRender : ScriptableObject {\n" +
				"\t[MenuItem(\"ExcelDatabase/Render Data\")]\n" +
				"\tstatic void render()\n" +
				"\t{\n" +
				"\t\tAssetUtility.Init();\n" + getDataRenderQueue(classnamelist) + 
				"\t}\n" +
				"}";
		return dataRenderTpl;
	}

	private static string getDataRenderQueue(List<string> classnamelist)
	{
		string dataRenderQueue = "";
		foreach(string classname in classnamelist)
		{
			dataRenderQueue += 
				"\t\t" + classname + "DataEntity " + classname.ToLower() + "data = " + classname + "DataRender.Render();\n" +
				"\t\tAssetUtility.CreateAsset<" + classname + "DataEntity>(" + classname.ToLower() + "data);\n"; 
		}
		return dataRenderQueue;
	}

	private static void FillRenderClass(string path, DataSet datasource, string className)
	{
		if(File.Exists(path))
		{
			FileStream filestream = File.Open(path, FileMode.Open);
			StreamWriter streamWriter = new StreamWriter(filestream);
			string RenderClassTpl = getRenderClassTpl(datasource, className);
			streamWriter.Flush();
			streamWriter.Write(RenderClassTpl);
			streamWriter.Close();
		}
	}

	private static string getRenderClassTpl(DataSet datasource, string className)
	{
		string renderClassTpl = 
				"using System;\n" +
				"using System.Collections.Generic;\n" +
				"using System.Data;\n" + 
				"using System.IO;\n" +
				"using System.Reflection;\n" +
				"using Config;\n" +
				"using UnityEditor;\n" +
				"using UnityEngine;\n" +
				"class " + className + "DataRender\n" +
				"{\n" +
				"\tstatic public " + className + "DataEntity Render()\n" +
				"\t{\n" +
				"\t\t" + className + "DataEntity entity = AssetUtility.CreateInstance<" + className + "DataEntity> ();\n" +
				"\t\tList<" + className + "> data = new List<" + className + ">();\n" +
				"\t\tRenderExcelData(data);\n" +
				"\t\tentity.data = data;\n" +
				"\t\treturn entity;\n" +
				"\t}\n" +
				"\n" +
				"\tprivate static void RenderExcelData(List<" + className + "> data)\n" +
				"\t{\n" +
				"\t\tDataSet datasource = ExcelReader.XLSX(\"" + className + ".xlsx\");\n" +
				"\t\tfor(int t = 0; t < datasource.Tables.Count; t++)\n" +
				"\t\t{\n" +
				"\t\t\tint columnnum = datasource.Tables[t].Columns.Count;\n" +
				"\t\t\tfor(int r = 1; r < datasource.Tables[t].Rows.Count; r++)\n" +
				"\t\t\t{\n" +
				"\t\t\t\t" + className + " info = new " + className + "();\n" +
				"\t\t\t\tfor(int c = 0; c < columnnum; c++)\n" +
				"\t\t\t\t{\n" +
				"\t\t\t\t\tFieldInfo field = info.GetType().GetField(datasource.Tables[t].Rows[0][c].ToString());\n" +
				"\t\t\t\t\tType type = field.FieldType;\n" +
				"\t\t\t\t\tobject columnvalue = Convert.ChangeType(datasource.Tables[t].Rows[r][c], type);\n" +
				"\t\t\t\t\tfield.SetValue(info, columnvalue);\n" +
				"\t\t\t\t}\n" +
				"\t\t\t\tdata.Add(info);\n" +
				"\t\t\t}\n" +
				"\t\t}\n" +
				"\t}\n" +
				"}"; 
		return renderClassTpl;
	}

	private static void FillDataClass(string path, DataSet datasource, string className)
	{
		if(File.Exists(path))
		{
			FileStream filestream = File.Open(path, FileMode.Open);
			StreamWriter streamWriter = new StreamWriter(filestream);
			string DataClassTpl = getDataClassTpl(datasource, className);
			streamWriter.Flush();
			streamWriter.Write(DataClassTpl);
			streamWriter.Close();
		}
	}

	private static string getDataClassTpl(DataSet datasource, string className)
	{
		string dataClassTpl = 
				"using UnityEngine;\n" +
				"using System.IO;\n" +
				"using System.Collections.Generic;\n" +
				"namespace Config\n" +
				"{\n" +
				"\t[System.Serializable]\n" +
				"\tpublic class " + className + "\n" +
				"\t{\n" + getDataClassProperty(datasource) +
				"\t}\n" +
				"}";
		return dataClassTpl;
	}

	private static string getDataClassProperty(DataSet dataSource)
	{
		int columns = dataSource.Tables[0].Columns.Count;
		string classPorperty = "";
		for(int i = 0; i < columns; i++)
		{
			var columndata= dataSource.Tables[0].Rows[1][i];
			string datatype = columndata.GetType().ToString();
			switch(datatype)
			{
			case "System.String":
				classPorperty += "\t\tpublic string " + dataSource.Tables[0].Rows[0][i].ToString() + ";\n";
				break;
			case "System.Double":
				classPorperty += "\t\tpublic int " + dataSource.Tables[0].Rows[0][i].ToString() + ";\n";
				break;
			}
		}
		return classPorperty;
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
