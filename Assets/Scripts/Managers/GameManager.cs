using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] LevelManager _levelManager;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] Player _player;

    [Header("Boxes and Set up")]
    [SerializeField] List<MailBox> _mailboxes;

    [Header("Game Points and Rewards")]
    public int points = 0;

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
        _mailboxes = new List<MailBox>();
        MailBox[] tempTab = GameObject.FindObjectsOfType<MailBox>();
        foreach(MailBox mailbox in tempTab)
        {
            _mailboxes.Add(mailbox);
        }
        SetUpBoxes();
    }

    private void SetUpBoxes()
    {
        if (_mailboxes.Count <= 0) return;
        if (_mailboxes.Count == 1) _mailboxes[0].isWin();
        else if (_mailboxes.Count == 2)
        {
            _mailboxes[0].isWin();
            _mailboxes[1].isLose();
        }
        else if (_mailboxes.Count >= 3)
        {
            spam = Random.Range(0, _mailboxes.Count);
            _mailboxes[spam].isSpam();
            _mailboxes.RemoveAt(spam);
           win = Random.Range(0, _mailboxes.Count);
            _mailboxes[win].isWin();
            _mailboxes.RemoveAt(win);
            lose = Random.Range(0, _mailboxes.Count);
            _mailboxes[lose].isLose();
        }
    }

    public void StartPlaying()
    {
        _levelManager.LoadLevelOne();
        _gameIsPlaying = true;
    }

    public void PlayerHasDeliveredTheMail(string boxDelivered) { 
        if(boxDelivered == "spam")
        {
            if (_player.isPlayerInfected()) points += 2;
            LevelOver();
        }
        else if (boxDelivered == "win")
        {
            points += 4;
            LevelOver();
        } else if (boxDelivered == "lose")
        {
            GameOver();
        }
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
