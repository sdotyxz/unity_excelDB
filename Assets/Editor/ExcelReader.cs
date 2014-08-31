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

public class ExcelReader 
{
	public static DataSet XLSX(string filename)
	{
		FileStream stream = File.Open(Application.dataPath + Resconfig.RES_EXCEL + filename, FileMode.Open);
		IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
		return excelReader.AsDataSet();
	}

	public static DataSet ReadExcel(string classname)
	{
		string filename = classname + ".xlsx";
		return XLSX(filename);
	}

	public static string ExcelName(string filename)
	{
		string[] words = filename.Split('.');
		return words[0];
	}
}
