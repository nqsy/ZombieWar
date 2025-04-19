#if UNITY_EDITOR
using UnityEditor;

public class BuildToolSettingEditor : EditorWindow
{
    [MenuItem("--TOOL--/BuildToolSetting")]
    public static void OpenInspector()
    {
        Selection.activeObject = BuildToolConfig.instance;
    }
}
#endif