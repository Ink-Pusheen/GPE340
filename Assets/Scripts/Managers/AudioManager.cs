using System.IO;
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


    [Header("Save Files")]

    [Tooltip("File Name for this file")]
    public string filename { get; private set; }

    [Tooltip("Data container of the settings")]
    public AudioSettings audioSettings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set file paths
        filename = Application.persistentDataPath + "/" + "AudioSettings.json";

        LoadSliderValues(); //Load the slider values
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void LoadSliderValues()
    {
        if (File.Exists(filename))
        {
            string readJson = File.ReadAllText(filename);
            audioSettings = JsonUtility.FromJson<AudioSettings>(readJson);

            float masterVolumeAmt = audioSettings.masterVolume;
            float musicVolumeAmt = audioSettings.musicVolume;
            float sfxVolumeAmt = audioSettings.sfxVolume;

            //Set the sliders and apply the valumes
            audioMainSlider.value = masterVolumeAmt;
            audioMusicSlider.value = musicVolumeAmt;
            audioSFXSlider.value = sfxVolumeAmt;

            if (masterVolumeAmt != 0) masterAudioMix.SetFloat("MasterVolume", Mathf.Log10(masterVolumeAmt) * 20);

            else masterAudioMix.SetFloat("MasterVolume", -80f); //Default mute

            if (musicVolumeAmt != 0) masterAudioMix.SetFloat("MusicVolume", Mathf.Log10(musicVolumeAmt) * 20);

            else masterAudioMix.SetFloat("MusicVolume", -80f); //Default mute

            if (sfxVolumeAmt != 0) masterAudioMix.SetFloat("SFXVolume", Mathf.Log10(sfxVolumeAmt) * 20);

            else masterAudioMix.SetFloat("SFXVolume", -80f); //Default mute
        }
        else
        {
            CreateSaveFile(); // Create the save file

            LoadSliderValues(); //Reload the function
        }

        
    }

    private void CreateSaveFile()
    {
        audioSettings = new(); //Create a new audio settings

        //Set the default values
        audioSettings.masterVolume = 1;
        audioSettings.musicVolume = 1;
        audioSettings.sfxVolume = 1;

        //Create and save the file
        string json = JsonUtility.ToJson(audioSettings);

        File.WriteAllText(filename, json);
    }

    public void OnMasterVolumeChange(Slider masterVolumeSlider)
    {
        if (masterVolumeSlider.value != 0) masterAudioMix.SetFloat("MasterVolume", Mathf.Log10(masterVolumeSlider.value) * 20);

        else masterAudioMix.SetFloat("MasterVolume", -80f); //Default mute

        audioSettings.masterVolume = masterVolumeSlider.value; //Update the settings value
    }


    public void OnMusicVolumeChange(Slider musicVolumeSlider)
    {
        if (musicVolumeSlider.value != 0) masterAudioMix.SetFloat("MusicVolume", Mathf.Log10(musicVolumeSlider.value) * 20);

        else masterAudioMix.SetFloat("MusicVolume", -80f); //Default mute

        audioSettings.musicVolume = musicVolumeSlider.value; //Update the settings value
    }


    public void OnSFXVolumeChange(Slider sfxVolumeSlider)
    {
        if (sfxVolumeSlider.value != 0) masterAudioMix.SetFloat("SFXVolume", Mathf.Log10(sfxVolumeSlider.value) * 20);

        else masterAudioMix.SetFloat("SFXVolume", -80f); //Default mute

        audioSettings.sfxVolume = sfxVolumeSlider.value; //Update the settings value
    }



    public void SaveAudioSettings()
    {
        AudioSettings savedSettings = new();

        savedSettings.masterVolume = audioSettings.masterVolume;
        savedSettings.musicVolume = audioSettings.musicVolume;
        savedSettings.sfxVolume = audioSettings.sfxVolume;

        //Create and save the json file
        string json = JsonUtility.ToJson(savedSettings);

        File.WriteAllText(filename, json);
    }
}
