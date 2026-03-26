using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        clodeMap.gameObject.SetActive(true);
        leftButton.onClick.AddListener(ShowPreviousMap);
        rightButton.onClick.AddListener(ShowNextMap);

        UpdateMapVisibility();
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
}
