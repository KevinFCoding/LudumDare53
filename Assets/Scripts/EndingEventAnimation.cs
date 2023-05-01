using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingEventAnimation : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    public void Win()
    {
        Invoke("InvokeWin", 2);
    }
    public void Lose()
    {
        Invoke("InvokeLose", 2);
    }

    private void InvokeWin()
    {
        _gameManager.LevelOver();
    }

    private void InvokeLose()
    {
        _gameManager.GameOver();
    }
}
