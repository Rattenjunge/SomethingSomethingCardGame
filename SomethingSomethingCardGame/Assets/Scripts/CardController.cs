using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int scaleFactor=2;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip placedSound;

    private RectTransform rect;

    private AudioSource audioSource;

    private void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();

    }

    private void ZoomCard(bool isZoomed)
    {
        if (isZoomed)
        {
            rect.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        else
        {
            rect.localScale = Vector3.one;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(hoverSound);
        ZoomCard(true);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        ZoomCard(false);
    }
}
