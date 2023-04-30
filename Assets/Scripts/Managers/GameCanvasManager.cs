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

    private void Start()
    {
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
    private void StartGame()
    {
        StartCoroutine(FadeInPlayerFadeOutCanvas());
    }

    IEnumerator FadeInPlayerFadeOutCanvas()
    {
        float time = 0;
        Color _playerGFXColor = _playerGFX.GetComponent<SpriteRenderer>().color;
        while (time < 1) {
            time += Time.deltaTime;
            _playerGFXColor.a = Mathf.Lerp(_playerGFXColor.a, 1, 1f / time);
            _playerGFX.GetComponent<SpriteRenderer>().color = _playerGFXColor;
            yield return null;
        }
        if (time >= 1)
        {
            _gameManager.SceneHasChanged();
            _nudeAnim.SetActive(false);

         }
    }
}
