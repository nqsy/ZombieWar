#if UNITY_EDITOR
using UnityEditor;

public class SoundConfigEditor : EditorWindow
{
    [MenuItem("--TOOL--/SoundConfigEditor")]
    public static void OpenInspector()
    {
        Selection.activeObject = SoundConfig.instance;
    }
}
#endif