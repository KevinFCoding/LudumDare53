using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] LevelManager _levelManager;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] DrawingManager _drawingManager;

    [SerializeField] Player _player;

    [Header("Boxes and Set up")]
    [SerializeField] List<MailBox> _mailboxes;
    [SerializeField] List<GameObject> _deliveryThreads;
    [SerializeField] List<GameObject> _spawners;

    [Header("Game Points and Rewards")]
    public int points = 0;
    public bool perfectDelivery = true;

    private bool _gameIsPlaying = false;

    private int spam;
    private int win;
    private int lose;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //_drawingManager = FindObjectOfType<DrawingManager>();
        //SetUpLevel();
        //SetUpThreadsAndSpawners();
    }

    public void SceneHasChanged()
    {
        _player = FindObjectOfType<Player>();
        _drawingManager = FindObjectOfType<DrawingManager>();
        if(_gameIsPlaying)
        {
            SetUpLevel();
            SetUpThreadsAndSpawners();
            LaunchVlopAnimation();
        }
    }
    #region New Scene Set upping
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
    private void SetUpThreadsAndSpawners()
    {
        GameObject gameObject = GameObject.Find("DeliveryThreads"); 
        DeliveryThread[] tempTab = gameObject.GetComponentsInChildren<DeliveryThread>();

        foreach (DeliveryThread dt in tempTab)
        {
            _deliveryThreads.Add(dt.gameObject);
            _spawners.Add(dt.gameObject.GetComponentInParent<Spawner>().gameObject);
        }
            _drawingManager.SetThread(_deliveryThreads);
    }

    private void LaunchVlopAnimation()
    {
        _player.GameStarted();
    }

    #endregion

    #region GameFlow

    public void StartPlaying()
    {
        _gameIsPlaying = true;
    }

    public void PlayerHasDeliveredTheMail(string boxDelivered) { 
        if(boxDelivered == "spam")
        {
            if (_player.isPlayerInfected()) points += 2;
            perfectDelivery = false;
            LevelOver();
        }
        else if (boxDelivered == "win")
        {
            points += 4;
            LevelOver();
        } else if (boxDelivered == "lose")
        {
            perfectDelivery = false;
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
    #endregion
}
