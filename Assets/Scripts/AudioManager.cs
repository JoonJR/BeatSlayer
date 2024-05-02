using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance => _instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip gameMusic;
    public AudioClip sliceEffect;
    public AudioClip missEffect;

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
        Debug.Log("Loaded scene: " + scene.name);  // Check which scene has been loaded
        if (scene.name == "RainingBloodTeacher")
        {
            Debug.Log("Playing game music");
            PlayMusic(gameMusic);

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
        sfxSource.PlayOneShot(missEffect);
    }
}