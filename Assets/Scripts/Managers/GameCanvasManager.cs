using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject _nudeAnim;
    [SerializeField] GameManager _gameManager;
    [SerializeField] GameObject _playerGFX;


    [Header("Audio")]
    [SerializeField] AudioClip _photo;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _stampSound;

    private void Start()
    {
        _audioSource = GameObject.FindAnyObjectByType<AudioSource>();
        LaunchNudeAnim();
    }

    public void LaunchNudeAnim()
    {
        _audioSource.PlayOneShot(_photo);
        _nudeAnim.SetActive(true);
    }

    public void AnimationOver()
    {
        StartGame();
    }

    public void PlayStampSound()
    {
        _audioSource.PlayOneShot(_stampSound);
    }

    private void StartGame()
    {
        StartCoroutine(FadeInPlayerFadeOutCanvas());
    }

    IEnumerator FadeInPlayerFadeOutCanvas()
    {
        float time = 0;
        Color _playerGFXColor = _playerGFX.GetComponent<SpriteRenderer>().color;
        while (time < 1f)
        {
            time += Time.deltaTime;
            _playerGFXColor.a = Mathf.Lerp(_playerGFXColor.a, 1f, time / 1f);
            _playerGFX.GetComponent<SpriteRenderer>().color = _playerGFXColor;
            yield return null;
        }
        if (time >= 1)
        {
            _gameManager.StartPlaying();
            _gameManager.SceneHasChanged();
            _nudeAnim.SetActive(false);

        }
    }
}
