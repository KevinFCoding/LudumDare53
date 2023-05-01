using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    [SerializeField] GameManager _gameManager;

    private string _currentScene;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _currentScene = SceneManager.GetActiveScene().name;
    }

    private void Update()
    {
        if(_currentScene != SceneManager.GetActiveScene().name)
        {
            _gameManager.SceneHasChanged();
            _currentScene = SceneManager.GetActiveScene().name;
        }
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadScene("Level1");
    }

    /**
     *  Load Next level based on the buildIndex
     */
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("MainScene");
    }
}
