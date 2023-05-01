using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    public AudioClip[] playlist;
    public AudioSource audioSource;
    public AudioMixerGroup soundEffectMixer;
    public static SoundManager instance;


    public void Awake()
    {
        if(instance != null) return;
        DontDestroyOnLoad(gameObject);

    }

    public void StartAudio()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    public AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = pos;
        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = soundEffectMixer;
        audioSource.Play();
        Destroy(tempGO, clip.length);
        return audioSource;
    }

}
