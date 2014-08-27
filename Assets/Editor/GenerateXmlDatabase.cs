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

public class GenerateXmlDatabase : MonoBehaviour 
{
	private static DataSet xmldatasource;
	private static string excelName = "";
	private static string filename = "";
	private static string filepath = "";

    private static void GenerateXml(string path)
	{
		if(!File.Exists(path))
		{
			// Create a XMLDoucment
			XmlDocument xmlDoc = new XmlDocument();


			// Create first node - Root
			XmlElement root = xmlDoc.CreateElement("Database");
			XmlComment comment = xmlDoc.CreateComment(excelName);
			XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "");

			root.AppendChild(comment);
			xmlDoc.AppendChild(declaration);
			xmlDoc.AppendChild(root);
			
			// Save XML
			xmlDoc.Save(path);
		}
	}

	private static void FillXml(string path, DataSet dataset)
	{
		if(File.Exists(path))
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(path);
			XmlNode root = xmlDoc.SelectSingleNode("Database");

			int rownum = dataset.Tables[0].Rows.Count;
			int columnnum = dataset.Tables[0].Columns.Count;
			string elementName = excelName;
			string AttributeName = "ID";//dataset.Tables[0].Rows[0][0].ToString();
			string AttributeValue = "";
			for(int i = 1; i < rownum; i++)
			{
				AttributeValue = dataset.Tables[0].Rows[i][0].ToString();
				XmlNodeList nodelist = GetNodesByAttribute(xmlDoc, elementName, AttributeName, AttributeValue);
				if(nodelist.Count == 0)
				{
					// Add a new node
					Debug.Log ("------ Add New Node ------");
					XmlElement elmNew = xmlDoc.CreateElement(elementName);
					elmNew.SetAttribute(AttributeName, AttributeValue);
					for(int j = 0; j < columnnum; j++)
					{
						XmlElement columnElement = xmlDoc.CreateElement(dataset.Tables[0].Rows[0][j].ToString());
						columnElement.InnerText = dataset.Tables[0].Rows[i][j].ToString();
						elmNew.AppendChild(columnElement);
					}
					root.AppendChild(elmNew);
				}
				else
				{
					// Update Node
					Debug.Log ("------ Update Exist Node ------");
					XmlNode targetNode = nodelist[0];
					XmlNodeList targetNodeChild = targetNode.ChildNodes;
					for(int h = 1; h < columnnum; h++)
					{
						XmlElement columnElement = xmlDoc.CreateElement(dataset.Tables[0].Rows[0][h].ToString());
						columnElement.InnerText = dataset.Tables[0].Rows[i][h].ToString();
						XmlNode oldnode =  targetNode.SelectSingleNode(dataset.Tables[0].Rows[0][h].ToString());
						if(oldnode != null)
						{
							targetNode.ReplaceChild(columnElement, oldnode);
						}
						else
						{
							targetNode.AppendChild(columnElement);
						}
					}
				}
			}
			xmlDoc.AppendChild(root);
			xmlDoc.Save(path);
		}
	}

	private static XmlNodeList GetNodesByAttribute(XmlDocument xmlDoc, string elementName, string attributeName, string attributeValue)
	{
		// Formate a node path to search
		string nodepath = "/" + xmlDoc.DocumentElement.Name + "/" + elementName + 
			"[@" + attributeName + "='" + attributeValue + "']";
		return xmlDoc.SelectNodes(nodepath);
	}

	[MenuItem("XMLDatabase/Generate XMLDatabase File")]
	private static void GenerateXMLDatabase()
	{
		Debug.Log ("------ GenerateXMLDatabase ------");
		xmldatasource = ExcelReader.XLSX(Resconfig.RES_EXCEL_FILE_1);
		excelName = ExcelReader.ExcelName(Resconfig.RES_EXCEL_FILE_1);
		string tempName = excelName.Replace("Info", "Data");
		filename = tempName + ".xml";
		filepath = Application.dataPath + Resconfig.RES_XML + filename;
		GenerateXml(filepath);
		FillXml(filepath, xmldatasource);
		AssetDatabase.Refresh();
		Debug.Log ("------ Finish Generate ------");
	}
}
