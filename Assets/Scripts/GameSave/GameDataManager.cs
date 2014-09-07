using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using Config;

// Class which used to save game data
public class GameData
{
	// The Saving Key, which used to prevent save data copy
	public string key;

	public string PlayerName;
	public float MusicVolume;

	public List<CardGroup> CardGroupList;

	public GameData()
	{
		PlayerName = "Player";
		MusicVolume = 0.6f;
		CardGroupList = new List<CardGroup>();
	}
}

public class GameDataManager : MonoBehaviour 
{
	private string dataFileName = "MyCardData.dat";
	private XmlSaver xs = new XmlSaver();

	public GameData gameData;

	public void Awake()
	{
		gameData = new GameData();

		gameData.key = SystemInfo.deviceUniqueIdentifier;
		Load();
	}

	// Save GameData Method
	public void Save()
	{
		string gameDataFile = GetDataPath() + "/" + dataFileName;
		string dataString = xs.SerializeObject(gameData, typeof(GameData));
		xs.CreateXML(gameDataFile, dataString);
	}
	
	// Load GameData Method
	public void Load()
	{
		string gameDataFile = GetDataPath() + "/" + dataFileName;
		if(xs.HasFile(gameDataFile))
		{
			string dataString = xs.LoadXML(gameDataFile);
			GameData gameDataFromXML = xs.DeserializedObject(dataString, typeof(GameData)) as GameData;

			// Is Legal Save Data
			if(gameDataFromXML.key == gameData.key)
			{
				gameData = gameDataFromXML;
			}
			else // Illegal Sava data
			{
				// illegal save data will be cover
			}
		}
		else
		{
			if(gameData != null) Save();
		}
	}

	// Get Save Data Path
	private static string GetDataPath()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
			path = path.Substring(0, path.LastIndexOf('/'));
			return path + "/Documents";
		}
		else
			return Application.dataPath;
	}
}
