using UnityEngine;

[RequireComponent(typeof(WA_AmmoClip)), RequireComponent(typeof(AudioSource))]
public class WA_FireSound : MonoBehaviour
{
    [Header("Attributes")]

    [Tooltip("Sound to play when firing successfully")]
    public AudioClip fireSound;

    [Tooltip("Sound to play when fire failed")]
    public AudioClip failedFireSound;

    //Source that will play the audio
    [SerializeField] private AudioSource audSource;

    private void Start()
    {
        //Grab Components
        audSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Play the designated sound of proper fire
    /// </summary>
    public void ConductFireSound()
    {
        audSource.PlayOneShot(fireSound);
    }

    /// <summary>
    /// Play the designated sound of failed fire
    /// </summary>
    public void ConductFailedFireSound()
    {
        audSource.PlayOneShot(failedFireSound);
    }
}
