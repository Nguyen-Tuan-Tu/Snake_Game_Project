using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    public static WordManager Instance;
    public Transform characterContainer; // Kéo object Character cha của 7 ô kí tự vào đây
    private int currentTargetIndex = 0;  // Biến thứ tự các ô kí tự

    void Awake()
    {
        Instance = this;
    }

    // Hàm gọi khi rắn ăn kí tự
    public void CollectLetter(Sprite letterSprite)
    {
        if(currentTargetIndex < characterContainer.childCount)
        {
            // Lấy ô kí tự theo thứ tự
            Image slotImage = characterContainer.GetChild(currentTargetIndex).GetComponent<Image>();
            
            // Gán Sparite của kí tự ăn được vào ô kí tự
            slotImage.sprite = letterSprite;
            
            // Hiển thị màu rõ lên
            slotImage.color = Color.white;

            // Tăng chỉ số cho biến thứ tự ô
            currentTargetIndex++;
        }
    }

    // Hàm reser bảng chữ mỗi lần chơi
    public void ResetBoard()
    {
        currentTargetIndex = 0;
        foreach (Transform child in characterContainer)
        {
            Image img = child.GetComponent<Image>();
            img.sprite = null;
            img.color = new Color(1,1,1,0.2f);
        }
    }
}
