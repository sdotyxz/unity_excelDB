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
}
