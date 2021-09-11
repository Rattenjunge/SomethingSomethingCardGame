using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleFactorZoomIn = 1f;
    [SerializeField] private float scaleFactorZoomOut = 0.5f;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip placedSound;
    [SerializeField] private List<AttackScoreColor> attackScoreColors;
    [SerializeField] private List<AttackScoreController> attackScoreControllers;
    [SerializeField] private Image creatureImage;

    private RectTransform rect;

    private AudioSource audioSource;


    private void Awake()
    {
        attackScoreColors.Sort(AttackScoreColor.CompareByScore);
        attackScoreControllers.ForEach(controller => { controller.SetAttackScoreColors(attackScoreColors); });
    }

    private void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        ZoomCard(false);
    }

    public void Init(CreatureCard newCard)
    {
        GetComponent<InstantiatedCard>().playableCard = newCard;
        creatureImage.sprite = newCard.FaceImage;
        foreach (Direction4 direction4 in Enum.GetValues(typeof(Direction4)))
        {
            attackScoreControllers.Where(controller => controller.direction4 == direction4).First()
                .SetAttackScore(newCard.GetAttackPointsByDirection(direction4));
        }
    }

    private void ZoomCard(bool isZoomed)
    {
        if (isZoomed)
        {
            rect.localScale = Vector3.one * scaleFactorZoomIn;
        }
        else
        {
            rect.localScale = Vector3.one * scaleFactorZoomOut;
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

[SerializeField]
public enum Direction4
{
    North,
    South,
    East,
    West
}
