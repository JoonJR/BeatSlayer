using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour // More like gamemanager but too lazy to move them anymore i think... 
{
    private static ScoreManager _instance;
    public static ScoreManager Instance => _instance;

    public int score = 0;
    public int combo = 0;
    public int multiplier = 1;

    public int missCount = 0;  // Track consecutive misses
    public int maxAllowedMisses = 100;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;

    public GameObject gameOverUI; 
    public GameObject pauseMenuUI; 
    public GameObject winMenuUI;

    public TextMeshProUGUI finalScoreText;


    

    private CubeSpawner cubeSpawner;
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
    void Start()
    {
        QualitySettings.vSyncCount = 0;  // Disable VSync
        Application.targetFrameRate = 90;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handling UI elements based on the scene
        if (scene.name == "MainMenu" && scoreText != null)
        {
            scoreText.enabled = false; // Hide score text in the main menu

        }
        if (scene.name == "RainingBloodTeacher" | scene.name == "RainingBloodJoona" | scene.name == "RainingBloodHaveFun))")
        {
            //ControllerManager.Instance.DisableAllInteractors();
            //leftRayController = GameObject.FindGameObjectWithTag("LeftController").GetComponent<RayController>();
            //rightRayController = GameObject.FindGameObjectWithTag("LeftController").GetComponent<RayController>();
            //leftRayController.DisableRayInteractor();
            //rightRayController.DisableRayInteractor();
            FindUIElements();
            ResetGame();
            UpdateScoreUI();
        }

    }
    // Method to find and assign the scoreText UI element
    public void FindUIElements()
    {
        winMenuUI = GameObject.FindGameObjectWithTag("WinMenuUI");
        if (winMenuUI != null)
        {
            GameObject finalScoreTextObj = GameObject.FindGameObjectWithTag("FinalScoreText");
            finalScoreText = FindTextComponent(finalScoreTextObj, "FinalScoreText");
        }
            
        winMenuUI.SetActive(false);
        gameOverUI = GameObject.FindGameObjectWithTag("GameOverUI");
        gameOverUI.SetActive(false);
        pauseMenuUI = GameObject.FindGameObjectWithTag("PauseMenuUI");
        pauseMenuUI.SetActive(false);

        GameObject scoreTextObj = GameObject.FindGameObjectWithTag("ScoreText");
        GameObject comboTextObj = GameObject.FindGameObjectWithTag("ComboText");
        GameObject multiplierTextObj = GameObject.FindGameObjectWithTag("MultiplierText");

        // Attempt to get the TextMeshProUGUI component from each GameObject
        scoreText = FindTextComponent(scoreTextObj, "ScoreText");
        comboText = FindTextComponent(comboTextObj, "ComboText");
        multiplierText = FindTextComponent(multiplierTextObj, "MultiplierText");
    }

    // Helper method to get the TextMeshProUGUI component and handle errors
    private TextMeshProUGUI FindTextComponent(GameObject obj, string tagName)
    {
        if (obj != null)
        {
            TextMeshProUGUI textComponent = obj.GetComponent<TextMeshProUGUI>();

            if (textComponent != null)
            {
                return textComponent;
            }
            else
            {
                Debug.LogError($"TextMeshProUGUI component not found on {tagName} object");
            }
        }
        else
        {
            Debug.LogError($"{tagName} object not found");
        }
        return null;
    }
    void UpdateScoreUI()
    {
        scoreText.text = "Score \n" + score;
        comboText.text = "Combo \n" + combo;
        multiplierText.text = "Multiplier \nx" + multiplier;
    }
    public void ResetCombo()
    {
        combo = 0;
        multiplier = 1;
        UpdateScoreUI();
    }

    public void AddScore(int basePoints)
    {
        if (combo > 0)
        {
            multiplier = DetermineMultiplier(combo);
        }
        else
        {
            multiplier = 1;
        }
        missCount = 0;
        score += basePoints * multiplier;
        UpdateScoreUI();
    }


    public void IncrementCombo()
    {
        combo++;
        UpdateScoreUI();
    }
    int DetermineMultiplier(int combo)
    {
        return 1 + (combo / 5);  // Increase multiplier by 1 every 5 combos
    }
    public void MissedNote()
    {
        missCount++;
        if (missCount >= maxAllowedMisses)
        {
            if (gameOverUI != null)
            {
                GameOver();
                gameOverUI.SetActive(true);

            }
            else
            {
                Debug.LogError("GameOverUI is missing!");
            }
        }
    }
    void GameOver()
    {
        //if (ControllerManager.Instance != null)
        //ControllerManager.Instance.EnableAllInteractors();
        // Stop spawning new cubes
        
        FindObjectOfType<CubeSpawner>().StopSpawning();
        ApplyGravityToCubes();
        ControllerManager controllerManager = FindObjectOfType<ControllerManager>();
        controllerManager.DisableSabersColliders();
        // Find the XR Rig and enable gravity
        GameObject xrRig = GameObject.FindGameObjectWithTag("XRRig"); // Make sure the XR Rig is tagged properly
        XRGravityManager gravityManager = xrRig.GetComponent<XRGravityManager>();
        if (gravityManager != null)
        {
            gravityManager.EnableGravity();
        }
        else
        {
            Debug.LogError("XRGravityManager component not found on the XR Rig!");
        }

        // Slow down time
        StartCoroutine(SlowTimeAndMusic(1.3f));
    }
    private void ApplyGravityToCubes()
    {
        var cubes = FindObjectsOfType<MoveTowardsPlayer>();
        foreach (var cube in cubes)
        {
            cube.speed = 0;
            var rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                new WaitForSeconds(1.5f);
                //Destroy(cube.gameObject); // Ensure objects are destroyed correctly.
            }
        }
    }
    public void ResetGame()
    {
        StopAllCoroutines();  // Stop ongoing coroutine SlowTimeAndMusic

        Time.timeScale = 1;   // Reset time scale immediately
        if (AudioManager.Instance != null && AudioManager.Instance.musicSource != null)
        {
            AudioManager.Instance.musicSource.pitch = 1;  // Reset music pitch immediately
        }
        missCount = 0;        // Reset miss count
        score = 0;            // Reset score
        combo = 0;            // Reset combo
        multiplier = 1;       // Reset multiplier
        UpdateScoreUI();      // Update the UI to reflect the reset
        gameOverUI.SetActive(false);
        pauseMenuUI.SetActive(false);

        cubeSpawner = FindObjectOfType<CubeSpawner>();
        if (cubeSpawner != null)
        {
            cubeSpawner.stopSpawning = false;
        }
        else
        {
            Debug.LogError("CubeSpawner not found!");
        }
    }
    public void DisplayWinUI()
    {
        
        finalScoreText.text = $"Final Score: {score}"; // Update the final score text
        winMenuUI.SetActive(true); // Show win menu UI
        gameOverUI.SetActive(false); // Ensure game over UI is not shown
        StartCoroutine(SlowTimeAndMusic(0));
       // if (ControllerManager.Instance != null)
           // ControllerManager.Instance.EnableAllInteractors();
    }

    IEnumerator SlowTimeAndMusic(float delay)
    {
        yield return new WaitForSeconds(delay);
        float startPitch = AudioManager.Instance.musicSource.pitch;
        float duration = 2f;  // Duration over which to slow down
        float elapsed = 0;

        while (elapsed < duration)
        {
            // Slow down time and music pitch proportionally
            float newTimeScale = Mathf.Lerp(1, 0, elapsed / duration);
            Time.timeScale = newTimeScale;
            AudioManager.Instance.musicSource.pitch = Mathf.Lerp(startPitch, 0.1f, elapsed / duration);

            elapsed += Time.unscaledDeltaTime;  // Use unscaledDeltaTime so the slowdown isn't affected by Time.timeScale
            yield return null;
        }

        Time.timeScale = 0f;  // Stop the game completely
        AudioManager.Instance.musicSource.pitch = 0;  // Stop the music completely
    }
}