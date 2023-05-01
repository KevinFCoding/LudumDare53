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

    [SerializeField] Text _scoreText;

    [Header("Managers")]
    [SerializeField] LevelManager _levelManager;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] DrawingManager _drawingManager;
    [SerializeField] EndingManager _endingManager;

    [SerializeField] Player _player;

    [Header("Boxes and Set up")]
    [SerializeField] List<MailBox> _mailBoxes;
    [SerializeField] List<GameObject> _deliveryThreads;
    [SerializeField] List<Spawner> _spawners;

    [Header("Game Points and Rewards")]
    public int points = 0;
    public bool perfectDelivery = true;

    private string[] defeatTableName = {"Dad", "Granny", "Bro" };
    public bool gameIsPlaying = false;

    private int spam;
    private int win;
    private int lose;

    private bool _isPaused = false;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        //_drawingManager = FindObjectOfType<DrawingManager>();
        //SetUpLevel();
        //SetUpThreadsAndSpawners();
        points = 0;
        _scoreText.text = points.ToString();

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
        if (_mailBoxes.Count == 1) PlaceBox("Win"); //_mailboxes[0].MailBoxName("Win");
        else if (_mailBoxes.Count == 2)
        {
            PlaceBox("Win");
            PlaceBox("Spam");
            //_mailboxes[0].MailBoxName("Win");
            //_mailboxes[1].MailBoxName("Spam");
        }
        else if (_mailBoxes.Count >= 3)
        {
            PlaceBox("Win");
            PlaceBox("Spam");

            
            for (int i = 0; i < _mailBoxes.Count - 2; i++)
            {
                if(i < defeatTableName.Length)
                {
                    PlaceBox(defeatTableName[i]);
                }
            }

            // spam = Random.Range(0, _mailboxes.Count);
            // _mailboxes[spam].MailBoxName("Spam");
            // _mailboxes.RemoveAt(spam);

            //win = Random.Range(0, _mailboxes.Count);
            // _mailboxes[win].MailBoxName("Win");
            // _mailboxes.RemoveAt(win);
            // for (int i = 0; i < _mailboxes.Count; i++)
            // {
            //     int number = Random.Range(0, _mailboxes.Count);

            //     Debug.Log("test : " + number + " " + _mailboxes.Count);
            //     _mailboxes[number].MailBoxName(defeatTableName[i]);
            // }
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
        _soundManager.StartAudio();
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
        if(_mailBoxes[random].nameBox == "" || _mailBoxes[random].nameBox == null)
        {
            _mailBoxes[random].MailBoxName(name);
                
            if(name == "Win")
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

    public void PlayerHasDeliveredTheMail(string boxDelivered) {
        _player.StopPlayer();
        if(boxDelivered == "Spam")
        {
            if (_player.isPlayerInfected()) points += 100;
            perfectDelivery = false;
            _endingManager.Spam();
        }
        else if (boxDelivered == "Win")
        {
            points += 200;
            if (_player.isPlayerInfected()) _endingManager.Infected();
            else _endingManager.Win();
        } else if (boxDelivered == "Dad")
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
        _scoreText.text = points.ToString();
    }

    public void LevelOver()
    {
        if (SceneManager.GetActiveScene().name == "Level13") GameOver();
        else _levelManager.LoadNextLevel();

    }

    public void GameOver()
    {
        gameIsPlaying = false;
        _levelManager.LoadGameOver();
    }
    #endregion
}
