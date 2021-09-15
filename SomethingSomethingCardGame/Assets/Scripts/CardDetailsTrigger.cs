using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(InstantiatedCard))]
public class CardDetailsTrigger : MonoBehaviour, IPointerClickHandler {
	[SerializeField] private InstantiatedCard instantiatedCard;
	private CardDetailsController detailsController;

	// Start is called before the first frame update
	void Awake() {
		// Get details controller component
		GameObject detailsObject = GameObject.FindGameObjectWithTag("CardDetails");
		if (detailsObject != null) {
			detailsController = detailsObject.GetComponent<CardDetailsController>();
		}
	}

	public void OnPointerClick(PointerEventData eventData) {
		Debug.Log(eventData);
		if (detailsController == null) {
			return;
		}

		if (eventData.button == PointerEventData.InputButton.Right) {
			ShowCard(instantiatedCard.playableCard);
		}
	}

	private void ShowCard(PlayableCard card) {
		if (card is CreatureCard) {
			detailsController.ShowCard((CreatureCard)card);
		}
	}
}
