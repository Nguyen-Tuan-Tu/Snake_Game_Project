using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterItem : MonoBehaviour
{
    public char myCharacter; // Chữ cái của iteam này(ví dụ 'C')
    public bool isCorrect; // Biến kiểm tra đây có phải ký tự đúng của từ khóa không
    public Sprite mySprite; // Lưu lại Sprite của ký tự để gửi lên bảng KeyWords

    private bool isEaten = false; // Biến kiểm tra đã ăn chưa
    // Hàm này gọi khi rắn chạm vào mồi
    public void OnEaten() 
    {
        if (isEaten) return; // Nếu đã ăn rồi thì không xử lý nữa 
        
        isEaten = true; // Đánh dấu là đã ăn ngay lập tức
        // Tắt Collider để con rắn không còn "thấy" vật thể này nữa
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        Destroy(gameObject);
    }
}
