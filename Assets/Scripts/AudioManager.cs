using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip slayMusic;
    public AudioClip rölliMusic;
    public AudioClip sliceEffect;
    public List<AudioClip> missEffects; // List of miss effect clips

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded scene: " + scene.name);

        if (scene.name == "RainingBloodTeacher")
        {
            Debug.Log("Playing scene-specific music");
            PlayMusic(rölliMusic);
        }
        else if (scene.name == "RainingBloodJoona" || scene.name == "RainingBloodHaveFun))")
        {
            Debug.Log("Playing Slayer music");
            PlayMusic(slayMusic);
        }
        else if (scene.name == "MainMenu" || scene.name == "DebugScene")
        {
            StopMusic();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySliceEffect()
    {
        sfxSource.PlayOneShot(sliceEffect);
    }

    public void PlayMissEffect()
    {
        if (missEffects.Count > 0)
        {
            int randomIndex = Random.Range(0, missEffects.Count);
            sfxSource.PlayOneShot(missEffects[randomIndex]);
            //Debug.Log("Playing " + missEffects[randomIndex]);
        }
    }
}
