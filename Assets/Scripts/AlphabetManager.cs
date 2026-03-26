using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class AlphabetManager : MonoBehaviour
{
    public static AlphabetManager Instance; // Để các script khác dễ dàng gọi

    [Header("Cài đặt chữ cái")]
    public Sprite[] alphabetSprite; // Kéo toàn bộ chữ cái đã cắt vào đây
    public GameObject letterFoodPrefab; // Kéo cái Prefab LetterFood vào đây
    //public int foodCount = 2;
    [Header("Logic từ vựng(Nâng cấp)")]
    // Tạo danh sách các từ chủ đề động vật
    public string[] animalWords = {"CHICKEN", "SNAKE", "TIGER", "DUCK", "CAT", "DOG", "LION","MONKEY", "ZEBRA", "GIRAFFE", "HIPPO" };
    // Tạo 1 danh sách mới để trộn các từ khóa
    private List<string> shuffledWords = new List<string>();
    private int currentWordIndex = 0;
    [HideInInspector] public string targetWord;
    private int charIndex = 0;  // Thứ tự của kí tự trong từ
    // Danh sách để quản lý các chữ cái đang hiện có trên sân
    private List<GameObject> activeFood = new List<GameObject>();
    [Header("Hiệu ứng bay")]
    public GameObject flyingLetterPrefab; // Kéo cái Prefab FlyingLetter vào đây
    public Transform uiTargetContainer; // Kéo cái bảng Keyword vào đây
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ Manager này xuyên suốt các màn chơi
            PrepareWordList(); // Trộn bài ngay từ đầu
        } else {
            Destroy(gameObject);
        }
    }
    //__________HÀM TRỘN DANH SÁCH TỪ KHÓA_________
    void PrepareWordList()
    {
        shuffledWords = new List<string>(animalWords);
        // Thuật toán xáo bài Fisher-Yates
        for (int i = 0; i < shuffledWords.Count; i++) {
            string temp = shuffledWords[i];
            int randomIndex = Random.Range(i, shuffledWords.Count);
            shuffledWords[i] = shuffledWords[randomIndex];
            shuffledWords[randomIndex] = temp;
        }
        currentWordIndex = 0;
        PickNextWordFromList();
    }
    //________HÀM LẤY TỪ TIẾP THEO TRONG DANH SÁCH ĐÃ TRỘN________
    public void PickNextWordFromList()
    {
        if (currentWordIndex < shuffledWords.Count) {
            targetWord = shuffledWords[currentWordIndex];
            charIndex = 0;
            currentWordIndex++; // Tăng lên để màn sau lấy từ kế tiếp
            
            if (MySceneManager.Instance != null)
                MySceneManager.Instance.UpdateKeywordUI(targetWord);
        } else {
            Debug.Log("Đã chơi hết sạch từ vựng rồi ní ơi!");
            // Ní có thể cho trộn lại từ đầu hoặc hiện bảng "Phá đảo" ở đây
            PrepareWordList(); 
        }
    }
    private void OnEnable()
    {
        // Đăng ký sự kiện: Mỗi khi một Scene được load xong, gọi hàm OnSceneLoaded
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Hủy đăng ký để tránh rác bộ nhớ
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Hàm này sẽ tự động chạy MỖI KHI ní load sang màn mới (kể cả dùng LoadScene)
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene m, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // CHỈ CHẠY LOGIC KHI TÊN SCENE LÀ "GameScene" (Hoặc tên scene chơi game của ní)
        if (m.name == "GameScene") 
        {
            StopAllCoroutines();
            StartCoroutine(InitNewLevelRoutine());
        }
        else 
        {
            // Nếu ở Menu, ta xóa sạch mồi cũ đi cho rảnh nợ
            foreach(GameObject food in activeFood) if(food != null) Destroy(food);
            activeFood.Clear();
        }
    }
    IEnumerator InitNewLevelRoutine()
    {
        Debug.Log("<color=cyan>== BẮT ĐẦU LOAD MÀN MỚI ==</color>");
        
        // 1. Kiểm tra giá trị biến targetWord ngay khi vừa sang màn
        Debug.Log("1. Giá trị targetWord hiện tại: " + targetWord);
        Debug.Log("2. Chỉ số charIndex hiện tại: " + charIndex);

        // Đợi 1 frame để các Instance khác (MySceneManager) kịp khởi tạo
        yield return new WaitForEndOfFrame();

        // Đảm bảo cái bảng Keywords của ní trong Hierarchy tên là "KeyWords"
        GameObject keywordGo = GameObject.Find("KeyWords");
        if (keywordGo != null)
        {
            uiTargetContainer = keywordGo.GetComponent<RectTransform>();
            Debug.Log("Đã nối lại dây UI cho màn mới thành công!");
        }
        // 3. Kiểm tra MySceneManager đã tồn tại chưa
        if (MySceneManager.Instance != null) {
            Debug.Log("3. Đã tìm thấy MySceneManager. Đang gọi UpdateUI...");
            MySceneManager.Instance.UpdateKeywordUI(targetWord);
        } else {
            Debug.LogWarning("3. LỖI: Không tìm thấy MySceneManager Instance!");
        }

        // Đợi thêm một xíu cho chắc chắn trước khi sinh mồi
        yield return new WaitForSecondsRealtime(0.1f);

        // 4. Kiểm tra xem có đủ điều kiện sinh mồi không
        if (!string.IsNullOrEmpty(targetWord)) {
            Debug.Log("4. Đang tiến hành SpawnNewSet cho từ: " + targetWord);
            SpawnNewSet();
        } else {
            Debug.LogError("4. LỖI: targetWord bị trống, không thể sinh mồi!");
        }
        
        Debug.Log("<color=cyan>== KẾT THÚC QUÁ TRÌNH KHỞI TẠO MÀN ==</color>");
    }
    public void ResetProgressForNewGame()
    {
        charIndex = 0; 
        targetWord = ""; // Xóa từ khóa cũ để Coroutine bốc từ mới hoàn toàn
        
        // Gọi WordManager dọn dẹp bảng chữ cái (ní nhớ kéo Instance vào nhé)
        if (WordManager.Instance != null)
        {
            WordManager.Instance.ResetWordUI();
        }
        
        PrepareWordList(); 
        Debug.Log("Đã Reset sạch sẽ từ biến đến UI!");
    }
    // ________Hàm này chỉ dành riêng cho nút Replay_________
    public void ResetCurrentWord()
    {
        charIndex = 0; // Đưa về chữ cái đầu tiên của từ hiện tại
        
        // Dọn dẹp bảng UI chữ cái đã ăn (nếu ní có dùng WordManager)
        if (WordManager.Instance != null)
        {
            WordManager.Instance.ResetWordUI();
        }

        Debug.Log("Đã Reset từ hiện tại: " + targetWord + ". Mời ní ăn lại từ đầu!");
    }
        //_________HÀM CHỌN TỪ NGẪU NHIÊN TỪ DANH SÁCH_________
    public void PickRandomWord()
    {
        if(animalWords.Length > 0)
        {
            int randomIndex = Random.Range(0, animalWords.Length);
            targetWord = animalWords[randomIndex];
            charIndex = 0;
            if(MySceneManager.Instance != null)
            {
                MySceneManager.Instance.UpdateKeywordUI(targetWord);
            }
        }
    }
    public void SpawnNewSet()
    {
        // --- BƯỚC KIỂM TRA AN TOÀN (CHỐNG LỖI VĂNG MẢNG)
        if (string.IsNullOrEmpty(targetWord) || charIndex >= targetWord.Length) 
        {
            Debug.LogWarning("Chưa có từ khóa hoặc charIndex vượt quá độ dài từ! Đang tự sửa...");
            charIndex = 0; // Reset lại chỉ số
            if (string.IsNullOrEmpty(targetWord)) PickNextWordFromList(); // Bốc từ mới nếu trống
        }
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
            // Cho rắn dừng lại
            if(SnakeController.Instance != null)
            {
                SnakeController.Instance.canMovie = false;
            }
            // Tắt nhạc nền game
            AudioManager.Instance.MuteBGM();
            // Hiện bảng Victory
            StartCoroutine(VictoryDelayRoutine());
        }
    }
    IEnumerator VictoryDelayRoutine()
    {
        // Đợi khoảng 0.4 giây cho hiệu ứng bay hoàn tất
        yield return new WaitForSeconds(0.5f);

        AudioManager.Instance.MuteBGM();
        MySceneManager.Instance.ShowVictory();
    }

    //_________HÀM GỌI HIỆU ỨNG KÍ TỰ BAY_______
    public void PlayFlyAnimation(Vector3 spawnWorldPos, Sprite letterSprite)
    {
        Canvas mainCanvas = FindObjectOfType<Canvas>();
        
        if (mainCanvas != null)
        {
            GameObject fly = Instantiate(flyingLetterPrefab, mainCanvas.transform);
            fly.transform.SetAsLastSibling();
            
            // 1. Ép Scale về chuẩn (1, 1, 1)
            fly.transform.localScale = Vector3.one;
            
            // 2. Ép Z Position về 0 để không bị che khuất
            RectTransform flyRect = fly.GetComponent<RectTransform>();
            Vector3 currentPos = flyRect.anchoredPosition3D;
            flyRect.anchoredPosition3D = new Vector3(currentPos.x, currentPos.y, 0f);

            // 3. Khởi tạo di chuyển
            Vector3 targetPos = uiTargetContainer.position;
            fly.GetComponent<FlyingLetter>().Init(spawnWorldPos, targetPos, letterSprite);
        }
    }
}
