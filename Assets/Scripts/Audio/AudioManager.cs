using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; 

    public AudioSource musicSource; 
    public AudioSource sfxSource;   

    public AudioClip[] musicClips;  
    public AudioClip[] sfxClips;

    void Start()
    {
        PlayMusic("menu_song");
    }

    public void PlayMusicClip(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }  

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(string name)
    {
        AudioClip clip = GetClip(name, musicClips);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        AudioClip clip = GetClip(name, sfxClips);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    private AudioClip GetClip(string name, AudioClip[] clips)
    {
        foreach (AudioClip clip in clips)
        {
            if (clip.name == name)
            {
                return clip;
            }
        }
        Debug.LogWarning("Audio clip not found: " + name);
        return null;
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
        sfxSource.volume = volume;
    }

}
