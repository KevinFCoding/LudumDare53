using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelManager _levelManager;
    [SerializeField] SoundManager _soundManager;
    [SerializeField] Player _player;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void StartPlaying()
    {
        _levelManager.LoadLevelOne();
    }
}
