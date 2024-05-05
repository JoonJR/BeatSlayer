using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 
using System.Collections;
using TMPro;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private InputActionReference menuInputActionReference;
    [SerializeField] private TextMeshProUGUI countdownText;
    public GameObject pauseMenuUI;

    private CubeSpawner cubeSpawner;
    private bool isCountingDown = false;
    private void Start()
    {
        countdownText.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        menuInputActionReference.action.started += MenuPressed;
    }

    private void OnDisable()
    {
        menuInputActionReference.action.started -= MenuPressed;

    }

    private void MenuPressed(InputAction.CallbackContext context)
    {
        TogglePause();
        
    }
    public void TogglePause()
    {
        if (isCountingDown)  
        {
            return;  
        }
        if (pauseMenuUI.activeInHierarchy)
        {
            StartCoroutine(ResumeWithDelay(3));
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        cubeSpawner = FindObjectOfType<CubeSpawner>();
        if (cubeSpawner != null)
        {
            cubeSpawner.PauseSpawning();
        }
        else
        {
            Debug.LogError("CubeSpawner not found!");
        }
       // ControllerManager.Instance.EnableAllInteractors();
        Time.timeScale = 0f; // Pause the game
        AudioManager.Instance.musicSource.pitch = 0;
    }

    IEnumerator ResumeWithDelay(int delay)
    {
        isCountingDown = true;
        countdownText.gameObject.SetActive(true);
        pauseMenuUI.SetActive(false);
        // Update the Text component to count down
        for (int i = delay; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1);
            }

       
        countdownText.gameObject.SetActive(false);
        // Resume the game

        isCountingDown = false;
       // ControllerManager.Instance.DisableAllInteractors();
        if (cubeSpawner != null)
        {
            cubeSpawner.ResumeSpawning();
        }
        Time.timeScale = 1f;
        AudioManager.Instance.musicSource.pitch = 1;
    }
}
