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
    [SerializeField] GameObject _launchWindow;
    [SerializeField] GameObject _menuContent;
    [SerializeField] GameObject _credits;
    [SerializeField] GameObject _bestGame;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip errorSound;
    public AudioClip windowsXpSound;

    [SerializeField] GameObject[] incons;
        

    void Start()
    {
        audioSource.PlayOneShot(windowsXpSound);
        Invoke("ShowMainMenu", 3f);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMainMenu()
    {
        _launchWindow.SetActive(false);
        StartCoroutine(ActiveIcons());

    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void ShowCredits()
    {
        audioSource.PlayOneShot(errorSound);

        _credits.SetActive(true);
    }
    public void HideCredits()
    {
        _credits.SetActive(false);

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }



    
    public void OpenSettingsWindows()
    {
        audioSource.PlayOneShot(errorSound);
        _settingsWindow.SetActive(true);
    }

    IEnumerator ActiveIcons()
    {
        yield return new WaitForSeconds(0.2f);
        incons[0].SetActive(true);
        

        yield return new WaitForSeconds(0.2f);

        incons[0].SetActive(false);

        incons[2].SetActive(true);

        yield return new WaitForSeconds(0.2f);
        incons[3].SetActive(true);

        incons[2].SetActive(false);
        yield return new WaitForSeconds(0.2f);
        incons[3].SetActive(false);

        incons[4].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        incons[5].SetActive(true);

        incons[4].SetActive(false);
        yield return new WaitForSeconds(0.2f);
        incons[6].SetActive(true);

        yield return new WaitForSeconds(0.2f);
        incons[2].SetActive(false);
        incons[2].SetActive(true);
        incons[3].SetActive(true);
        incons[6].SetActive(true);
        incons[4].SetActive(true);
        incons[1].SetActive(true);
        incons[5].SetActive(true);
        incons[7].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        audioSource.PlayOneShot(errorSound);
        _menuContent.SetActive(true);

        yield break;
    }

    public void OpenTheBestFeature()
    {
        _bestGame.SetActive(true);
    }

    public void CloseTheBestFeature()
    {
        _bestGame.SetActive(false);

    }
}
