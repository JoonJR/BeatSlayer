using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false; // Tracks if the game is currently paused
    public GameObject pauseMenuUI; // Reference to the pause menu UI 
    public GameObject gameWonUI; // Reference to the game won UI
    public GameObject gameOverUI; // Reference to the game over UI 
    public TextMeshProUGUI finalScoreText; // Text element for displaying the finawl score
    public TextMeshProUGUI gameOverScoreText; // Text element for displaying the game over score

    private static PauseMenu _instance;
    public static PauseMenu Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Handle pause menu activation/deactivation
        if (SceneManager.GetActiveScene().name != "MainMenu" & SceneManager.GetActiveScene().name != "Difficulty")
        {
            if (Input.GetKeyDown(KeyCode.Escape)) // Left controller start button
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        /*
        // If we are in last level and all bricks are destroyed, game is over. 
        if (SceneManager.GetActiveScene().name == "Level4" && BrickManager.Instance.AreAllBricksDestroyed() && !highScoreAdded)
        {
            int finalScore = ScoreManager.Instance.score;
            HighScoreManager.Instance.AddHighScore(finalScore);
            BallsManager.Instance.isInPlay = false;
            highScoreAdded = true;
            gameWonUI.SetActive(true);
            GameIsPaused = true;
            pauseMenuUI.SetActive(false);
            UpdateFinalScoreUI();
            ScoreManager.Instance.scoreText.enabled = false;
        }*/

        // Handle UI updates for different scenes
        if (SceneManager.GetActiveScene().name == "RainingBlood")
        {
            ScoreManager.Instance.scoreText.enabled = true;
            ScoreManager.Instance.comboText.enabled = true;
            ScoreManager.Instance.multiplierText.enabled = true;
        }

        // Disable 
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            
            //highScoreAdded = false;
            gameWonUI.SetActive(false);
            gameOverUI.SetActive(false);

        }

        /*// Handle game over scenario
        if (ScoreManager.Instance.lives == 0 && !highScoreAdded)
        {
            int finalScore = ScoreManager.Instance.score;
            HighScoreManager.Instance.AddHighScore(finalScore);
            highScoreAdded = true;
            gameOverUI.SetActive(true);
            pauseMenuUI.SetActive(false);
            UpdateGameOverScoreUI();
            GameIsPaused = true;
            ScoreManager.Instance.scoreText.enabled = false;
        }*/
    }

    // Resume the game from pause
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Pause the game
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void UpdateFinalScoreUI()
    {
        FindFinalScoreUI();
        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + ScoreManager.Instance.score;
        }
    }
    private void FindFinalScoreUI()
    {
        GameObject finalScoreTextObj = GameObject.FindGameObjectWithTag("FinalScoreText");
        if (finalScoreTextObj != null)
        {
            finalScoreText = finalScoreTextObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("Score Text object not found");
        }
    }
    public void UpdateGameOverScoreUI()
    {
        FindGameOverScoreUI();
        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = "Score: " + ScoreManager.Instance.score;
        }
    }
    private void FindGameOverScoreUI()
    {
        GameObject gameOverScoreTextObj = GameObject.FindGameObjectWithTag("GameOverScoreText");
        if (gameOverScoreTextObj != null)
        {
            finalScoreText = gameOverScoreTextObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("GameOver Text object not found");
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    // When going to mainmenu reset everything.
    public void LoadMainMenu()
    {

        Resume();
        pauseMenuUI.SetActive(false);
        ScoreManager.Instance.scoreText.enabled = false;
        ScoreManager.Instance.score = 0;
        SceneManager.LoadScene("MainMenu");
    }

}