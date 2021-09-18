using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] private float scaleFactorZoomIn = 1f;
	[SerializeField] private float scaleFactorZoomOut = 0.5f;
	[SerializeField] private float translateYZoomIn = 75f;
	[SerializeField] private AudioClip clickSound;
	[SerializeField] private AudioClip hoverSound;
	[SerializeField] private AudioClip placedSound;
	[SerializeField] private List<AttackScoreColor> attackScoreColors;
	[SerializeField] private List<AttackScoreController> attackScoreControllers;
	[SerializeField] private Image creatureImage;

	private RectTransform rect;
	private Quaternion handRotation;
	private Vector3 handPosition;
	private int handIndex;

	private AudioSource audioSource;

	private void Awake() {
		attackScoreColors.Sort(AttackScoreColor.CompareByScore);
		attackScoreControllers.ForEach(controller => { controller.SetAttackScoreColors(attackScoreColors); });
	}

	private void OnEnable() {
		rect = GetComponent<RectTransform>();
		audioSource = GetComponent<AudioSource>();
		rect.localScale = Vector3.one * scaleFactorZoomOut;
	}

	public void Init(CreatureCard newCard) {
		GetComponent<InstantiatedCard>().playableCard = newCard;
		creatureImage.sprite = newCard.FaceImage;
		foreach (Direction4 direction4 in Enum.GetValues(typeof(Direction4))) {
			attackScoreControllers.Where(controller => controller.direction4 == direction4).First()
				.SetAttackScore(newCard.GetAttackPointsByDirection(direction4));
		}
	}

	private void ZoomCard(bool isZoomed) {
		if (isZoomed) {
			handPosition = rect.anchoredPosition;
			handRotation = rect.rotation;
			handIndex = rect.GetSiblingIndex();

			rect.localScale = Vector3.one * scaleFactorZoomIn;
			rect.rotation = Quaternion.identity;
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, translateYZoomIn);
			rect.SetAsLastSibling();
		} else {
			rect.localScale = Vector3.one * scaleFactorZoomOut;
			rect.SetSiblingIndex(handIndex);

			// If exists, rearrange children
			WheelLayoutGroup layout = transform.parent.GetComponent<WheelLayoutGroup>();
			if (layout != null) {
				layout.ArrangeChildren();
			} else {
				rect.anchoredPosition = handPosition;
				rect.rotation = handRotation;
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		audioSource.PlayOneShot(hoverSound);
		ZoomCard(true);
	}


	public void OnPointerExit(PointerEventData eventData) {
		ZoomCard(false);
	}
}

[SerializeField]
public enum Direction4 {
	North,
	South,
	East,
	West
}
