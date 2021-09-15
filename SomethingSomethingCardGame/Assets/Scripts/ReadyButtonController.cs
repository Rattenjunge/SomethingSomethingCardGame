using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyButtonController : MonoBehaviour {
	[SerializeField] private Sprite victorySprite;
	[SerializeField] private Sprite defeatSprite;
	[SerializeField] private Sprite drawSprite;
	[SerializeField] private Image winStateImage;

	bool clicked = false;
	PlayerManager playerManager;



	public void OnClick() {
		if (playerManager == null) {
			NetworkIdentity networkIdentity = NetworkClient.connection.identity;
			playerManager = networkIdentity.GetComponent<PlayerManager>();
		}

		if (clicked) {
			clicked = false;
			GetComponentInChildren<Text>().text = "Ready";
			playerManager.CmdPlayerReady(playerManager.netId, false);
		} else {
			clicked = true;
			GetComponentInChildren<Text>().text = "Cancel";
			playerManager.CmdPlayerReady(playerManager.netId, true);
		}
	}

	public void ResetButton() {
		clicked = false;
		GetComponentInChildren<Text>().text = "Ready";

	}

	public void SetWinState(string winState) {
		winStateImage.enabled = true;
		switch (winState) {
			case "WIN":
				winStateImage.sprite = victorySprite;
				break;
			case "LOST":
				winStateImage.sprite = defeatSprite;
				break;
			case "DRAW":
			default:
				winStateImage.sprite = drawSprite;
				break;
		}
	}
}
