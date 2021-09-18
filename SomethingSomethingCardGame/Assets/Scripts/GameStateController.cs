using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour {
	[SerializeField] private GameObject victoryPrefab;
	[SerializeField] private GameObject defeatPrefab;
	[SerializeField] private GameObject drawPrefab;
	[SerializeField] private Color muteColor;
	[SerializeField] private Image muteComponent;
	[SerializeField] private float muteTransitionTime = 0.5f;

	private float muteTotalTransitionTime = 1;
	private GameObject winStateAnimation;

	private void Update() {
		float transitionProgress = muteTotalTransitionTime / muteTransitionTime;
		if (transitionProgress < 1) {
			transitionProgress += Time.deltaTime;
			muteComponent.color = Color.Lerp(muteComponent.color, muteColor, transitionProgress);
		}
	}

	public void SetWinState(string winState) {
		Destroy(winStateAnimation);
		switch (winState) {
			case "WIN":
				winStateAnimation = Instantiate(victoryPrefab, gameObject.transform);
				break;
			case "LOST":
				winStateAnimation = Instantiate(defeatPrefab, gameObject.transform);
				break;
			case "DRAW":
			default:
				winStateAnimation = Instantiate(drawPrefab, gameObject.transform);
				break;
		}
		winStateAnimation.transform.SetAsFirstSibling();
		(winStateAnimation.transform as RectTransform).anchoredPosition = Vector2.zero;

		muteComponent.color = new Color(muteColor.r, muteColor.g, muteColor.b, 0);
		muteTotalTransitionTime = 0;
	}
}
