using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    
    public AudioClip backgroundClip;
    public float bgVolume = 0.5f;
    public bool playBGOnStart = true;

    
    public AudioClip flipClip;
    public AudioClip matchClip;
    public AudioClip mismatchClip;   

    public float sfxVolume = 1f;

    AudioSource bgSource;     
    AudioSource sfxSource;    

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        // Create background audio source
        bgSource = gameObject.AddComponent<AudioSource>();
        bgSource.loop = true;
        bgSource.playOnAwake = false;
        bgSource.volume = bgVolume;

        // Create SFX source
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.loop = false;
        sfxSource.volume = sfxVolume;
    }

    void Start()
    {
        if (playBGOnStart && backgroundClip != null)
        {
            PlayBackground();
        }
    }

    public void PlayBackground()
    {
        if (backgroundClip == null) return;
        bgSource.clip = backgroundClip;
        bgSource.volume = bgVolume;
        bgSource.Play();
    }

    public void StopBackground() => bgSource.Stop();

    public void SetBGVolume(float volume)
    {
        bgVolume = Mathf.Clamp01(volume);
        bgSource.volume = bgVolume;
    }

    public void PlayFlip()
    {
        if (flipClip == null) return;
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(flipClip);
    }

    public void PlayMatch()
    {
        if (matchClip == null) return;
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(matchClip);
    }

    public void PlayMismatch()
    {
        if (mismatchClip == null) return;
        sfxSource.volume = sfxVolume;
        sfxSource.PlayOneShot(mismatchClip);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }
}
