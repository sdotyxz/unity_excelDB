using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JZWLEngine.Managers;
using JZWLEngine.Managers.Helper;
using JZWLEngine.Loader;

public class InitializeModule :  BaseModule
{
	public InitializeModule() : base()
	{
	}

	protected override void _Start ()
	{
		// Do something - such as connect to server
//		EventManager.instance.AddEventListener(VersionManager.instance, VersionManager.VERSION_PREPARE_COMPLETE, _OnVersionPrepareComplete, false, true);
//		VersionManager.instance.Init();

		GameManagerInit();
		ModuleManager.instance.GotoModule(typeof(CardInfoModule));
	}

	private void _OnVersionPrepareComplete(params object[] args)
	{
		AssetsLoader.Loads(VersionManager.instance.versionConfig, _OnAssetLoadComplete);
	}

	void _OnAssetLoadComplete ()
	{

	}

	private void GameManagerInit()
	{
		CardInfoManager.instance.Init();
	}

	protected override void _Dispose ()
	{

	}
}
