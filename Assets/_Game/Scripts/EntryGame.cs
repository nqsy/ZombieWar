using UnityEngine;

public class EntryGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        Application.targetFrameRate = 60;
#endif

        TransitionEffect.instance.LoadMenuScene();
    }
}
