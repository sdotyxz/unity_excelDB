using UnityEditor;
using UnityEngine;
using System.IO;

public class ModuleCreator : EditorWindow
{
    private const string lblDefaultModuleName = "模块名称";
    string defaultModuleName = "";
    string addNoteButtonName = "创建";
    string note1 = "";
    string note2 = "";
    string note3 = "";
    string note4 = "";
    string note5 = "";
    string note6 = "";
    string note7 = "";
    string note8 = "";
    string note9 = "";


    [MenuItem("---->ModuleCreator<----/Open ModuleCreator")]
    static void OpenMapEditor()
    {
        ModuleCreator window = (ModuleCreator)EditorWindow.GetWindow(typeof(ModuleCreator));
        window.Show();
    }

    void OnGUI()
    {
        defaultModuleName = EditorGUILayout.TextField(lblDefaultModuleName, defaultModuleName);
        
        GUILayout.BeginHorizontal();
        note1 = EditorGUILayout.TextField("note1", note1);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        note2 =EditorGUILayout.TextField("note2", note2);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        note3 =EditorGUILayout.TextField("note3", note3);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        note4 = EditorGUILayout.TextField("note4", note4);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        note5 = EditorGUILayout.TextField("note5", note5);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        note6 = EditorGUILayout.TextField("note6", note6);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        note7 = EditorGUILayout.TextField("note7", note7);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        note8 = EditorGUILayout.TextField("note8", note8);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        note9 = EditorGUILayout.TextField("note9", note9);
        GUILayout.EndHorizontal();

        if (GUILayout.Button(addNoteButtonName)) AddNote();
        Repaint();
    }

    private void createNote(int index)
    {
        GUILayout.BeginHorizontal();
        note1 = EditorGUILayout.TextField("note"+index, note1);
        GUILayout.EndHorizontal();
    }

    private void AddNote()
    {
        string Path = Application.dataPath + "/Scripts/Game/Modules/";
        string modulePath = Path + defaultModuleName + "Module";

        Directory.CreateDirectory(modulePath);
        string noteFilePath = modulePath+"/" + defaultModuleName + "Notes.cs";
        createFile(noteFilePath, getNotesTpl());

        string modulefilePath = modulePath + "/" + defaultModuleName + "Module.cs";
        createFile(modulefilePath, getModuleTpl());

        string proxyfilePath = modulePath + "/" + defaultModuleName + "Proxy.cs";
        createFile(proxyfilePath, getProxyTpl());

        string mediatorfilePath = modulePath + "/" + defaultModuleName + "Mediator.cs";
        createFile(mediatorfilePath, getMediatorTpl());

        string commandfilePath = modulePath + "/" + defaultModuleName + "Command.cs";
        createFile(commandfilePath, getCommandTpl());

        string viewPath = modulePath + "/View";
        Directory.CreateDirectory(viewPath);

        string viewFilePath = viewPath + "/" + defaultModuleName + "View.cs";
        createFile(viewFilePath, getViewTpl());

        string sceneFilePath = viewPath + "/" + defaultModuleName + "Scene.cs";
        createFile(sceneFilePath, getSceneTpl());

        string controllerPath = modulePath + "/Controller";
        Directory.CreateDirectory(controllerPath);
        EditorUtility.DisplayDialog("提示", "模块创建成功！", "确认");
    }

    private string getSceneTpl()
    {
        return "using UnityEngine;\n" +
                    "using System.Collections;\n" +
                    "\n" +
                    "public class "+defaultModuleName+"Scene : Scene\n" +
                    "{\n" +
                    "    public Transform birthPos;\n" +
                    "}";
    }

    private string getViewTpl()
    {
        return "using UnityEngine;\n" +
                    "using System.Collections;\n" +
                    "\n" +
                    "public class " + defaultModuleName + "View : MonoBehaviour\n" +
                    "{\n" +
                    "\n" +
                    "}";
    }

    private string getCommandTpl()
    {
        string commandTpl = "using System;\n"+
        "using System.Collections.Generic;\n"+
        "using System.Linq;\n"+
        "using System.Text;\n"+
        "using PureMVC.Patterns;\n"+
        "using NetProtocol;\n"+
        "public class " + defaultModuleName + "Command : SimpleCommand\n" +
        "{\n"+
        "   public static string NAME = \""+defaultModuleName+"Command\";\n"+
        "    public override void Execute(PureMVC.Interfaces.INotification notification)\n"+
        "    {\n"+
        "        "+defaultModuleName+"Proxy proxy = Facade.RetrieveProxy("+defaultModuleName+"Proxy.NAME) as "+defaultModuleName+"Proxy;\n"+
        "        switch (notification.Type)\n"+
        "        {\n"+
        "               case \"\":\n"+
        "               \n"+
        "               break;\n" +
        "        }\n"+
        "    }\n"+
        "}";
        return commandTpl;
    }

    private string getMediatorTpl()
    {
        return "using JZWLEngine.Managers;\n"+
                    "using PureMVC.Patterns;\n"+
                    "using System;\n"+
                    "using System.Collections.Generic;\n"+
                    "using System.Collections;\n"+
                    "using UnityEngine;\n"+
                    "using JZWLEngine.Loader;\n"+
                    "using NetProtocol;\n"+
                    "\n" +
                    "public class "+defaultModuleName+"Mediator : Mediator\n"+
                    "{\n"+
                    "    new public static string NAME = \""+defaultModuleName+"Mediator\";\n"+
                    "    private " + defaultModuleName + "Scene _scene;\n" +
                    "    private " + defaultModuleName + "View _UI;\n" +
                    "\n" +
                    "    public "+defaultModuleName+"Mediator(object data)\n"+
                    "        : base(NAME)\n"+
                    "    {\n"+
                    "           if (Scene.Instance == null)\n"+
                    "           {\n"+
                    "               throw new UnityException(\"App Error: Not exist Scene's instance.\");\n"+
                    "           }\n"+
                    "\n" +
                    "           if (!(Scene.Instance is " + defaultModuleName + "Scene))\n" +
                    "           {\n"+
                    "                throw new UnityException(\"App Error: Scene instance is not a "+defaultModuleName+"Scene.\");\n"+
                    "           }\n"+
                    "           _scene = Scene.Instance as " + defaultModuleName + "Scene;\n" +
                    "\n" +
                    "           GameObject go = _scene.birthPos.gameObject;\n"+
                    "           _UI = go.GetComponent<" + defaultModuleName + "View>();\n" +
                    "    }\n"+
                    "\n" +
                    "    public override IList<string> ListNotificationInterests()\n"+
                    "    {\n"+
                    "        return new List<String>()\n"+
                    "        {\n"+
            
                    "        };\n"+
                    "    }\n"+
                    "\n" +
                    "    public override void HandleNotification(PureMVC.Interfaces.INotification notification)\n"+
                    "    {\n"+
                    "        switch (notification.Name)\n"+
                    "        {\n"+
                    "            default:\n"+
                    "                break;\n"+
                    "        }\n"+
                    "    }\n"+
                    "\n" +
                    "    public override void OnRegister()\n"+
                    "    {\n"+

                    "    }\n"+
                    "\n" +
                    "    public override void OnRemove()\n"+
                    "    {\n"+
                    "\n" +
                    "    }\n"+
                    "}";
    }

    private string getProxyTpl()
    {
        return "using UnityEngine;\n"+
                    "using System.Collections;\n"+
                    "using PureMVC.Patterns;\n"+
                    "using JZWLEngine.Managers;\n"+
                    "using NetProtocol;\n"+
                    "\n" +
                    "public class "+defaultModuleName+"Proxy:Proxy\n"+
                    "{\n"+
                    "    public new static string NAME = \""+defaultModuleName+"Proxy\";\n"+
                    "    public "+defaultModuleName+"Proxy()\n"+
                    "        : base(NAME)\n"+
                    "    {\n"+
                    "\n" +
                    "    }\n"+
                    "\n" +
                    "    public override void OnRegister()\n"+
                    "    {\n"+
                    "\n" +
                    "    }\n"+
                    "}";
    }

    private string getModuleTpl()
    {
        return "using UnityEngine;\n" +
                    "using System.Collections;\n" +
                    "using NetProtocol;\n" +
                    "using System.Collections.Generic;\n" +
                    "using JZWLEngine.Managers.Helper;\n" +
                    "using JZWLEngine.Loader;\n" +
                    "using JZWLEngine.Managers;\n" +
                    "\n" +
                    "public class " + defaultModuleName + "Module : BaseModule\n" +
                    "{\n" +
                    "    public " + defaultModuleName + "Module(): base(SceneConfig.SceneName."+defaultModuleName+".ToString())\n" +
                    "    {\n" +
                    "    }\n" +
                    "\n" +
                    "    protected override void _Start()\n" +
                    "    {\n" +
                    "        facade.RegisterProxy(new " + defaultModuleName + "Proxy());\n" +
                    "        facade.RegisterMediator(new " + defaultModuleName + "Mediator(_data));\n" +
                    "        facade.RegisterCommand(" + defaultModuleName + "Command.NAME, typeof(" + defaultModuleName + "Command));\n" +
                    "    }\n" +
                    "\n" +
                    "    protected override void _Dispose()\n" +
                    "    {\n" +
                    "        facade.RemoveProxy(" + defaultModuleName + "Proxy.NAME);\n" +
                    "        facade.RemoveMediator(" + defaultModuleName + "Mediator.NAME);\n" +
                    "        facade.RemoveCommand(" + defaultModuleName + "Command.NAME);\n" +
                    "    }\n" +
                    "}";
    }

    private string getNotesTpl()
    {
        string note = "";
        if (note1 != "") note += "      public const string " + note1 + " = \"" + note1 + "\";\n";
        if (note2 != "") note += "      public const string " + note2 + " = \"" + note2 + "\";\n";
        if (note3 != "") note += "      public const string " + note3 + " = \"" + note3 + "\";\n";
        if (note4 != "") note += "      public const string " + note4 + " = \"" + note4 + "\";\n";
        if (note5 != "") note += "      public const string " + note5 + " = \"" + note5 + "\";\n";
        if (note6 != "") note += "      public const string " + note6 + " = \"" + note6 + "\";\n";
        if (note7 != "") note += "      public const string " + note7 + " = \"" + note7 + "\";\n";
        if (note8 != "") note += "      public const string " + note8 + " = \"" + note8 + "\";\n";
        if (note9 != "") note += "      public const string " + note9 + " = \"" + note9 + "\";\n";

        string noteScriptTpl = "using UnityEngine;" + "\n" +
                                            "using System.Collections;" + "\n" +
                                            "public class "+defaultModuleName+"Notes" + "\n" +
                                            "{" + "\n" +
                                                    note + "\n" +
                                            "}";
        return noteScriptTpl;
    }

    private void createFile(string filePath,string content)
    {
        StreamWriter sw;
        FileInfo t = new FileInfo(filePath);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }
        
        sw.WriteLine(content);
        sw.Close();
        sw.Dispose();
    }
}