using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlyingLetter : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 targetPosition;
    private float speed = 10f;
    private bool isMoving = false;

    public void Init(Vector3 startPos, Vector3 endPos, Sprite letterSprite)
    {
        rectTransform = GetComponent<RectTransform>();
        GetComponent<UnityEngine.UI.Image>().sprite = letterSprite;

        // Chuyển đổi vị trí từ World Space sang Screen Space (UI)
        transform.position = Camera.main.WorldToScreenPoint(startPos);
        targetPosition = endPos;
        isMoving = true;
    }

    void Update()
    {
        if(isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            if(Vector3.Distance(transform.position, targetPosition) < 10f)
            {
                isMoving = false;
                Destroy(gameObject);
            }
        }
    }
}
