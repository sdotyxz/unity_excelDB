using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System;

public class XmlSaver
{
	// Content Encrypt
	public string Encrypt(string toE)
	{
		// Encrypt and Decrypt use the same key, should be 32-bits
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578902223367877723456789012");
		RijndaelManaged rDel = new RijndaelManaged();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		rDel.Padding = PaddingMode.PKCS7;
		ICryptoTransform cTransform = rDel.CreateEncryptor();

		byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toE);
		byte[] resultArray = 
			cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

		return Convert.ToBase64String(resultArray, 0, resultArray.Length);
	}

	// Content Dencrypt
	public string Decrypt(string toD)
	{
		byte[] keyArray = UTF8Encoding.UTF8.GetBytes("12348578902223367877723456789012");

		RijndaelManaged rDel = new RijndaelManaged();
		rDel.Key = keyArray;
		rDel.Mode = CipherMode.ECB;
		rDel.Padding = PaddingMode.PKCS7;
		ICryptoTransform cTransform = rDel.CreateDecryptor();

		byte[] toEncryptArray = Convert.FromBase64String(toD);
		byte[] resultArray = 
			cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

		return UTF8Encoding.UTF8.GetString(resultArray);
	}

	public string SerializeObject(object pObject, System.Type ty)
	{
		string XmlizedString = null;
		MemoryStream memoryStream = new MemoryStream();
		XmlSerializer xs= new XmlSerializer(ty);
		XmlTextWriter xmlTextWrite = new XmlTextWriter(memoryStream, Encoding.UTF8);
		xs.Serialize(xmlTextWrite, pObject);
		memoryStream = (MemoryStream)xmlTextWrite.BaseStream;
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
		return XmlizedString;
	}

	public object DeserializedObject(string pXmlizedString, System.Type ty)
	{
		XmlSerializer xs = new XmlSerializer(ty);
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		return xs.Deserialize(memoryStream);
	}

	// Create XML File
	public void CreateXML(string fileName, string thisData)
	{
		string xxx = Encrypt(thisData);
		StreamWriter writer;
		writer = File.CreateText(fileName);
		writer.Write(xxx);
		writer.Close();
	}

	// Load XML File
	public string LoadXML(string fileName)
	{
		StreamReader sReader = File.OpenText(fileName);
		string dataString = sReader.ReadToEnd();
		sReader.Close();
		string xxx = Decrypt(dataString);
		return xxx;
	}

	// Check File Exists
	public bool HasFile(string fileName)
	{
		return File.Exists(fileName);
	}

	public String UTF8ByteArrayToString(byte[] characters)
	{
		UTF8Encoding encoding = new UTF8Encoding();
		string constructedString = encoding.GetString(characters);
		return (constructedString);
	}

	public byte[] StringToUTF8ByteArray(string pXmlString)
	{
		UTF8Encoding encoding = new UTF8Encoding();
		byte[] byteArray = encoding.GetBytes(pXmlString);
		return byteArray;
	}
}





















