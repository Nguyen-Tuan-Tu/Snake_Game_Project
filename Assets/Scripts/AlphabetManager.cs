using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AlphabetManager : MonoBehaviour
{
    public static AlphabetManager Instance; // Để các script khác dễ dàng gọi

    [Header("Cài đặt chữ cái")]
    public Sprite[] alphabetSprite; // Kéo toàn bộ chữ cái đã cắt vào đây
    public GameObject letterFoodPrefab; // Kéo cái Prefab LetterFood vào đây
    //public int foodCount = 2;
    [Header("Logic từ vựng")]
    public string targetWord = "CHICKEN";
    private int charIndex = 0;  // Thứ tự của kí tự trong từ
    // Danh sách để quản lý các chữ cái đang hiện có trên sân
    private List<GameObject> activeFood = new List<GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        SpawnNewSet();
    }
    public void SpawnNewSet()
    {
        // 1. Xóa sạch các chữ cũ đang có trên màn hình
        foreach(GameObject food in activeFood)
        {
            Destroy(food);
        }

        activeFood.Clear();

        // 2. Sinh ra 1 kí tự đúng 
        char correctChar = targetWord[charIndex];
        SpawnLetter(correctChar, true);

        // 3.Tạo 2 kí tự sai ngẫu nhiên và khác vs kí tự đúng
        List<char> spawnedChars = new List<char>();
        spawnedChars.Add(correctChar); // Đưa kí tự đúng vào danh sách đã tồn tại

        for(int i = 0; i < 2 ; i++)
        {
            char wrongChar;
            do
            {
                wrongChar = (char)Random.Range('A', 'Z' + 1);
            }
            // Vòng lặp này sẽ chạy cho đến khi wrongChar KHÁC chữ đúng 
            // và KHÁC luôn cả chữ sai đã sinh trước đó
            while ( spawnedChars.Contains(wrongChar));

            spawnedChars.Add(wrongChar); // Lưu lại chữ sai vừa chọn
            SpawnLetter(wrongChar, false);
        }
         
    }

    // Hàm phụ để tạo kí tự

    private void SpawnLetter(char character, bool isCorrect)
    {
        GameObject newFoood = Instantiate(letterFoodPrefab);
        LetterItem iteam = newFoood.GetComponent<LetterItem>();

        // Bây giờ 'character' đã được xác định, không còn lỗi nữa!
        int spriteIndex = character - 'A';
        
        // Kiểm tra xem index có nằm trong mảng không để tránh lỗi văng game
        if (spriteIndex >= 0 && spriteIndex < alphabetSprite.Length)
        {
            Sprite letterSprite = alphabetSprite[spriteIndex];

            // Gán dữ liệu vào Script LetterItem trên Prefab
            iteam.myCharacter = character;
            iteam.isCorrect = isCorrect;
            iteam.mySprite = letterSprite;

            newFoood.GetComponent<SpriteRenderer>().sprite = letterSprite;
            SnakeController.Instance.RandomizeFoodPosition(newFoood);
            activeFood.Add(newFoood);
        }
        else
        {
            Debug.LogError("Lỗi rồi ní: Ký tự " + character + " không tìm thấy Sprite tương ứng!");
        }
    }

    public void NextLetter()
    {
        charIndex++;
        if(charIndex < targetWord.Length)
        {
            SpawnNewSet();
        }
        else
        {
            // Tắt nhạc nền game
            GameObject bgm = GameObject.Find("BackgroundMusic");
            if (bgm != null)
            {
                AudioSource bgmSource = bgm.GetComponent<AudioSource>();
                if (bgmSource != null) bgmSource.Stop();
            }
            // Hiện bảng Victory
            MySceneManager.Instance.ShowVictory();
        }
    }
}
