using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using JZWLEngine.Managers;

public class Engine : JZWLEngine.JZWLBaseEngine
{
	public bool IsDebug;

	// Use this for initialization
	void Start () 
	{
		SystemConfigSetting();
		EngineConfigSetting();
		GameInit();
	}

	private void SystemConfigSetting()
	{
		Application.targetFrameRate = 24;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Input.simulateMouseWithTouches = true;
	}

	private void EngineConfigSetting()
	{
		// Do something to config the engine
	}

	private void GameInit()
	{
		// OnScreen Text Manager For Multiple Language Support
		OTManager.instance.AddOT((Resources.Load("ot") as TextAsset).text);

		// EventManager provided by JZWLEngine
		EventManager.instance.Init();

		ModuleManager.instance.GotoModule(typeof(InitializeModule));
	}
}
