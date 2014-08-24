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
using UnityEngine;
using System.Collections.Generic;


public class ReadAndBuild : MonoBehaviour 
{
	private DataSet exceldataset;
	string fileName = "";
	string pathstring = "";
	
	// Use this for initialization
	void Start () {
		XLSX();
		//BuildCSharp();
//		GenerateXml();
//		UpdateXml();
//		AddXml();
		deleteXml();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void XLSX()
	{
		FileStream stream = File.Open(Application.dataPath + "/AVCARDDB.xlsx", FileMode.Open, FileAccess.Read);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		
		exceldataset = excelReader.AsDataSet();
		fileName = exceldataset.Tables[0].TableName + ".xml";
		pathstring = Path.Combine(Application.dataPath, fileName);
	}

	void BuildCSharp()
	{
		if(exceldataset != null)
		{
			FileStream filestream;
			if(!File.Exists(pathstring))
			{
				filestream = new FileStream(pathstring, FileMode.CreateNew);
			}
			else
			{
				filestream = File.Open(pathstring, FileMode.Open);
			}

			StreamWriter stremaWrite = new StreamWriter(filestream);
			string classtring = 
					"using UnityEngine;\n" +
					"using System.Collections;\n\n" +
					"public class " + exceldataset.Tables[0].TableName + "{\n" +
					workoutclassProperty(exceldataset) + "}";
			stremaWrite.Flush();
			stremaWrite.Write(classtring);
			stremaWrite.Close();
		}
	}

	public void GenerateXml()
	{
		if(!File.Exists(pathstring))
		{
			// Create a XMLDoucment
			XmlDocument xmlDoc = new XmlDocument();

			// Create first node - Root
			XmlElement root = xmlDoc.CreateElement("transforms");

			// Create second node
			XmlElement elmNew = xmlDoc.CreateElement("rotation");

			// Seting node's two propertys ID and Name
			elmNew.SetAttribute("id", "0");
			elmNew.SetAttribute("name", "mono");

			// Create third node and give value
			XmlElement rotation_X = xmlDoc.CreateElement("x");
			rotation_X.InnerText = "0";

			XmlElement rotation_Y = xmlDoc.CreateElement("y");
			rotation_Y.InnerText = "1";

			XmlElement rotation_Z = xmlDoc.CreateElement("z");
			rotation_Z.InnerText = "z";

			// Set an node attribute
			rotation_Z.SetAttribute("id", "1");

			// Add node to XMLDoc
			elmNew.AppendChild(rotation_X);
			elmNew.AppendChild(rotation_Y);
			elmNew.AppendChild(rotation_Z);

			root.AppendChild(elmNew);
			xmlDoc.AppendChild(root);

			// Save XML
			xmlDoc.Save(pathstring);
		}
	}

	public void UpdateXml()
	{
		if(File.Exists(pathstring))
		{
			XmlDocument xmlDoc = new XmlDocument();

			// Load XML data
			xmlDoc.Load(pathstring);

			// Get All Child node of transform node
			XmlNodeList nodeList = xmlDoc.SelectSingleNode("transforms").ChildNodes;

			foreach(XmlElement xe in nodeList)
			{
				if(xe.GetAttribute("id") == "0")
				{
					xe.SetAttribute("id", "1000");

					foreach(XmlElement x1 in xe.ChildNodes)
					{
						if(x1.Name == "z")
						{
							x1.InnerText = "update00000";
						}
					}
					break;
				}
			}
			xmlDoc.Save(pathstring);
		}
	}

	public void AddXml()
	{
		if(File.Exists(pathstring))
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(pathstring);
			XmlNode root = xmlDoc.SelectSingleNode("transforms");
			XmlElement elmNew = xmlDoc.CreateElement("rotation");
			elmNew.SetAttribute("id", "1");
			elmNew.SetAttribute("name", "yusong");

			XmlElement rotation_X = xmlDoc.CreateElement("x");
			rotation_X.InnerText = "0";
			rotation_X.SetAttribute("id", "1");
			XmlElement rotation_Y = xmlDoc.CreateElement("y");
			rotation_Y.InnerText = "1";
			XmlElement rotation_Z = xmlDoc.CreateElement("z");
			rotation_Z.InnerText = "2";

			elmNew.AppendChild(rotation_X);
			elmNew.AppendChild(rotation_Y);
			elmNew.AppendChild(rotation_Z);
			root.AppendChild(elmNew);
			xmlDoc.AppendChild(root);
			xmlDoc.Save(pathstring);
		}
	}

	public void deleteXml()
	{
		if(File.Exists(pathstring))
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(pathstring);
			XmlNodeList nodelist = xmlDoc.SelectSingleNode("transforms").ChildNodes;
			foreach(XmlElement xe in nodelist)
			{
				if(xe.GetAttribute("id") == "1")
				{
					xe.RemoveAttribute("id");
				}

				foreach(XmlElement x1 in xe.ChildNodes)
				{
					if(x1.Name == "z")
					{
						x1.RemoveAll();
					}
				}
			}
			xmlDoc.Save(pathstring);
		}
	}

	string workoutclassProperty(DataSet dataset)
	{
		int columns = dataset.Tables[0].Columns.Count;
		string classPorperty = "";
		for(int i = 0; i < columns; i++)
		{
			var columndata= dataset.Tables[0].Rows[1][i];
			string datatype = columndata.GetType().ToString();
			switch(datatype)
			{
				case "System.String":
				classPorperty += "    public string " + dataset.Tables[0].Rows[0][i].ToString() + ";\n";
					break;
				case "System.Double":
				classPorperty += "    public int " + dataset.Tables[0].Rows[0][i].ToString() + ";\n";
					break;
			}
		}
		return classPorperty;
	}
}















