using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    [Header("Kéo 3 cái map vào đây theo thứ tự")]
    public List<GameObject> mapImages;

    [Header("Kéo 2 cái button mũi tên vào đây")]
    public Button leftButton;
    public Button rightButton;

    public Button clodeMap;

    private int currentMapIndex = 0;
    // Danh sách các chủ đề
    public List<TopicData> allTopics;

    [Header("UI Popup Chủ đề")]
    public GameObject topicPopupPanel; // Bảng tên topic
    public TextMeshProUGUI topicNameText; // Hiển thị tên topic

    private int tempSelectedTopicIndex; // Biến tạm để nhớ topic đang chơi

    void Start()
    {
        clodeMap.gameObject.SetActive(true);
        leftButton.onClick.AddListener(ShowPreviousMap);
        rightButton.onClick.AddListener(ShowNextMap);

        UpdateMapVisibility();
        if (AlphabetManager.Instance != null && AlphabetManager.Instance.shouldOpenMapOnMenuLoad)
        {
            // 1. Gọi hàm mở Topic Map (Hàm mà ní gán cho nút Word Topic ấy)
            MainMenuManager.Instance.OpenTopicMap(); 

            // 2. Hạ cờ xuống để lần sau vào Menu bình thường không bị tự mở Map
            AlphabetManager.Instance.shouldOpenMapOnMenuLoad = false;
            
            Debug.Log("Đã tự động mở Topic Map từ Game quay về!");
        }
    }

    public void ShowNextMap()
    {
        if(currentMapIndex < mapImages.Count - 1)
        {
            currentMapIndex++;
            UpdateMapVisibility();
        }
    }

    public void ShowPreviousMap()
    {
        if(currentMapIndex > 0)
        {
            currentMapIndex--;
            UpdateMapVisibility();
        }
    }

    private void UpdateMapVisibility()
    {
        // Hiện map hiện tại, ẩn các map còn lại
        for(int i = 0;i < mapImages.Count; i++)
        {
            mapImages[i].SetActive(i == currentMapIndex);
        }
        // Tự ẩn nút nếu hết map để chuyển
        leftButton.gameObject.SetActive(currentMapIndex > 0);
        rightButton.gameObject.SetActive(currentMapIndex < mapImages.Count - 1);
    }

    public void ShowTopicPreview(TopicData data) // Nhận trực tiếp file dữ liệu
    {
        if (data == null) return;
    
        topicNameText.text = data.topicName;
        topicPopupPanel.SetActive(true);
        
        Debug.Log($"<color=red>FILE ĐANG NHẬN:</color> {data.name} | <color=yellow>NỘI DUNG CHỮ:</color> {data.topicName}");

        // 1. Tìm vị trí của file data này trong danh sách 15 chủ đề
        int index = allTopics.IndexOf(data); 

        // 2. Lưu vào "trí nhớ" của AlphabetManager để tí nữa nạp từ
        if (index != -1) // Nếu tìm thấy trong danh sách
        {
            AlphabetManager.Instance.currentSelectedTopicIndex = index;
            Debug.Log($"<color=green>Đã lưu Index chủ đề:</color> {index}");
        }
        else
        {
            Debug.LogError("CẢNH BÁO: File TopicData này chưa được kéo vào List allTopics trong Inspector!");
        }
    }
    public void CloseTopicPreview()
    {
        topicPopupPanel.SetActive(false);
    }
}
