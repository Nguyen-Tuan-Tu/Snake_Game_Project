using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphabetManager : MonoBehaviour
{
    public static AlphabetManager Instance; // Để các script khác dễ dàng gọi

    [Header("Cài đặt chữ cái")]
    public Sprite[] alphabetSprite; // Kéo toàn bộ chữ cái đã cắt vào đây
    public GameObject letterFoodPrefab; // Kéo cái Prefab LetterFood vào đây
    public int foodCount = 2;

    // Danh sách để quản lý các chữ cái đang hiện có trên sân
    private List<GameObject> activeFood = new List<GameObject>();
    private void Awake()
    {
        // Khởi tạo Instance ngay khi game load để các script khác dùng được [cite: 2026-03-01]
        //if (Instance == null) {
            Instance = this;
       // } else {
        //    Destroy(gameObject);
        //}
    }
    // Tạo bộ 3 chữ cái đầu tiên khi vào game
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
        // 2.Tạo 3 chữ mới ở 3 vị trí mới
        for( int i = 0; i < foodCount; i++)
        {
            GameObject newFood = Instantiate(letterFoodPrefab);

            // Set chữ ngãu nhiên
            int randomIndex = Random.Range(0, alphabetSprite.Length);
            newFood.GetComponent<SpriteRenderer>().sprite = alphabetSprite[randomIndex];

            // Set vị trí ngẫu nhiên
            SnakeController.Instance.RandomizeFoodPosition(newFood);

            // Thêm vào danh sách để quản lý
            activeFood.Add(newFood);
        }
    }
}
