using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void SetSpeedSlow()
    {
        GameSettings.snakeSpeed = 0.3f;
        StartGame();
    }
    public void SetSpeedMedium()
    {
        GameSettings.snakeSpeed = 0.12f;
        StartGame();
    }
    public void SetSpeedFast()
    {
        GameSettings.snakeSpeed = 0.05f;
        StartGame();
    }

    private void StartGame() {
        SceneManager.LoadScene("GameScene"); 
    }
}
