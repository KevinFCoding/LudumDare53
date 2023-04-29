using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] Player _player;

    [SerializeField] MailBox[] _mailboxes;
    private bool _gameIsPlaying = false;

    private int spam;
    private int win;
    private int lose;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        SetUpLevel();
    }

    public void SceneHasChanged()
    {
        _player = FindObjectOfType<Player>();
        if(_gameIsPlaying)
        {
            SetUpLevel();
        }
    }

    private void SetUpLevel()
    {
        _mailboxes = GameObject.FindObjectsOfType<MailBox>();
        SetUpBoxes();
    }

    private void SetUpBoxes()
    {
        if (_mailboxes.Length <= 0) return;
        if (_mailboxes.Length == 1) _mailboxes[0].isWin();
        else if (_mailboxes.Length == 2)
        {
            _mailboxes[0].isWin();
            _mailboxes[1].isLose();
        }
        else if (_mailboxes.Length >= 3)
        {
            spam = Random.Range(0, _mailboxes.Length);
            _mailboxes[spam].isSpam();
           win = Random.Range(0, _mailboxes.Length);
            _mailboxes[win].isWin();

            lose = Random.Range(0, _mailboxes.Length);
            _mailboxes[lose].isLose();
        }
    }

    public void StartPlaying()
    {
        _levelManager.LoadLevelOne();
        _gameIsPlaying = true;
    }

    public void LevelOver()
    {
        _levelManager.LoadNextLevel();
    }

    public void GameOver()
    {
        _gameIsPlaying = false;
        _levelManager.LoadGameOver();
    }
}
