using UnityEngine;

public class EntryGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TransitionEffect.instance.LoadMenuScene();
    }
}
