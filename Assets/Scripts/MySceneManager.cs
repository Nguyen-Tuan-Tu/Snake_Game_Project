using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager Instance;
    public GameObject pauseUI;
    public GameObject blurPanel;

    public GameObject gameOverPanel;

    public GameObject VictoryPanel;

    public GameObject ReadyPanel;

    public TextMeshProUGUI CountDownText;

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
    public void ShowReadPanel()
    {
        ReadyPanel.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ReadyGame()
    {
        ReadyPanel.SetActive(false);
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        CountDownText.gameObject.SetActive(true);
        CountDownText.text = "3";
        yield return new WaitForSecondsRealtime(1f);
        CountDownText.gameObject.SetActive(false);

        CountDownText.gameObject.SetActive(true);
        CountDownText.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        CountDownText.gameObject.SetActive(false);

        CountDownText.gameObject.SetActive(true);
        CountDownText.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        CountDownText.gameObject.SetActive(false);

        CountDownText.gameObject.SetActive(true);
        CountDownText.text = "GO";
        yield return new WaitForSecondsRealtime(0.5f);
        CountDownText.gameObject.SetActive(false);

        Time.timeScale = 1f;
        AudioManager.Instance.UnmuteBGM();
    }
}
