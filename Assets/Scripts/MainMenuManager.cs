using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject selectPanel;

    public GameObject introductionPanel;
    //__________HÀM BẮT ĐẦU GAME__________
    public void StartGame()
    {
        Time.timeScale = 1f;
        
        // ÉP Reset ngay tại đây cho chắc ăn [cite: 2026-03-23]
        if (AlphabetManager.Instance != null)
        {
            AlphabetManager.Instance.ResetProgressForNewGame();
        }

        SceneManager.LoadScene("GameScene");
    }
    //_________HÀM THOÁT GAME__________
    public void ExitGame()
    {
        Application.Quit();
    }
    //_________HÀM SỰ KIỆN KHI ẤN NÚT SELECT LEVEL__________
    public void OpenSelectLevel()
    {
        mainPanel.SetActive(false);
        selectPanel.SetActive(true);
    }
    //_________HÀM SỰ KIỆN KHI ẤN NÚT BACK TẠI SELECT LEVEL__________
    public void BackToMainMenu()
    {
        mainPanel.SetActive(true);
        selectPanel.SetActive(false);
        introductionPanel.SetActive(false);
    }

    public void OpenIntroduction()
    {
        introductionPanel.SetActive(true);
    }
}
