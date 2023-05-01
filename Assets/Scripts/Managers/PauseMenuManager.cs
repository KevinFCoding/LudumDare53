using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuManager : MonoBehaviour
{
    public static bool isPaused;

    [Header("Manager")]
    [SerializeField] GameManager _gameManager;
    [SerializeField] DrawingManager _drawingManager;

    [SerializeField] GameObject _pauseMenuUI;
    [SerializeField] GameObject _selfi;
    [SerializeField] GameObject _videoTuto;

    [SerializeField] SettingsWindow _settingsWindows;
    [SerializeField] AudioClip errorClip;

    private AudioSource _audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _audioSource = GameObject.FindAnyObjectByType<AudioSource>();

    }
  
    void Update()
    {
        if (_gameManager.gameIsPlaying)
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
    }

    public void SetDrawingManager()
    {
        _drawingManager = GameObject.FindObjectOfType<DrawingManager>();
    }

    public void Paused()
    {
        _drawingManager.GamePaused(true);
        _audioSource.PlayOneShot(errorClip);
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }

    public void Resume()
    {
        _drawingManager.GamePaused(false);
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        CloseTuto();
        _settingsWindows.CloseSettingsMenu();

    }

    public void SettingsWindow()
    {
        Debug.Log("SettingsWindow PRESSED");
        _settingsWindows.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        Debug.Log("MainMenu PRESSED");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    public void CloseTuto()
    {
        _videoTuto.SetActive(false);
        _selfi.SetActive(false);
    }

    public void OpenTuto()
    {
        Debug.Log("OpenTuto PRESSED");
        _selfi.SetActive(true);
        _videoTuto.SetActive(true);
    }
}
