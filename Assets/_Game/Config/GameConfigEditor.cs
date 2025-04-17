#if UNITY_EDITOR
using UnityEditor;

public class GameConfigEditor : EditorWindow
{
    [MenuItem("--Configs--/GameConfigEditor")]
    public static void OpenInspector()
    {
        Selection.activeObject = GameConfig.instance;
    }
}
#endif
