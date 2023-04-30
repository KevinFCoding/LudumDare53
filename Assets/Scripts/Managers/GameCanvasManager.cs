using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class GameCanvasManager : MonoBehaviour
{
    [SerializeField] GameObject _nudeAnim;


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
}
