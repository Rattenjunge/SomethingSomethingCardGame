using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightAtmosphereController : MonoBehaviour
{
    [SerializeField] private Image lightOverlay;
    [SerializeField] private List<Color> colors;
    [SerializeField] private float fastTransitionDuration;
    [SerializeField] private float slowTransitionDuration;

    public Color previousColor;
    public Color nextColor;
    public float duration;
    public float totalTransitionTime;

    public float TransitionProgress { get => totalTransitionTime / duration; }

    private void Awake() {
        lightOverlay.color = ChooseRandomColor();
        StartNewTransition();
    }

    private void StartNewTransition() {
        previousColor = lightOverlay.color;
        nextColor = ChooseRandomColor();
        duration = ChooseRandomTransitionDuration();
        totalTransitionTime = 0;
    }

    private Color ChooseRandomColor() {
        int index = (int)Math.Round(UnityEngine.Random.value * colors.Count - 0.4f);
        return colors[index];
    }

    private float ChooseRandomTransitionDuration() {
        return fastTransitionDuration + UnityEngine.Random.value * (slowTransitionDuration - fastTransitionDuration);
    }

    private void Update() {
        if (TransitionProgress < 1) {
            totalTransitionTime += Time.deltaTime;
            lightOverlay.color = Color.Lerp(previousColor, nextColor, TransitionProgress);
        } else {
            StartNewTransition();
        }
    }
}
