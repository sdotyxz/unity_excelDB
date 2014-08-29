using UnityEngine;
using System.Collections;

public class ManagerTemplate<T> where T : ManagerTemplate<T>, new()
{
	private static T _instance = new T();
	
	public static T instance
	{
		get
		{
			return _instance;
		}
	}
	
	public void Init()
	{
		OnInit();
	}
	
	protected virtual void OnInit() { }
}
