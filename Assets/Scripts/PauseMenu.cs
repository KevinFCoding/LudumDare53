using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;

    [SerializeField] GameObject _pauseMenuUI;
    [SerializeField] GameObject _settingsWindow;
    [SerializeField] GameObject _selfi;
    [SerializeField] GameObject _videoTuto;

    [SerializeField] SettingsWindow _settingsWindows;

    [SerializeField] AudioSource audioMixer;
    [SerializeField] AudioClip errorClip;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    public void Paused()
    {
        audioMixer.PlayOneShot(errorClip);
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        CloseTuto();
        _settingsWindows.CloseSettingsMenu();

    }

    public void SettingsWindow()
    {
        _settingsWindow.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void CloseTuto()
    {
        _videoTuto.SetActive(false);
        _selfi.SetActive(false);
    }

    public void OpenTuto()
    {
        _selfi.SetActive(true);
        _videoTuto.SetActive(true);
    }
}
