using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement; // Để dùng lệnh LoadScene (Chơi lại)
using UnityEngine.UI; // Để điều khiển được cái Text
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class SnakeController : MonoBehaviour
{
    // Biến lưu hướng đi hiện tại
    private Vector2 _direction = Vector2.right;
    private float moveInterval; // Biến khai báo tốc độ của rắn

    private float nextMoveTime; // Biến phụ để kiểm tra thời điểm bước tiếp theo

    public TMP_Text speedDisplayText; // Biến hiển thị tốc độ rắn

    // Biến chứa khuôn mẫu thân rắn
    public GameObject bodyPrefab;
    public GameObject tailPrefab;

    // Các biến xử lý âm thanh
    public AudioClip eatSound; // Kéo file âm thanh ăn mồi vào đây
    public AudioClip wrongSound; // Âm thanh khi ăn sau kí tự

    public GameObject[] Hearts;

    public int wrongCount = 0;

    public GameObject eatEffectPrefab; // Kéo hiệu ứng ăn mồi vào đây
    public AudioClip collideSound; // // Kéo file âm thanh va chạmh vào đây
    private AudioSource _audioSource; // Biến để điều khiển cái loa

    public TMP_Text scoreText;  // Biến hiện thị điểm khi chơi
    public TMP_Text PlayerScoreText;    // Biến hiển thị điểm khi kết thúc
    private int _score = 0; // Biến lưu điểm hiện tại

    // Danh sách chứa toàn bộ các đốt(cả đầu)
    private List<Transform> _segments = new List<Transform>();

    // Các hướng cơ thể của rắn
    // public Sprite headUp, headDown, headLeft, headRight;
    // public Sprite tailUp, tailDown, tailLeft, tailRight;
    // public Sprite bodyVert, bodyHorz;
    // public Sprite corner;
    public Sprite cornerUL, cornerUR, cornerDL, cornerDR;
    public Sprite bodySprite;
    public Sprite headUp;     
    public Sprite tailUp;

    public static SnakeController Instance;
    private void Awake()
    {
        // Gán chính cái Script này vào biến Instance ngay khi game khởi động [cite: 2026-03-01]
        Instance = this;
    }
    private void Start()
    {
        // Chưa bắt đầu chơi ngay
        MySceneManager.Instance.ShowReadPanel();
        // Lấy tốc độ mà người chơi vừa chọn ở Menu
        moveInterval = GameSettings.snakeSpeed;
        // Thêm đầu vào List
        _segments.Add(this.transform);

        // Tạo cái đuôi ngay lập tức
        GameObject tail = Instantiate(tailPrefab);
        // Đặt ví trí cho đuôi
        tail.transform.position = transform.position - Vector3.right;
        tail.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 180);
        // Thêm đuôi vào List
        _segments.Add(tail.transform);

        // Lấy component AudioSource đang gắn trên con rắn
        _audioSource = GetComponent<AudioSource>();

        UpdateSpeedUI(); // Hiển thị tốc độ của rắn
    }
    //__________HÀM KẾT THÚC GAME_________
    private void EndGame()
    {
        FindObjectOfType<MySceneManager>().ShowGameOver();
        //ShowPlayerScore();
        Time.timeScale = 0;
        //Debug.Log("Thua rồi cưng ơi^_^");
    }
    // Hàm update: Chạy liên tục mỗi khung hình
    private void Update()
    {
        // Nhấn phím W hoặc mũi tên lên
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(_direction != Vector2.down)
            {
                _direction = Vector2.up;
                
            }
        }
        // Nhấn phím S hoặc mũi tên xuống
        else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(_direction != Vector2.up)
            {
                _direction = Vector2.down;
                
            }
        }
        // Nhấn phím A hoặc mũi tên sang trái
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(_direction != Vector2.right)
            {
                _direction = Vector2.left;
                
            }
        }
        // Nhấn phím D hoặc mũi tên sang phải
        else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(_direction != Vector2.left)
            {
                _direction = Vector2.right;
                
            } 
        }
    }

    //_________HÀM CẬP NHẬT VỊ TRÍ CHO RẮN KHI DI CHUYỂN________
    private void FixedUpdate()
    {
        // CHỈ CHO PHÉP DI CHUYỂN KHI ĐÃ ĐẾN GIỜ (dựa trên moveInterval)
        if (Time.time < nextMoveTime)
        {
            return; // Nếu chưa đến lúc thì thoát hàm, không làm gì cả
        }

        // Cập nhật thời điểm cho bước di chuyển tiếp theo [cite: 2026-03-02]
        nextMoveTime = Time.time + moveInterval;
        // THUẬT TOÁN DI CHUYỂN ĐUÔI*
        // duyệt ngược từ đốt cuối cùng lên đốt thứ 2
        for(int i = _segments.Count - 1; i > 0;i--)
        {
            // đốt sau lấy vị trí của đốt trước
            _segments[i].position = _segments[i -1].position;
            // Đốt sau xoay theo đốt trước
           // _segments[i].rotation = _segments[i - 1].rotation;
        }
        // 1. Tính toán vị trí mới
        // Lấy vị trí hiện tại (x,y) + hướng đi (x,y)
        float x = transform.position.x + _direction.x;
        float y = transform.position.y + _direction.y;

        // 2. Cập nhật vị trí cho rắn
        // Mathf.Round để đảm bảo rắn luôn nằm tròn trịa trong ô lưới( không bị lẻ 0.5)
        transform.position = new Vector3(
            Mathf.Round(x),
            Mathf.Round(y),
            0.0f
        );

        // 3. GỌI HÀM VẼ HÌNH MỚI
        UpdateSnakeVisuals();
    }

    //__________HÀM XỬ LÝ GÓC CHUYỂN HƯỚNG CỦA RẮN__________
    private void UpdateSnakeVisuals()
    {
        for (int i = 0; i < _segments.Count; i++)
        {
            Transform segment = _segments[i];
            SpriteRenderer sr = segment.GetComponent<SpriteRenderer>(); 

            // --- 1. XỬ LÝ ĐẦU (Ảnh gốc nhìn xuống) ---
            if (i == 0) 
            {
                sr.sprite = headUp; 
                if (_direction == Vector2.up) segment.rotation = Quaternion.Euler(0, 0, 180);
                else if (_direction == Vector2.down) segment.rotation = Quaternion.Euler(0, 0, 0);
                else if (_direction == Vector2.left) segment.rotation = Quaternion.Euler(0, 0, -90);
                else if (_direction == Vector2.right) segment.rotation = Quaternion.Euler(0, 0, 90);
                continue;
            }

            Vector3 prevPos = _segments[i - 1].position;
            Vector3 nextPos;

            // --- 2. XỬ LÝ ĐUÔI ---
            if (i == _segments.Count - 1) 
            {
                sr.sprite = tailUp; 
                
                // So sánh vị trí đuôi với đốt ngay trước nó (i-1)
                if (segment.position.x == prevPos.x) // Đang nằm dọc
                {
                    // Nếu đuôi ở dưới thân -> Giữ nguyên (0 độ) để trỏ xuống đất
                    // Nếu đuôi ở trên thân -> Xoay 180 để trỏ lên trời
                    segment.rotation = (segment.position.y < prevPos.y) ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 180);
                } 
                else // Đang nằm ngang
                {
                    // Nếu đuôi bên trái thân -> Xoay -90 (hoặc 270) để trỏ sang trái
                    // Nếu đuôi bên phải thân -> Xoay 90 để trỏ sang phải
                    segment.rotation = (segment.position.x < prevPos.x) ? Quaternion.Euler(0, 0, -90) : Quaternion.Euler(0, 0, 90);
                }
                continue;
            }
            else {
                nextPos = _segments[i + 1].position;
            }

            // --- 3. XỬ LÝ THÂN & GÓC CUA ---
            if (prevPos.x == nextPos.x) // Thẳng đứng
            {
                sr.sprite = bodySprite; 
                segment.rotation = Quaternion.identity; 
            }
            else if (prevPos.y == nextPos.y) // Thẳng ngang
            {
                sr.sprite = bodySprite; 
                segment.rotation = Quaternion.Euler(0, 0, 90);
            }
            else // KHÚC CUA
            {
                segment.rotation = Quaternion.identity;
                bool isR = (prevPos.x > segment.position.x || nextPos.x > segment.position.x);
                bool isL = (prevPos.x < segment.position.x || nextPos.x < segment.position.x);
                bool isU = (prevPos.y > segment.position.y || nextPos.y > segment.position.y);
                bool isD = (prevPos.y < segment.position.y || nextPos.y < segment.position.y);

                // Gán đúng theo số thứ tự đã đánh dấu
                if (isL && isU) sr.sprite = cornerUL;      // Số 1
                else if (isR && isU) sr.sprite = cornerUR; // Số 2
                else if (isR && isD) sr.sprite = cornerDR; // Số 3
                else if (isL && isD) sr.sprite = cornerDL; // Số 4
            }
        }
    }
    //_________HÀM MỌC THÊM ĐỐT MỚI_________
    private void Grow()
    {
        // sinh ra 1 khúc thân mới tại vị trí con rắn
        GameObject segment = Instantiate(bodyPrefab);

        // Lấy vị trí của cái đuôi hiện tại(phần tử cuối cùng)
        Transform tail = _segments[_segments.Count - 1];


        // Đặt khúc thân mới trùng vào vị trí cái đuôi
        segment.transform.position = tail.position;
        segment.transform.rotation = tail.rotation;

        // Chèn khúc thân mới vào trước cái đuôi
        _segments.Insert(_segments.Count - 1, segment.transform);
    }

    //_________HÀM PHÁT HIỆN VA CHẠM________
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            // 1. Lấy thông tin từ chữ cái vừa đụng trúng
            LetterItem item = other.GetComponent<LetterItem>();

            // KIỂM TRA: NẾU ĂN ĐÚNG CHỮ [cite: 2026-03-01]
            if (item != null && item.isCorrect)
            {
                // Gọi hiệu ứng bay kí tụ lên bảng keyword
                AlphabetManager.Instance.PlayFlyAnimation(other.transform.position, item.mySprite);
                _audioSource.PlayOneShot(eatSound);
                GameObject effect = Instantiate(eatEffectPrefab, other.transform.position, Quaternion.identity);
                Destroy(effect, 1f);

                // Gửi Sprite lên bảng Keywords và tăng độ dài rắn
                WordManager.Instance.CollectLetter(item.mySprite);
                Grow();

                // Gọi AlphabetManager để tiến tới chữ cái tiếp theo và sinh bộ mới
                // Hàm NextLetter này sẽ tự kiểm tra thắng cuộc nếu hết từ CHICKEN [cite: 2026-03-01]
                AlphabetManager.Instance.NextLetter(); 
            }
            // KIỂM TRA: NẾU ĂN SAI CHỮ (Gây nhiễu)
            else 
            {
                if (wrongCount < Hearts.Length)
                {
                    Hearts[wrongCount].SetActive(false);
                }
                wrongCount++;
                if(wrongCount >= 3)
                {
                    MySceneManager.Instance.ShowGameOver();
                    HandleGameOver();
                }
                // Phát âm thanh khi ăn sai kí tự
                _audioSource.PlayOneShot(wrongSound);

                // Tùy chọn: Sinh lại bộ chữ mới ở vị trí khác
                AlphabetManager.Instance.SpawnNewSet();
            }
        }
        else if (other.CompareTag("Obstacle")) 
        {
            HandleGameOver();
        }
    }

    // Gom nhóm đoạn tắt nhạc và EndGame vào một hàm
    public void HandleGameOver()
    {
        _audioSource.PlayOneShot(collideSound);
        // GameObject bgm = GameObject.Find("BackgroundMusic");
        // if (bgm != null)
        // {
        //     AudioSource bgmSource = bgm.GetComponent<AudioSource>();
        //     if (bgmSource != null) bgmSource.Stop();
        // }
        AudioManager.Instance.MuteBGM();
        EndGame();
    }
    //_________HÀM TẠO THỨC ĂN TẠI VỊ TRÍ NGẪU NHIÊN_________
    public void RandomizeFoodPosition(GameObject food)
    {
        Vector3 newPos;
        bool isInvalid;
        int safetyBreak = 0;

        // Tạm thời tắt Collider của chính nó để không tự va chạm với chính mình [cite: 2026-03-02]
        Collider2D myCol = food.GetComponent<Collider2D>();
        if (myCol != null) myCol.enabled = false;

        do {
            float x = Mathf.Round(Random.Range(-15f, 15f));
            float y = Mathf.Round(Random.Range(-7f, 4f));
            newPos = new Vector3(x, y, 0f);

            // QUAN TRỌNG: Cập nhật vị trí vào hệ thống vật lý ngay lập tức [cite: 2026-03-01]
            food.transform.position = newPos;
            Physics2D.SyncTransforms(); 

            // Kiểm tra vùng 2.5 ô xung quanh có chạm chữ khác không [cite: 2026-03-01]
            // Sử dụng OverlapBox vì các chữ cái của bạn hình vuông
            Collider2D hit = Physics2D.OverlapBox(newPos, new Vector2(2.5f, 2.5f), 0f);
            
            isInvalid = (hit != null); // Nếu chạm bất cứ thứ gì khác là không hợp lệ [cite: 2026-03-01]
            
            safetyBreak++;
        } while (isInvalid && safetyBreak < 100);

        // Bật lại Collider sau khi đã tìm được chỗ đỗ xe an toàn [cite: 2026-03-02]
        if (myCol != null) myCol.enabled = true;
    }

    //__________HÀM TÍNH ĐIỂM__________
    // private void UpdateScore()
    // {
    //     _score++;
    //     scoreText.text = "Score : " + _score.ToString();
    // }
    public void UpdateSpeedUI()
    {
        string speedName = "";
        if(moveInterval >= 0.2f) speedName = "Slow";
        else if(moveInterval <= 0.05f) speedName = "Fast";
        else speedName = "Medium";

        // Gán nội dung vào bảng hiển thị
        speedDisplayText.text = "" + speedName;
    }

    public void ShowPlayerScore()
    {
        PlayerScoreText.text = "YOUR SCORE : " + _score.ToString();
    }
}
