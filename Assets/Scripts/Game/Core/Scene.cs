using UnityEngine;
using System.Collections;
using System;

public class Scene : JZWLEngine.Utilities.Scene 
{
	public Camera mCamera;
	public Transform uiLayer;
	public static Scene Instance { get { return instance as Scene; }}

	protected override void _InitLayer ()
	{
		mCamera = Camera.main;
	}
}
