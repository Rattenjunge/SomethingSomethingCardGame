using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButtonController : MonoBehaviour {
	[SerializeField] private Sprite pressedImage;
	[SerializeField] private Sprite unpressedImage;
	[SerializeField] private Image buttonFace;

	bool clicked = false;
	PlayerManager playerManager;

	private void Awake() {
		ResetButton();
	}

	public void OnClick() {
		if (playerManager == null) {
			NetworkIdentity networkIdentity = NetworkClient.connection.identity;
			playerManager = networkIdentity.GetComponent<PlayerManager>();
		}

		if (clicked) {
			ResetButton();
			playerManager.CmdPlayerReady(playerManager.netId, false);
		} else {
			clicked = true;
			buttonFace.sprite = pressedImage;
			playerManager.CmdPlayerReady(playerManager.netId, true);
		}
	}

	public void ResetButton() {
		clicked = false;
		buttonFace.sprite = unpressedImage;
	}
}
