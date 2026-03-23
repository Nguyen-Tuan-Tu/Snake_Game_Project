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

    public TextMeshProUGUI keywordText;

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

        // QUAN TRỌNG: Trước khi load lại, bảo AlphabetManager reset tiến trình của TỪ HIỆN TẠI
        if (AlphabetManager.Instance != null)
        {
            // Chúng ta sẽ tạo một hàm mới tên là ResetCurrentWord để không làm xáo trộn danh sách từ
            AlphabetManager.Instance.ResetCurrentWord();
        }

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

    public void UpdateKeywordUI(string word)
    {
        Debug.Log("UI nhận được từ khóa: " + word);
        // Nếu dây bị None, code tự đi tìm Object trong Hierarchy
        if (keywordText == null) {
            GameObject go = GameObject.Find("Txt_C_KeyWord");
            if (go != null) keywordText = go.GetComponent<TextMeshProUGUI>();
        }

        if (keywordText != null) {
            keywordText.text = "Challenger Keywords : " + word;
        }
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
    public void OnClickNextLevel()
    {
        // 1. Bốc sẵn từ mới trong danh sách
        if (AlphabetManager.Instance != null) {
            AlphabetManager.Instance.PickNextWordFromList();
        }

        // 2. Load Scene (Khi Scene mới hiện ra, hàm Start của AlphabetManager ở Bước 1 sẽ tự chạy)
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}   
