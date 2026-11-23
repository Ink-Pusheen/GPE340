using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    [Header("Audio Mixers")]

    public AudioMixer masterAudioMix;


    public Slider audioMainSlider;
    public Slider audioMusicSlider;
    public Slider audioSFXSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnMasterVolumeChange(Slider masterVolumeSlider)
    {
        masterAudioMix.SetFloat("MasterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);
    }


    public void OnMusicVolumeChange(Slider musicVolumeSlider)
    {
        masterAudioMix.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);
    }


    public void OnSFXVolumeChange(Slider sfxVolumeSlider)
    {
        masterAudioMix.SetFloat("SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);
    }
}
