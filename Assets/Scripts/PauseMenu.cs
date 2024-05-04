using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
/*
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button mainMenuButton;
    public XRController leftController; // Reference to the left XR controller

    private void Start()
    {
        pauseMenuUI.SetActive(false); // Start with the pause menu hidden

        // Add listener for main menu button
        mainMenuButton.onClick.AddListener(() => {
            SceneManager.LoadScene("MainMenu");
        });

        // Add listener for resume button with delay
        resumeButton.onClick.AddListener(() => StartCoroutine(ResumeWithDelay(3)));
    }

    private void Update()
    {
        if (leftController)
        {
            if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButtonPressed) && menuButtonPressed)
            {
                TogglePause();
            }
        }
    }

    void TogglePause()
    {
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
        Time.timeScale = 0f; // Pause the game
    }

    IEnumerator ResumeWithDelay(int delay)
    {
        resumeButton.interactable = false; // Disable the button while waiting

        // Update the button's text to count down
        for (int i = delay; i > 0; i--)
        {
            resumeButton.GetComponentInChildren<Text>().text = "Resume (" + i + ")";
            yield return new WaitForSecondsRealtime(1);
        }

        resumeButton.GetComponentInChildren<Text>().text = "Resume";
        resumeButton.interactable = true;

        // Resume the game
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
}*/