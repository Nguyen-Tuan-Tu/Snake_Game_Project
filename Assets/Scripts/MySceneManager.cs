using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;
    public GameObject pauseUI;
    public GameObject blurPanel;

    public GameObject gameOverPanel;

    public GameObject VictoryPanel;

    private void Awake()
    {
        if(Instance == null ) Instance = this;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
        blurPanel.SetActive(true);
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        blurPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        VictoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
