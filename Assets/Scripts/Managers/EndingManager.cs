using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public SoundManager audioManager;
    public AudioSource audioSource;
    public AudioClip winSound;
    public AudioClip looseSound;
    public AudioClip looseGPSound;
    public AudioClip youGotMail;
    public AudioClip looseBrotherSound;
    public AudioClip stampSound;

    [Header("Spam")]
    [SerializeField] GameObject _spamParticules;
    [SerializeField] GameObject _spamPanel;

    [Header("Victory")]

    [SerializeField] GameObject _winParticules;
    [SerializeField] GameObject _victoryPanel;
    [SerializeField] GameObject _victoryImage;


    [Header("Defeate")]

    [SerializeField] GameObject _looseParticules;
    [SerializeField] GameObject _loosePanel;
    [SerializeField] GameObject _looseImage;

    [Header("GrampaDefeate")]

    [SerializeField] GameObject _looseGPParticules;
    [SerializeField] GameObject _looseGPPanel;
    [SerializeField] GameObject _looseGPImage;
    
    [Header("BrotherDefeat")]

    [SerializeField] GameObject _brotherParticules;
    [SerializeField] GameObject _brotherPanel;
    [SerializeField] GameObject _brotherImage;

    void Start()
    {

        // print("Start");
        //  Win();
        StartCoroutine(ActiveSpamPanel());
    }

    void Update()
    {
        
    }

    public void Loose()
    {
        audioSource.PlayOneShot(youGotMail, 5);

        _loosePanel.SetActive(true);
        StartCoroutine(ActiveEndingLoosePanel());
    }

    public void LooseGrampa()
    {
        audioSource.PlayOneShot(youGotMail, 5);

        _looseGPPanel.SetActive(true);
        StartCoroutine(ActiveEndingLooseGPPanel());
    }
    public void LooseBrother()
    {
        audioSource.PlayOneShot(youGotMail, 5);

        _brotherPanel.SetActive(true);
        StartCoroutine(ActiveEndingLooseBrotherPanel());
    }

    public void Win()
    {
        audioSource.PlayOneShot(youGotMail, 5);

        _victoryPanel.SetActive(true);
        StartCoroutine(ActiveEndingWinPanel());
    }

    IEnumerator ActiveSpamPanel()
    {
        _spamPanel.SetActive(true);


        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(stampSound, 2);
        yield return new WaitForSeconds(0.5f);
        _spamParticules.SetActive(true);

        yield break;
    }
    IEnumerator ActiveEndingLoosePanel()
    {
        yield return new WaitForSeconds(1.6f);
        _looseImage.SetActive(true);
        audioSource.PlayOneShot(looseSound, 10);

        yield return new WaitForSeconds(0.5f);

        _looseParticules.SetActive(true);

        yield break;
    }

    IEnumerator ActiveEndingLooseGPPanel()
    {
        yield return new WaitForSeconds(1.6f);
        _looseGPImage.SetActive(true);
        audioSource.PlayOneShot(looseGPSound, 5);

        yield return new WaitForSeconds(0.5f);

        _looseGPParticules.SetActive(true);

        yield break;
    }

    IEnumerator ActiveEndingLooseBrotherPanel()
    {
        yield return new WaitForSeconds(1.6f);
        _brotherImage.SetActive(true);
        audioSource.PlayOneShot(looseBrotherSound, 5);

        yield return new WaitForSeconds(0.5f);

        _brotherParticules.SetActive(true);

        yield break;
    }
    IEnumerator ActiveEndingWinPanel()
    {
        yield return new WaitForSeconds(1.6f);
        _victoryImage.SetActive(true);
        audioManager.PlayClipAt(winSound, transform.position);

        yield return new WaitForSeconds(0.5f);
        _winParticules.SetActive(true);

        yield break;
    }

}
