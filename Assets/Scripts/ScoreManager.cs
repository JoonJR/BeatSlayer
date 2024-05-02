using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour // More like gamemanager but too lazy to move them i think... 
{
    private static ScoreManager _instance;
    public static ScoreManager Instance => _instance;

    public int score = 0;
    public int combo = 0;
    public int multiplier = 1;

    public int missCount = 0;  // Track consecutive misses
    public int maxAllowedMisses = 5;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI multiplierText;

    public GameObject gameOverUI; // Reference to the game over UI 
    public GameObject pauseMenuUI; // Reference to the game over UI 

    private ControllerManager controllerManager;
    
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
   
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Handling UI elements based on the scene
        if (scene.name == "MainMenu" && scoreText != null)
        {
            scoreText.enabled = false; // Hide score text in the main menu

        }
        if (scene.name == "RainingBloodTeacher" | scene.name == "RainingBloodJoona")
        {
            ControllerManager.Instance.DisableAllInteractors();
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
    // Example method to add points
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

    // Call this method from wherever you handle your game logic
    public void IncrementCombo()
    {
        combo++;
        UpdateScoreUI();
    }
    int DetermineMultiplier(int combo)
    {
        return 1 + (combo / 10);  // Increase multiplier by 1 every 10 combos
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
        if (ControllerManager.Instance != null)
            ControllerManager.Instance.EnableAllInteractors();
        // Stop spawning new cubes
        FindObjectOfType<CubeSpawner>()?.StopSpawning();
        ApplyGravityToCubes();

        // Slow down time
        StartCoroutine(SlowTime());
        StartCoroutine(SlowTimeAndMusic());
       

        // Show game over UI
        // Assuming there is a method to activate the game over screen
        //GameOverScreen.Instance.Show(true);
    }
    private void ApplyGravityToCubes()
    {
        var cubes = FindObjectsOfType<MoveTowardsPlayer>();
        foreach (var cube in cubes)
        {
            var rb = cube.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                Destroy(cube.gameObject); // Ensure objects are destroyed correctly.
            }
        }
    }
    public void ResetGame()
    {
        cubeSpawner = FindObjectOfType<CubeSpawner>();
        cubeSpawner.stopSpawning = false;

        missCount = 0;        // Reset miss count
        score = 0;            // Reset score
        combo = 0;            // Reset combo
        multiplier = 1;       // Reset multiplier
        UpdateScoreUI();      // Update the UI to reflect the reset
        Time.timeScale = 1;  

    }
    IEnumerator SlowTime()
    {
        float t = 0.5f;
        while (Time.timeScale > 0.1f)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0, t);
            t += Time.deltaTime;
            yield return null;
        }
        Time.timeScale = 0;  // Freeze time completely
    }
    IEnumerator SlowTimeAndMusic()
    {
        float startPitch = AudioManager.Instance.musicSource.pitch;
        float duration = 1.5f;  // Duration over which to slow down
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