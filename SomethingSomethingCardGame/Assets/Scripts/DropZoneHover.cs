using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DropZoneHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField] private Color standbyColor;
	[SerializeField] private Color hoverColor;

	private Image imageComponent;

	private void Awake() {
		imageComponent = GetComponent<Image>();
		imageComponent.color = standbyColor;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		imageComponent.color = hoverColor;
	}

	public void OnPointerExit(PointerEventData eventData) {
		imageComponent.color = standbyColor;
	}
}
