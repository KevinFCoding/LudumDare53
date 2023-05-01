using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = Unity.Mathematics.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int CurrentLevelNumber { get; private set; }

    public event EventHandler<OnScroreGainedEventArgs> OnScoreGained;
    public event EventHandler OnNextLevel;

    public class OnScroreGainedEventArgs : EventArgs
    {
        public int scoreGained;
        public int totalScore;
    }

    [Header("Managers")]
    [SerializeField] LevelManager _levelManager;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] DrawingManager _drawingManager;
    [SerializeField] EndingManager _endingManager;
    [SerializeField] PauseMenuManager _pauseManager;

    [SerializeField] Player _player;

    [Header("Boxes and Set up")]
    [SerializeField] List<MailBox> _mailBoxes;
    [SerializeField] List<GameObject> _deliveryThreads;
    [SerializeField] List<Spawner> _spawners;

    [Header("Game Points and Rewards")]
    public int points = 0;
    public bool perfectDelivery = true;

    private string[] defeatTableName = { "Dad", "Granny", "Bro" };
    public bool gameIsPlaying = false;

    private bool _isMusicStarted = false;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        points = 0;
        CurrentLevelNumber = 1;
        _soundManager = GameObject.FindAnyObjectByType<SoundManager>();
    }

    public void SceneHasChanged()
    {
        _player = FindObjectOfType<Player>();
        _drawingManager = FindObjectOfType<DrawingManager>();
        if (gameIsPlaying)
        {
            EndCanvasReset();
            SetUpLevel();
            SetUpThreadsAndSpawners();
            LaunchVlopAnimation();
            _pauseManager.SetDrawingManager();
        }
    }
    #region New Scene Set upping
    private void SetUpLevel()
    {
        _player.SetCurrentWinThread(null);
        _player.SetCurrentSpamThread(null);

        _mailBoxes.Clear();
        MailBox[] tempTab = GameObject.FindObjectsOfType<MailBox>();
        foreach (MailBox mailbox in tempTab)
        {
            _mailBoxes.Add(mailbox);
        }
        SetUpBoxes();
    }

    private void EndCanvasReset()
    {
        for (int i = 0; i < _endingManager.transform.childCount; i++)
        {
            _endingManager.transform.GetChild(i).gameObject.SetActive(false);
        }
        _endingManager.GetAllParticles();
    }

    private void SetUpBoxes()
    {
        if (_mailBoxes.Count <= 0) return;
        if (_mailBoxes.Count == 1) PlaceBox("Win");
        else if (_mailBoxes.Count == 2)
        {
            PlaceBox("Win");
            PlaceBox("Spam");
        }
        else if (_mailBoxes.Count >= 3)
        {
            PlaceBox("Win");
            PlaceBox("Spam");


            for (int i = 0; i < _mailBoxes.Count - 2; i++)
            {
                if (i < defeatTableName.Length)
                {
                    PlaceBox(defeatTableName[i]);
                }
            }
        }
    }
    private void SetUpThreadsAndSpawners()
    {
        _deliveryThreads.Clear();
        _spawners.Clear();
        GameObject gameObject = GameObject.Find("DeliveryThreads");
        DeliveryThread[] tempTab = gameObject.GetComponentsInChildren<DeliveryThread>();
        _deliveryThreads = new();
        foreach (DeliveryThread dt in tempTab)
        {
            _deliveryThreads.Add(dt.gameObject);
            _spawners.Add(dt.gameObject.GetComponentInParent<Spawner>());
        }
        _drawingManager.SetThread(_deliveryThreads);
        StartCoroutine(SpawnViruses());
    }

    IEnumerator SpawnViruses()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < _spawners.Count; i++)
        {
            _spawners[i].ActivateSpawners();
        }
    }
    private void LaunchVlopAnimation()
    {
        if (!_isMusicStarted)
        {
            _soundManager.StartAudio();
            _isMusicStarted = true;
        }
        _player.GameStarted();
    }
    private void PlaceBox(string name)
    {
        int random = UnityEngine.Random.Range(0, _mailBoxes.Count);
        if (_mailBoxes[random].nameBox != null)
        {
            PlaceBox(name);
            return;
        }
        if (_mailBoxes[random].nameBox == "" || _mailBoxes[random].nameBox == null)
        {
            _mailBoxes[random].MailBoxName(name);

            if (name == "Win")
                _player.SetCurrentWinThread(_mailBoxes[random].gameObject);
            if (name == "Spam")
                _player.SetCurrentSpamThread(_mailBoxes[random].gameObject);
        }
    }
    #endregion

    #region GameFlow

    public void StartPlaying()
    {
        gameIsPlaying = true;
    }

    public void PlayerHasDeliveredTheMail(string boxDelivered)
    {
        _player.StopPlayer();
        if (boxDelivered == "Spam")
        {
            if (_player.isPlayerInfected())
            {
                int scoreGained = Mathf.Clamp(100 - _player.GetNumberOfInfections() * 30, 0, 100);
                points += scoreGained;
                OnScoreGained?.Invoke(this, new OnScroreGainedEventArgs { scoreGained = scoreGained, totalScore = points });
            }

            perfectDelivery = false;
            _endingManager.Spam();
        }
        else if (boxDelivered == "Win")
        {
            if (_player.isPlayerInfected())
            {
                _endingManager.Infected();
            }
            else
            {
                points += 200;
                OnScoreGained?.Invoke(this, new OnScroreGainedEventArgs { scoreGained = 200, totalScore = points });
                _endingManager.Win();
            }
        }
        else if (boxDelivered == "Dad")
        {
            perfectDelivery = false;
            _endingManager.LooseGrampa();
        }
        else if (boxDelivered == "Granny")
        {
            perfectDelivery = false;
            _endingManager.Loose();
        }
        else if (boxDelivered == "Bro")
        {
            perfectDelivery = false;
            _endingManager.LooseBrother();
        }
    }

    public void LevelOver()
    {
        if (SceneManager.GetActiveScene().name == "Level13")
        {
            GameOver();
        }
        else
        {
            CurrentLevelNumber++;
            OnNextLevel?.Invoke(this, EventArgs.Empty);
            _levelManager.LoadNextLevel();
        }
    }

    public void GameOver()
    {
        gameIsPlaying = false;
        _levelManager.LoadGameOver();
    }
    #endregion
}
