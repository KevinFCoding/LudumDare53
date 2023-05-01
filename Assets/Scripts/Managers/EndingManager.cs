using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public SoundManager _soundManager;
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


    [Header("Defeat")]

    [SerializeField] GameObject _looseParticules;
    [SerializeField] GameObject _loosePanel;
    [SerializeField] GameObject _looseImage;

    [Header("GrampaDefeat")]

    [SerializeField] GameObject _looseGPParticules;
    [SerializeField] GameObject _looseGPPanel;
    [SerializeField] GameObject _looseGPImage;
    
    [Header("BrotherDefeat")]

    [SerializeField] GameObject _brotherParticules;
    [SerializeField] GameObject _brotherPanel;
    [SerializeField] GameObject _brotherImage;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _soundManager = GameObject.FindAnyObjectByType<SoundManager>();
        audioSource = GameObject.FindAnyObjectByType<AudioSource>();
    }

    public void GetAllParticles()
    {
        //_winParticules = GameObject.FindObjectsOfTypeAll<>();
        GameObject FindVlop = GameObject.FindObjectOfType<Player>().gameObject;
        GameObject findParticles = FindVlop.transform.GetChild(FindVlop.transform.childCount - 1).gameObject; ;
        Debug.Log("PARTICULES " + findParticles.name + " is empty");
        //GameObject findParticles = FindVlop.transform.GetChild(FindVlop.transform.GetChildCount() - 1).gameObject;
        //GameObject[] ChildNames = findParticles.GetComponentsInChildren<GameObject>(includeInactive: true);
        ParticleSystem[] ChildNames = findParticles.GetComponentsInChildren<ParticleSystem>(includeInactive: true);

        Debug.Log("CHILD NAMES AAA " + ChildNames);
        Debug.Log("CHILD NAMES LENGTH  AAA" + ChildNames.Length);
        for (int i = 0; i < ChildNames.Length; i++)
        {
            string name = ChildNames[i].gameObject.name;
            Debug.Log("NAME " + name);
            switch (name)
            {
                case "Win_part":
                    _winParticules = ChildNames[i].gameObject;
                    break;
                case "Spam_part":
                    _spamParticules = ChildNames[i].gameObject;
                    break;
                case "Granny_part":
                     _looseParticules = ChildNames[i].gameObject;
                    break;
                case "Dad_part":
                    _looseGPParticules = ChildNames[i].gameObject;
                    break;
                case "Bro_part":
                    _brotherParticules = ChildNames[i].gameObject;
                    break;
                default:
                    break;
            }
        }
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
    public void Spam()
    {
        audioSource.PlayOneShot(stampSound, 5);

        _spamPanel.SetActive(true);
        StartCoroutine(ActiveSpamPanel());
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
        _soundManager.PlayClipAt(winSound, transform.position);

        yield return new WaitForSeconds(0.5f);
        _winParticules.SetActive(true);

        yield break;
    }

}
