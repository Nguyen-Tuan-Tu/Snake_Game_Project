using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite greenSprite; // Kéo '22Button_Midl_Green' vào đây
    public Sprite redSprite;   // Kéo '23Button_Midl_Red' vào đây

    [Header("Buttons")]
    public Image slowBtnImg;
    public Image mediumBtnImg;
    public Image fastBtnImg;

    // Hàm này tự chạy MỖI KHI bảng Select Level được bật lên (SetActive(true)) [cite: 2026-03-31]
    void OnEnable()
    {
        if (AlphabetManager.Instance != null)
        {
            // Lấy cái tên level đang lưu trong AlphabetManager để tô màu
            string savedLevel = AlphabetManager.Instance.currentLevelName;
            UpdateButtonClickUI(savedLevel);
            Debug.Log("Đã cập nhật màu nút theo mức: " + savedLevel);
        }
    }
    public void SelectLevel(string level)
    {
        // 1. Lưu vào AlphabetManager để sang GameScene dùng
        if (AlphabetManager.Instance != null)
        {
            AlphabetManager.Instance.currentLevelName = level;
        }

        // 2. Cập nhật màu sắc nút bấm [cite: 2026-03-31]
        UpdateButtonClickUI(level);
    }
    public void UpdateButtonClickUI(string level)
    {
        // 1. Reset tất cả về màu xanh trước
        slowBtnImg.sprite = greenSprite;
        mediumBtnImg.sprite = greenSprite;
        fastBtnImg.sprite = greenSprite;

        // 2. Nút nào được chọn thì đổi sang màu đỏ
        switch (level)
        {
            case "Slow":
                slowBtnImg.sprite = redSprite;
                GameSettings.snakeSpeed = 0.3f;
                break;
            case "Medium":
                mediumBtnImg.sprite = redSprite;
                GameSettings.snakeSpeed = 0.12f;
                break;
            case "Fast":
                fastBtnImg.sprite = redSprite;
                GameSettings.snakeSpeed = 0.05f;
                break;
        }
    }
}
