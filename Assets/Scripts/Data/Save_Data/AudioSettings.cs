using UnityEngine;

public class AudioSettings
{
    [Tooltip("Saved value for Master"), Range(0, 1f)]
    public float masterVolume;

    [Tooltip("Saved value for Music"), Range(0, 1f)]
    public float musicVolume;

    [Tooltip("Saved value for SFX"), Range(0, 1f)]
    public float sfxVolume;
}
