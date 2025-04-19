#if UNITY_EDITOR
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum ENextBuildType
{
    none,
    major,
    minor,
    build,
}

public class BuildTool : EditorWindow
{
    private Vector2 scrollPosition; // Stores the current scroll position
    BuildToolConfig config;

    string versionName;
    int nextBundle = 0;
    ENextBuildType nextBuildType = ENextBuildType.none;
    string envBuilding = "";

    [MenuItem("--TOOL--/BuildTool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BuildTool));
    }

    private void OnGUI()
    {
        if (config == null)
        {
            config = BuildToolConfig.instance;
            if (GUILayout.Button("Load Existing Data"))
            {
                string path = EditorUtility.OpenFilePanel("Load Data", "Assets/_system/BuildTool/Scriptable/BuildToolConfig.asset", "asset");
                if (!string.IsNullOrEmpty(path))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                    config = AssetDatabase.LoadAssetAtPath<BuildToolConfig>(path);
                }
            }
        }
        else
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height));

            //InitSetScreen();
            InitSymbol();
            InitLine();
            EditorGUILayout.LabelField($"Symbol: {GetAllSymbol()}");
            InitLine();
            InitNextBuildVersion();
            InitLine();
            InitBundle();
            InitLine();
            InitButtonBuild();

            EditorGUILayout.EndScrollView();
        }
    }

    #region init window
    void InitSetScreen()
    {
        if (GUILayout.Button("Set Screen For Design"))
        {
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        }
        if (GUILayout.Button("Set Screen For Game"))
        {
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        }
    }

    void InitSymbol()
    {
        foreach (var symbol in config.lSymbol)
        {
            if (symbol.isDisplay)
            {
                InitLine();
                InitSymbol(symbol.symbol);
            }
        }
    }

    void InitSymbol(string symbol)
    {
        var isEnable = IsDefineSymbolDefined(symbol);

        GUIStyle textStyle = new GUIStyle(EditorStyles.label);
        textStyle.normal.textColor = isEnable ? Color.green : Color.red;

        var strEna = isEnable ? "actived" : "disabled";
        EditorGUILayout.LabelField($"{symbol}: {strEna}", textStyle);

        var strButton = isEnable ? "Disable" : "Enable";
        if (GUILayout.Button($"{strButton} {symbol}"))
        {
            if (!isEnable)
            {
                AddDebugLogDefineSymbolForGroup(BuildTargetGroup.Android, symbol);
                AddDebugLogDefineSymbolForGroup(BuildTargetGroup.Standalone, symbol);
            }
            else
            {
                RemoveDebugLogDefineSymbolForGroup(BuildTargetGroup.Android, symbol);
                RemoveDebugLogDefineSymbolForGroup(BuildTargetGroup.Standalone, symbol);
            }
            Repaint();
        }
    }

    void InitButtonBuild()
    {
        string env = "null";

#if UNITY_ANDROID
        env = ".apk";
#elif UNITY_UWP
        env = "UWP";
#elif UNITY_EDITOR
        env = "Window";
#endif

        if (GUILayout.Button($"Build Game ({env})"))
        {
            Build(EBuildType.game);
        }
#if UNITY_ANDROID
        //if (GUILayout.Button($"Build Game (.aab)"))
        //{
        //    Build(EBuildType.game, true);
        //}

        //if (GUILayout.Button($"Revert config app for release"))
        //{
        //    RevertSettingForEditor();
        //}
#endif
    }

    void InitNextBuildVersion()
    {
#if UNITY_UWP
        versionName = $"{PlayerSettings.WSA.packageVersion.Major}.{PlayerSettings.WSA.packageVersion.Minor}.{PlayerSettings.WSA.packageVersion.Build}";
        GUILayout.Label($"Current Version Name: {PlayerSettings.WSA.packageVersion}");
#else
        versionName = PlayerSettings.bundleVersion;
        GUILayout.Label($"Current Version Name: {Application.version}");
#endif

        EditorGUILayout.BeginHorizontal();
        InitNextBuildVersion(ENextBuildType.none);
        InitNextBuildVersion(ENextBuildType.major);
        InitNextBuildVersion(ENextBuildType.minor);
        InitNextBuildVersion(ENextBuildType.build);
        EditorGUILayout.EndHorizontal();

        GUILayout.Label($"Next Version Name: {GetNextBuild()}");
    }

    void InitNextBuildVersion(ENextBuildType nextBuildType)
    {
        GUIStyle textBtnStyle = new GUIStyle(EditorStyles.miniButton);
        textBtnStyle.normal.textColor = this.nextBuildType == nextBuildType ? Color.green : Color.white;

        if (GUILayout.Button($"{nextBuildType}", textBtnStyle))
            this.nextBuildType = nextBuildType;
    }

    void InitBundle()
    {
#if UNITY_ANDROID
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button($"Prev Bundle"))
            nextBundle--;

        if (GUILayout.Button($"Next Bundle"))
            nextBundle++;
        EditorGUILayout.EndHorizontal();

        GUIStyle textBundleStyle = new GUIStyle(EditorStyles.label);
        textBundleStyle.normal.textColor = nextBundle == 0 ? Color.white :
            nextBundle > 0 ? Color.green : Color.red;

        EditorGUILayout.LabelField($"Bundle Version: {PlayerSettings.Android.bundleVersionCode + nextBundle}", textBundleStyle);
#endif
    }

    void InitLine()
    {
        GUIStyle textStyle = new GUIStyle(EditorStyles.label);
        textStyle.normal.textColor = Color.red;
        EditorGUILayout.LabelField($"--------------------------------------------------------", textStyle);
    }

    string GetNextBuild()
    {
        var v = GetNextBuildDetail();

        return $"{v.major}.{v.minor}.{v.build}";
    }

    private (int major, int minor, int build) GetNextBuildDetail()
    {
        var v = versionName.Split(".");
        var v_1 = Utils.StringToInt(v[0]);
        var v_2 = Utils.StringToInt(v[1]);
        var v_3 = Utils.StringToInt(v[2]);
        switch (nextBuildType)
        {
            case ENextBuildType.none: return (v_1, v_2, v_3);
            case ENextBuildType.major: v_1++; v_2 = 0; v_3 = 0; break;
            case ENextBuildType.minor: v_2++; v_3 = 0; break;
            case ENextBuildType.build: v_3++; break;
        }
        return (v_1, v_2, v_3);
    }
    #endregion

    #region build
    void Build(EBuildType buildType, bool isAAB = false)
    {
        var pathProject = Utils.GetProjectPath();

        var pathBuilds = $"{pathProject}/Builds";
        if (!Utils.IsExistFolder(pathBuilds))
            Utils.CreateFolder(pathBuilds);

        PlayerSettings.bundleVersion = GetNextBuild();

        var isMobile = false;
#if UNITY_ANDROID
        isMobile = true;
#endif

        PrepareBuild(isMobile);

        var extraStr = buildType switch
        {
            EBuildType.design => "_Design",
            EBuildType.track => "_Trace",
            EBuildType.game => isAAB ? "_Game" : "_Dev",
            _ => ""
        };

#if UNITY_ANDROID
        envBuilding = isAAB ? "AAB" : "APK" + extraStr;
        AndroidBuild(pathBuilds, buildType, isAAB);
#elif UNITY_EDITOR
        envBuilding = "Window" + extraStr;
        WindowBuild(pathBuilds, isDesign);
#endif
    }

    void PrepareBuild(bool isMobile)
    {
        string folderConfigEditor = $"{Utils.GetProjectPath()}/Configs";
        string folderConfigAsset = !isMobile ? $"{Application.streamingAssetsPath}/Configs" :
            $"{Application.dataPath}/Configs";

        if (Utils.IsExistFolder(folderConfigAsset))
        {
            Utils.RemoveFolder(folderConfigAsset);
        }

        Utils.CopyFolder(folderConfigEditor, folderConfigAsset);
        UnityEngine.Debug.Log("Save file config success!");
    }

    void AndroidBuild(string pathBuilds, EBuildType buildType, bool isAAB)
    {
        string productName = GetProductName(buildType, isAAB);

        string folder = GetFolder_Android(buildType);

        folder = isAAB ? "Android_Release" : folder;
        var pathBuildApp = $"{pathBuilds}/{folder}";
        if (!Utils.IsExistFolder(pathBuildApp))
            Utils.CreateFolder(pathBuildApp);

        EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Debugging;

        // Set the path where the APK or AAB will be generated
        string formatFile = isAAB ? "aab" : "apk";
        PlayerSettings.Android.bundleVersionCode += nextBundle;
        string buildPath = $"{pathBuildApp}/{productName}_v.{Application.version}_{PlayerSettings.Android.bundleVersionCode}.{formatFile}";
        BuildOptions options = isAAB ? BuildOptions.None : BuildOptions.Development;

        Build(buildPath, BuildTarget.Android, options: options, buildType: buildType, isAAB: isAAB);
    }

    void WindowBuild(string pathBuilds, EBuildType buildType)
    {
        var folder = GetFolder_Window(buildType);
        var pathBuildApp = $"{pathBuilds}/{folder}";
        if (!Utils.IsExistFolder(pathBuildApp))
            Utils.CreateFolder(pathBuildApp);

        string productName = GetProductName(buildType, false);

        var pathFinish = $"{pathBuildApp}/{productName}_V.{Application.version}";
        if (!Utils.IsExistFolder(pathFinish))
            Utils.CreateFolder(pathFinish);

        var buildPath = $"{pathFinish}/{productName}.exe";
        Build(buildPath, BuildTarget.StandaloneWindows, buildType: buildType);

        //Run build
        //Process proc = new Process();
        //proc.StartInfo.FileName = pathFinish + $"/{config.gameName}.exe";
        //proc.Start();
    }

    void Build(string buildPath, BuildTarget buildTarget, BuildOptions options = BuildOptions.Development, EBuildType buildType = EBuildType.game, bool isAAB = false)
    {
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;

        PlayerSettings.productName = GetProductName(buildType, isAAB);
        PlayerSettings.applicationIdentifier = GetPackageName(buildType, isAAB);
        //LoadIcon(isDesign, isAAB).Forget();

        string[] extraScriptingDefines =
            buildTarget == BuildTarget.Android ?
            isAAB ? new string[] { "BUILDED_FLAG", "RELEASE" } : new string[] { "BUILDED_FLAG", "DEV" }
            : new string[] { "BUILDED_FLAG" };

        // Set build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetScene(buildType),
            locationPathName = buildPath,
            target = buildTarget,
            options = options,
            extraScriptingDefines = extraScriptingDefines,
        };

        // Enable App Bundle (Google Play)
        PlayerSettings.Android.useCustomKeystore = isAAB;
        EditorUserBuildSettings.buildAppBundle = isAAB;

        PlayerSettings.Android.minifyRelease = isAAB;
        PlayerSettings.Android.minifyDebug = !isAAB;
        PlayerSettings.Android.useAPKExpansionFiles = isAAB;

        //if (isAAB)
        //{
        //    PlayerSettings.Android.keystorePass = config.keystorePass;
        //    PlayerSettings.Android.keyaliasName = config.keyAliasName;
        //    PlayerSettings.Android.keyaliasPass = config.keyAliasPass;
        //}

        // Build the APK
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        string env = buildTarget == BuildTarget.StandaloneWindows ? "Window" : "Android";
        env = buildTarget == BuildTarget.WSAPlayer ? "UWP" : env;
        env = isAAB ? "AAB" : env;
        var folder = GetFolder_Android(buildType);

        // Check if the build succeeded
        if (summary.result == BuildResult.Succeeded)
        {
            EditorUtility.RevealInFinder(buildPath);
            ShowToast($"{folder} build succeeded ({env}): {summary.totalSize} bytes");
            RevertSettingForEditor();
        }
        else if (summary.result == BuildResult.Failed)
        {
            ShowToast($"{folder} build failed ({env}");
            RevertSettingForEditor();
        }
    }
    #endregion

    #region other func
    void RevertSettingForEditor()
    {
        EditorUserBuildSettings.buildAppBundle = false;
#if !UNITY_UWP
        EditorUserBuildSettings.development = true;
#endif
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;

        PlayerSettings.Android.minifyRelease = false;
        PlayerSettings.Android.minifyDebug = false;
        PlayerSettings.Android.useAPKExpansionFiles = false;

        PlayerSettings.productName = GetProductName(EBuildType.game, true);
        PlayerSettings.applicationIdentifier = GetPackageName(EBuildType.game, true);

        nextBuildType = ENextBuildType.none;
        nextBundle = 0;
        envBuilding = "";
    }

    string GetFolder_Android(EBuildType buildType)
    {
        return buildType switch
        {
            EBuildType.design => "Android_Design",
            EBuildType.track => "Android_Trace",
            EBuildType.game => "Android_Game",
            _ => "Error",
        };
    }

    string GetFolder_Window(EBuildType buildType)
    {
        return buildType switch
        {
            EBuildType.design => "Window_Design",
            EBuildType.track => "Window_Trace",
            EBuildType.game => "Window_Game",
            _ => "Error",
        };
    }

    string GetProductName(EBuildType buildType, bool isAAB)
    {
#if UNITY_UWP
        return buildType == EBuildType.design ? $"{config.gameNameUWP}_Design" : $"{config.gameNameUWP}";
#else
        return buildType switch
        {
            EBuildType.design => $"{config.gameName}_Design",
            EBuildType.track => $"{config.gameName}_Track",
            EBuildType.game => $"{config.gameName}",
            _ => "Error",
        };
#endif
    }

    string GetPackageName(EBuildType buildType, bool isAAB)
    {
#if UNITY_UWP
        return buildType == EBuildType.design ? $"{config.companyNameUWP}.{config.gameNameUWP}_Design" : $"{config.companyNameUWP}.{config.gameNameUWP}";
#else
        return buildType switch
        {
            EBuildType.design => $"com.{config.companyName}.{config.packageName}_Design",
            EBuildType.track => $"com.{config.companyName}.{config.packageName}_Track",
            EBuildType.game => $"com.{config.companyName}.{config.packageName}",
            _ => "Error",
        };
#endif
    }  

    string[] GetScene(EBuildType buildType)
    {
        string[] lScene = config.GetScene(buildType);
        string parentPath = config.GetParentPathScene(buildType);
        List<string> scenes = new();
        string path = "";

        foreach (string scene in lScene)
        {
            scenes.Add($"{parentPath}/{scene}");
        }

#if DebugLog
        scenes.Add("Assets/Mobile Console/Assets/LogConsole.unity");
#endif

        string[] ret = new string[scenes.Count()];
        for (int i = 0; i < scenes.Count(); i++)
        {
            ret[i] = scenes[i];
        }

        return ret;
    }

    static void AddDebugLogDefineSymbolForGroup(BuildTargetGroup group, string symbol)
    {
        string defineSymbolsString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        List<string> defineSymbols = defineSymbolsString.Split(';').ToList();
        if (!defineSymbols.Contains(symbol))
        {
            defineSymbols.Add(symbol);
            string newDefineSymbolsString = string.Join(";", defineSymbols.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, newDefineSymbolsString);
        }
    }

    static void RemoveDebugLogDefineSymbolForGroup(BuildTargetGroup group, string symbol)
    {
        string defineSymbolsString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        List<string> defineSymbols = defineSymbolsString.Split(';').ToList();
        if (defineSymbols.Contains(symbol))
        {
            defineSymbols.Remove(symbol);
            string newDefineSymbolsString = string.Join(";", defineSymbols.ToArray());
            PlayerSettings.SetScriptingDefineSymbolsForGroup(group, newDefineSymbolsString);
        }
    }

    private string GetAllSymbol()
    {
        var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
    }

    private bool IsDefineSymbolDefined(string symbol)
    {
        var defineSymbols = GetAllSymbol();

        return defineSymbols.Split(';').Contains(symbol);
    }

    void ShowToast(string msg)
    {
        EditorUtility.DisplayDialog("Notice", msg, "Ok");
    }    
    #endregion
}
#endif
