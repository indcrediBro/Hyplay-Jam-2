using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : Singleton<SettingsManager>
{
    [Header("Audio Settings")]
    public AudioMixer audioMixer;            // Reference to the AudioMixer
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";

    private float musicVolume;
    private float sfxVolume;

    [Header("UI Settings")]
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    private void Start()
    {
        InitializeUI();
        LoadSettings();
    }

    private void InitializeUI()
    {
        // Initialize sliders with current settings
        musicVolumeSlider.minValue = 0.001f;
        sfxVolumeSlider.minValue = 0.001f;

        musicVolumeSlider.value = GetMusicVolume();
        sfxVolumeSlider.value = GetSFXVolume();

        // Add listeners to sliders
        musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnMusicVolumeChanged(float value)
    {
        SetMusicVolume(value);
        ApplySettings();
    }

    private void OnSFXVolumeChanged(float value)
    {
        SetSFXVolume(value);
        ApplySettings();
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        audioMixer.SetFloat(musicVolumeParameter, musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        audioMixer.SetFloat(sfxVolumeParameter, sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    private void LoadSettings()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
    }

    public void ApplySettings()
    {
        audioMixer.SetFloat(musicVolumeParameter, Mathf.Log10(GetMusicVolume()) * 20);
        audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(GetSFXVolume()) * 20);
    }
}
