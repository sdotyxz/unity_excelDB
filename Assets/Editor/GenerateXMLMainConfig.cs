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
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GenerateXMLMainConfig : MonoBehaviour 
{
	[MenuItem("XMLDatabase/Generate XML Main Config File")]
	private static void GenerateXMLMainConfigFile()
	{
		Debug.Log ("------ GenerateXMLMainConfig ------");

		// Get Execl Info
		DirectoryInfo excelFolder = new DirectoryInfo(Application.dataPath + Resconfig.RES_EXCEL);
		List<FileInfo> excelfileInfolist = new List<FileInfo>();//;
		List<DataSet> execldatalist = new List<DataSet>();
		FileInfo[] excelfileInfo = excelFolder.GetFiles();
		foreach(FileInfo file in excelfileInfo)
		{
			if(file.Extension == ".xlsx")
			{
				excelfileInfolist.Add(file);
			}
		}

		// Get XML Info
		DirectoryInfo xmlFolder = new DirectoryInfo(Application.dataPath + Resconfig.RES_XML);
		List<FileInfo> xmlfileInfoList = new List<FileInfo>();
		FileInfo[] xmlfileInfo = xmlFolder.GetFiles();
		foreach(FileInfo file in xmlfileInfo)
		{
			if(file.Extension == ".xml")
			{
				xmlfileInfoList.Add(file);
			}
		}
		string filename = "MainConfig.xml";
		string filepath = Application.dataPath + Resconfig.RES_XML + filename;
		GenerateXml(filepath);
		FillExcelFile(excelfileInfolist, filepath);
		FillDefineFile(xmlfileInfoList, filepath);
		AssetDatabase.Refresh();
		Debug.Log ("------ Finish Generate ------");
	}

	private static void GenerateXml(string path)
	{
		if(!File.Exists(path))
		{
			// Create a XMLDoucment
			XmlDocument xmlDoc = new XmlDocument();

			// Create first node - Root
			XmlElement root = xmlDoc.CreateElement("root");
			XmlComment comment = xmlDoc.CreateComment("Excel Source and define");
			XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "");
			
			root.AppendChild(comment);
			xmlDoc.AppendChild(declaration);
			xmlDoc.AppendChild(root);
			
			// Save XML
			xmlDoc.Save(path);
		}
	}

	private static void FillDefineFile(List<FileInfo> filelist, string path)
	{
		if(File.Exists(path))
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(path);
			XmlNode defineroot = xmlDoc.SelectSingleNode("root/defines");
			if(defineroot == null)
			{
				defineroot = xmlDoc.CreateElement("defines");
				XmlNode root = xmlDoc.DocumentElement;
				root.AppendChild(defineroot);
			}
			foreach(FileInfo file in filelist)
			{
				XmlElement defineelm = xmlDoc.CreateElement("define");
				XmlNodeList nodelist = GetNodesByAttribute(xmlDoc, "defines", "define", "file", file.Name);
				if(nodelist.Count == 0)
				{
					// Add a new node
					defineelm.SetAttribute("file", file.Name);
					defineroot.AppendChild(defineelm);
				}
				else
				{
					// Update exist node
					XmlElement targetelm = nodelist[0] as XmlElement;
					targetelm.SetAttribute("file", file.Name);
				}
			}

			xmlDoc.Save(path);
		}
	}

	private static void FillExcelFile(List<FileInfo> filelist, string path)
	{
		if(File.Exists(path))
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(path);
			XmlNode excelroot = xmlDoc.SelectSingleNode("root/excels");
			if(excelroot == null)
			{
				excelroot = xmlDoc.CreateElement("excels");
				XmlNode root =xmlDoc.DocumentElement;
				root.AppendChild(excelroot);
			}
			foreach(FileInfo file in filelist)
			{
				XmlElement excelelm = xmlDoc.CreateElement("excel");
				string filename = file.Name.Replace(file.Extension, "");
				XmlNodeList nodelist = GetNodesByAttribute(xmlDoc, "excels" ,"excel", "name", filename);
				if(nodelist.Count == 0)
				{
					// Add a new node
					excelelm.SetAttribute("name", filename);
					excelelm.SetAttribute("excel_file", file.Name);
					excelroot.AppendChild(excelelm);
				}
				else
				{
					// Update an exist node
					XmlElement targetelm = nodelist[0] as XmlElement;
					targetelm.SetAttribute("excel_file", file.Name);
				}
			}

			xmlDoc.Save(path);
		}
	}

	private static XmlNodeList GetNodesByAttribute(XmlDocument xmlDoc, string elementroot, string elementName, string attributeName, string attributeValue)
	{
		// Formate a node path to search
		string nodepath = "/" + xmlDoc.DocumentElement.Name + "/" + elementroot + "/" + elementName + 
			"[@" + attributeName + "='" + attributeValue + "']";
		return xmlDoc.SelectNodes(nodepath);
	}
}
