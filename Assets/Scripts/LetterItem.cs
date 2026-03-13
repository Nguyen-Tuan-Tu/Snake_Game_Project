using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterItem : MonoBehaviour
{
    public char myCharacter; // Chữ cái của iteam này(ví dụ 'C')
    public bool isCorrect; // Biến kiểm tra đây có phải ký tự đúng của từ khóa không
    public Sprite mySprite; // Lưu lại Sprite của ký tự để gửi lên bảng KeyWords
}
