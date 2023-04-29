using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] playlist;
    public AudioSource audioSource;

    public static SoundManager instance;


    public void Awake()
    {
        if(instance != null) return;

    }
    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    void Update()
    {
        
    }

}
