using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InstantiatedCard))]
public class PreviewCard : MonoBehaviour {
    [SerializeField] private Image faceImage;
    [SerializeField] private List<AttackScoreController> attackScoreControllers;
    [SerializeField] private List<AttackScoreColor> attackScoreColors;

    private void Awake() {
        attackScoreColors.Sort(AttackScoreColor.CompareByScore);
        attackScoreControllers.ForEach(controller => { controller.SetAttackScoreColors(attackScoreColors); });
    }

    public void Init(CreatureCard creatureCard) {
        GetComponent<InstantiatedCard>().playableCard = creatureCard;
        faceImage.sprite = creatureCard.FaceImage;

        // Set attack values
        foreach (Direction4 direction4 in Enum.GetValues(typeof(Direction4))) {
            attackScoreControllers.Where(controller => controller.direction4 == direction4).First()
                .SetAttackScore(creatureCard.GetAttackPointsByDirection(direction4));
        }
    }
}
