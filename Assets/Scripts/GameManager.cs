using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
/*
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            HandleMainMenuLoaded();
        }
        else if (scene.name == "RainingBloodTeacher" || scene.name == "RainingBloodJoona")
        {
            PrepareGameScene();
        }
    }

    private void HandleMainMenuLoaded()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        ScoreManager.Instance.ResetGame();
        ControllerManager.Instance.EnableAllInteractors();
    }

    private void PrepareGameScene()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        ScoreManager.Instance.FindUIElements();
        ScoreManager.Instance.UpdateScoreUI();
        ControllerManager.Instance.DisableAllInteractors();
    }

    public void GameOver()
    {
        if (gameOverUI != null) gameOverUI.SetActive(true);
        ScoreManager.Instance.ResetCombo();
        ControllerManager.Instance.EnableAllInteractors();
        CubeSpawner.Instance.StopSpawning();
        ApplyGravityToCubes();
        StartCoroutine(SlowTime());
        StartCoroutine(SlowTimeAndMusic());
    }

    private void ApplyGravityToCubes()
    {
        MoveTowardsPlayer[] cubes = FindObjectsOfType<MoveTowardsPlayer>();
        foreach (var cube in cubes)
        {
            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                Destroy(cube.gameObject, 2.0f); // Destroys the cube after 2 seconds
            }
        }
    }

    IEnumerator SlowTime()
    {
        float targetTimeScale = 0.1f;
        while (Time.timeScale > targetTimeScale)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.deltaTime * 0.5f);
            yield return null;
        }
    }

    IEnumerator SlowTimeAndMusic()
    {
        float duration = 1.5f;
        float startPitch = AudioManager.Instance.musicSource.pitch;
        float elapsed = 0;

        while (elapsed < duration)
        {
            AudioManager.Instance.musicSource.pitch = Mathf.Lerp(startPitch, 0.1f, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        AudioManager.Instance.musicSource.pitch = 0;
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }
}

*/