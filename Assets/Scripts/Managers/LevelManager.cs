using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager: MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
