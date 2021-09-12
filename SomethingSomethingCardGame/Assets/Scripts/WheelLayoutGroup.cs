using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelLayoutGroup : MonoBehaviour {
	[SerializeField] private float degreePadding = 6f; // In °
	[SerializeField] private float radius = 1500;

	private int lastChildCount = 0;

	void Update() {
		// Detecting changes in the children based on child count
		if (lastChildCount != transform.childCount) {
			lastChildCount = transform.childCount;
			ArrangeChildren();
		}
	}

	private void ArrangeChildren() {
		float initialAngle = degreePadding * (transform.childCount - 1) / 2;

		for (int i = 0; i < transform.childCount; i++) {
			// Calculate child angle
			float childAngle = initialAngle - i * degreePadding;

			// Calculate child position
			Vector2 circleDirection = GetDirectionOnUnitCircle(childAngle);
			Vector2 childPosition = radius * (circleDirection + Vector2.down);

			// Apply to child
			transform.GetChild(i).rotation = Quaternion.Euler(0, 0, childAngle);
			(transform.GetChild(i).transform as RectTransform).anchoredPosition = childPosition;
		}
	}

	private Vector2 GetDirectionOnUnitCircle(float angle) {
		// Given angle assume up vector as 0°, but functions take right vector as 0°
		float rotatedAngle = angle + 90;

		// Given angle in degree, functions use radians
		double radiansAngle = rotatedAngle / 180 * Math.PI;

		return new Vector2(
			(float)Math.Cos(radiansAngle),
			(float)Math.Sin(radiansAngle)
		);
	}
}
