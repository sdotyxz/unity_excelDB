#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;


public class AssetUtility
{
	public static void CreateAsset<T>(T obj)
	{
		string path = "Assets/Resources/" + typeof(T).ToString() + ".asset";
		//		AssetDatabase.DeleteAsset(
		//		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path);
		
		AssetDatabase.CreateAsset(obj as Object, path);
		
		Object assetObj = AssetDatabase.LoadAssetAtPath(path, typeof(T));
		//AssetDatabase.SaveAssets();
		Debug.LogError("------CreateAsset------  :" + path);
		assetList.Add(assetObj);
	}
	public static void CreateAsset(Object obj, string typeName)
	{
		string path = "Assets/Resources/" + typeName.ToString() + ".asset";
		AssetDatabase.CreateAsset(obj as Object, path);
		AssetDatabase.SaveAssets();
	}
	public static void CreateAsset(Object obj, string typeName,string subPath)
	{
		//string path = "Assets/Resources/"+subPath+"/" + typeName + ".asset";
		//AssetDatabase.DeleteAsset(path);
		
		//AssetDatabase.CreateAsset(obj, path);
		//AssetDatabase.SaveAssets();
	}
	
	private static List<Object> assetList = null;
	
	public static void Init()
	{
		if(assetList != null)
		{
			assetList = null;
		}
		assetList = new List<Object>();
	}
	
//	public static void Flush()
//	{
//		Debug.LogError ("--------------------");
//		string dirPath = "Assets/Resources/";
//		//string dirPath = EditorUtility.SaveFolderPanel("资源导出","","");
//		ImportMapMaskTool.CreateMapMask();
//		
//		string pc_path = dirPath + "pc/TBL.assertbundle.zip";
//		if (File.Exists(pc_path))
//		{
//			Debug.LogError("------delete-pc-path-------- :" + pc_path);
//			File.Delete(pc_path);
//		}
//		
//		if (BuildPipeline.BuildAssetBundle(null, assetList.ToArray(), pc_path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.StandaloneWindows))
//		{
//			AssetDatabase.Refresh();
//		}
//		else
//		{
//			Debug.LogError("pc assertbundle create fail");
//		}
//		
//		string android_path = dirPath + "android/TBL.assertbundle.zip";
//		if (File.Exists(android_path))
//		{
//			Debug.LogError("------delete-android-path-------- :" + android_path);
//			File.Delete(android_path);
//		}
//		
//		if (BuildPipeline.BuildAssetBundle(null, assetList.ToArray(), android_path, BuildAssetBundleOptions.CollectDependencies, BuildTarget.Android))
//		{
//			AssetDatabase.Refresh();
//		}
//		else
//		{
//			Debug.LogError("android assertbundle create fail");
//		}
//	}
	
	public static T CreateInstance<T>() where T : ScriptableObject
	{
		return ScriptableObject.CreateInstance<T>();
	}
}
#endif