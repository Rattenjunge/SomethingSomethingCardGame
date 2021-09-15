using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnIndicationController : MonoBehaviour {
	[SerializeField] private Sprite turnImage;
	[SerializeField] private Sprite waitingImage;
	[SerializeField] private bool isTurn = false;
	[SerializeField] private bool soundTurn = true;
	[SerializeField] private bool soundWaiting = false;
	[SerializeField] private string turnAnimation = "exchange";
	[SerializeField] private string waitingAnimation = "flip";
	[SerializeField] private Image imageComponent;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private Animator animator;
	[SerializeField] private AudioClip changeSound;

	private bool doHighlight { get { return (isTurn && soundTurn) || (!isTurn && soundWaiting); } }
	private Sprite CurrentImage {
		get {
			if (isTurn) {
				return turnImage;
			} else {
				return waitingImage;
			}
		}
	}
	private string CurrentAnimation {
		get {
			if (isTurn) {
				return turnAnimation;
			} else {
				return waitingAnimation;
			}
		}
	}
	private bool CurrentSound {
		get {
			if (isTurn) {
				return soundTurn;
			} else {
				return soundWaiting;
			}
		}
	}

	public void SetTurn(bool isTurn) {
		// Only do something if the state changes
		if (isTurn == this.isTurn) {
			return;
		}
		this.isTurn = isTurn;

		if (CurrentAnimation == "") {
			imageComponent.sprite = CurrentImage;
		} else {
			animator.Play(CurrentAnimation, -1, 0);
		}

		if (CurrentSound) {
			audioSource.PlayOneShot(changeSound);
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
