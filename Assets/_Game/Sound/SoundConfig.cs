using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundConfig", menuName = "Configs/SoundConfig", order = 1)]
public class SoundConfig : ScriptableObject
{
    public List<SoundDataItem> items;

    public AudioClip GetClip(ESoundType soundType)
    {
        return items.Find(x => x.soundType == soundType).audioClip;
    }

    public static SoundConfig instance => Resources.Load<SoundConfig>("SoundConfig");
}
