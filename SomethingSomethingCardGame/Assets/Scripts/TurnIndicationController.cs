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
	[SerializeField] private Animator animator;
	[SerializeField] private AudioClip highlightSound;

	enum TransitionState {
		
	}

	private bool doHighlight { get { return (isTurn && highlightTurn) || (!isTurn && highlightWaiting); } }
	private Sprite CurrentImage {
		get {
			if (isTurn) {
				return turnImage;
			} else {
				return waitingImage;
			}
		}
	}

	public void SetTurn(bool isTurn) {
		// Only do something if the state changes
		if (isTurn == this.isTurn) {
			return;
		}
		this.isTurn = isTurn;


		if (doHighlight) {
			animator.SetTrigger("exchange");
			audioSource.PlayOneShot(highlightSound);
		} else {
			animator.SetTrigger("flip");
		}
	}

	private void UpdateImage() {
		imageComponent.sprite = CurrentImage;
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
