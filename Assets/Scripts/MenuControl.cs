using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    
    public void LoadRainingBloodTeacher()
    {
        
        SceneManager.LoadScene("RainingBloodTeacher");
        
    }
    public void LoadRainingBloodJoona()
    {
        
        SceneManager.LoadScene("RainingBloodJoona");
        
    }
    public void ReloadRainingBloodJoona()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetGame();
        }

        SceneManager.LoadScene("RainingBloodJoona");

    }
    public void ReloadRainingBloodTeacher()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetGame();
        }
       
        SceneManager.LoadScene("RainingBloodTeacher");
        
    }
    public void LoadRainingBloodHaveFun()
    {

        SceneManager.LoadScene("RainingBloodHaveFun))");

    }
    public void ReloadRainingBloodHaveFun()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetGame();
        }

        SceneManager.LoadScene("RainingBloodHaveFun))");

    }
    public void LoadDebug()
    {
        SceneManager.LoadScene("DebugScene");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

        
        ScoreManager.Instance.gameOverUI.SetActive(false);
    }
    public void LoadMainMenuFromDebug()
    {
        
        SceneManager.LoadScene("MainMenu");


        
    }
    public void ReloadMainMenu()
    {

        ScoreManager.Instance.ResetGame();
        SceneManager.LoadScene("MainMenu");
        

        ScoreManager.Instance.gameOverUI.SetActive(false);
    }
    public void LoadDifficulty()
    {
        SceneManager.LoadScene("Difficulty");
    }
    public void LoadHiScores()
    {
        SceneManager.LoadScene("HiScores");
    }
 
    public void Quit()
    {
        Application.Quit();
    }
    

}