using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDetailsController : MonoBehaviour {
	[SerializeField] private Text nameText;
	[SerializeField] private PreviewCard previewCard;
	[SerializeField] private Text flavourText;

	private void Awake() {
		Hide();
	}

	public void ShowCard(CreatureCard card) {
		SetEnableChildren(true);
		nameText.text = card.Name;
		previewCard.Init(card);
		flavourText.text = card.FlavourText;
	}

	public void Hide() {
		SetEnableChildren(false);
	}

	private void SetEnableChildren(bool enable) {
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild(i).gameObject.SetActive(enable);
		}
	}
}
