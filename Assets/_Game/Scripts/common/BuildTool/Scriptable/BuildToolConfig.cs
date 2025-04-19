using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SymbolConfig
{
    public string symbol;
    public bool isDisplay;
}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BuildToolConfig", order = 1)]
public class BuildToolConfig : ScriptableObject
{
    public string gameName;
    public string packageName;
    public string companyName;

    //[Header("key store")]
    //public string keystorePass;
    //public string keyAliasName;
    //public string keyAliasPass;

    [Header("symbol")]
    public List<SymbolConfig> lSymbol;

    [Header("Scenes")]
    public string parentPathGameScene;
    public string[] lGameScene;
    //public string parentPathDesignScene;
    //public string[] lDesignScene;
    //public string parentPathTraceScene;
    //public string[] lTraceScene;

    public string[] GetScene(EBuildType buildType)
    {
        return buildType switch
        {
            //EBuildType.design => lDesignScene,
            //EBuildType.track => lTraceScene,
            EBuildType.game => lGameScene,
            _ => null,
        };
    }

    public string GetParentPathScene(EBuildType buildType)
    {
        return buildType switch
        {
            //EBuildType.design => parentPathDesignScene,
            //EBuildType.track => parentPathTraceScene,
            EBuildType.game => parentPathGameScene,
            _ => null,
        };
    }

    public static BuildToolConfig instance => Resources.Load<BuildToolConfig>("BuildToolConfig");
}
