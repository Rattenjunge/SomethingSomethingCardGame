using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnIndicationController : MonoBehaviour {
	[SerializeField] private Sprite turnImage;
	[SerializeField] private Sprite waitingImage;
	[SerializeField] private bool isTurn = false;
	[SerializeField] private bool highlightTurn = true;
	[SerializeField] private bool highlightWaiting = false;
	[SerializeField] private Image imageComponent;
	[SerializeField] private AudioSource audioSource;

	[SerializeField] private Vector2 standbyPosition = Vector2.zero;
	[SerializeField] private Vector2 highlightPosition;
	[SerializeField] private float highlightScale = 3;
	[SerializeField] private float standbyScale= 1;
	[SerializeField] private AudioClip highlightSound;

	private bool doHighlight { get { return (isTurn && highlightTurn) || (!isTurn && highlightWaiting); } }

	private void Awake() {
		(transform as RectTransform).anchoredPosition = standbyPosition;
		(transform as RectTransform).localScale = Vector2.one * standbyScale;
	}

	public void SetTurn(bool isTurn) {
		// Only do something if the state changes
		if (isTurn == this.isTurn) {
			return;
		}
		this.isTurn = isTurn;

		// Choose the correct image to display
		Sprite newImage = waitingImage;
		if (isTurn) {
			newImage = turnImage;
		}

		// Display new image
		imageComponent.sprite = newImage;

		// Do highlight, depending on state
		if (doHighlight) {
			(transform as RectTransform).anchoredPosition = highlightPosition;
			(transform as RectTransform).localScale = Vector2.one * highlightScale;

			audioSource.PlayOneShot(highlightSound);
		} else {
			(transform as RectTransform).anchoredPosition = standbyPosition;
			(transform as RectTransform).localScale = Vector2.one * standbyScale;
		}
	}

	public void Update() {
		if (doHighlight) {
			(transform as RectTransform).anchoredPosition = Vector2.Lerp((transform as RectTransform).anchoredPosition, standbyPosition, Time.deltaTime);
			(transform as RectTransform).localScale = Vector2.Lerp((transform as RectTransform).localScale, Vector2.one * standbyScale, Time.deltaTime);
		}
	}


	[ContextMenu("Set State Turn")]
	public void SetTurnState() {
		SetTurn(true);
	}

	[ContextMenu("Set State Waiting")]
	public void SetWaitingState() {
		SetTurn(false);
	}
}
