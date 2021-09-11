using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverController : MonoBehaviour
{
    [SerializeField] RectTransform cardPopup;

    public void ShowCardPopup()
    {
        cardPopup.gameObject.SetActive(true);
        cardPopup.anchoredPosition = new Vector2(transform.position.x, cardPopup.anchoredPosition.y);
    }

    public void HideCardPopup()
    {
        cardPopup.gameObject.SetActive(false);
    }
}
