using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Elements")]
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _quitButton;
    [SerializeField] GameObject _settingsWindow;


    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip errorSound;


    [SerializeField] Image _image;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        Debug.Log("StartGame");
      //  SceneManager.LoadScene("SceneMenu");
    }

    public void OnPointerEnter()
    {
        _image.enabled = true;
    }

    public void OnPointerExit()
    {
        _image.enabled = false;
    }
    public void OpenSettingsWindows()
    {
        _settingsWindow.SetActive(true);
        audioSource.PlayOneShot(errorSound);
    }
}
