using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] GameObject _settingsWindow;

    public AudioMixer audioMixer;
    public AudioSource audioSource;
    public AudioClip honk;
    public Slider musicSlider;
    public Slider soundSlider;
    void Start()
    {
        audioMixer.GetFloat("music", out float musicValueForSlider);
        musicSlider.value = musicValueForSlider;

        audioMixer.GetFloat("sound", out float soundValueForSlider);
        soundSlider.value = soundValueForSlider;

    }

    void Update()
    {
        
    }

    public void CloseSettingsMenu()
    {
        _settingsWindow.SetActive(false);
    }

    public void SetFullScreen(bool isFullScreen)
    {

        Screen.fullScreen = isFullScreen;

    }

    public void SetVolumeMusic(float volume) 
    {
        audioMixer.SetFloat("music", volume);
    }
    public void SetVolumeSound(float soundVolume)
    {
        audioMixer.SetFloat("sound", soundVolume);

    }

    public void HonkFunction()
    {
        audioSource.PlayOneShot(honk);

    }
}
