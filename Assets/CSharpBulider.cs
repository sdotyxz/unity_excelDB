using UnityEngine;
using System.IO;
using System.Collections;

public class CSharpBulider : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		BuildCSharp();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void BuildCSharp()
	{
		string fileName = "FileName.cs";	
		string pathstring = System.IO.Path.Combine(Application.dataPath, fileName);

		Debug.Log("Path to my file" + pathstring);

//		if(!System.IO.File.Exists(pathstring))
//		{
//			using(System.IO.FileStream fs = System.IO.File.Create(pathstring))
//			{
//				for(byte i = 0; i < 100; i ++)
//				{
//					fs.WriteByte(i);
//				}
//			}
//		}

		FileStream filestream = new FileStream(pathstring, FileMode.CreateNew);

		StreamWriter streamWrite = new StreamWriter(filestream);
		string teststring = "using UnityEngine;\nusing System.Collections;\npublic class Test2 { \n " +
			"// Use this for initialization \n " +
			"    void Start () { \n}\n " +
			"// Update is called once per frame \n" +
			"    void Update () { \n}\n" + "}";
		streamWrite.Write(teststring);
		streamWrite.Close();
	}
}
