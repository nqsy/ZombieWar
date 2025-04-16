using UnityEngine;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [SerializeField] AudioSource audioSource;

    public void PlaySound(ESoundType soundType)
    {
        var audioClip = SoundConfig.instance.GetClip(soundType);
        audioSource.PlayOneShot(audioClip);
    }
}
