using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _scoreGainedText;

    [SerializeField] private Text _currentLevelText;

    private Animator _animator;


    private void Start()
    {
        GameManager.Instance.OnScoreGained += GameManager_OnScoreGained;
        GameManager.Instance.OnNextLevel += GameManager_OnNextLevel;
        _scoreText.text = "Score : 0";
        _scoreGainedText.text = "";
        _animator = GetComponentInChildren<Animator>();
    }

    private void GameManager_OnScoreGained(object sender, GameManager.OnScroreGainedEventArgs e)
    {
        _scoreText.text = "Score : " + e.totalScore.ToString();
        _scoreGainedText.text = "+ " + e.scoreGained.ToString();
        _animator.SetTrigger("ScoreGained");
    }

    private void GameManager_OnNextLevel(object sender, System.EventArgs e)
    {
        _currentLevelText.text = "Level " + GameManager.Instance.CurrentLevelNumber;
    }
}
