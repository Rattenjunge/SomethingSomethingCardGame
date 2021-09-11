using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class HandController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int handCardsLimit = 5;
    [SerializeField] private List<CreatureCard> allCards;

    private List<CreatureCard> cardsInHand;
    //private HorizontalLayoutGroup layoutGroup;

    private void Awake()
    {
        cardsInHand = new List<CreatureCard>();
        // layoutGroup = GetComponent<HorizontalLayoutGroup>();

        for (int i = 0; i < 5; i++)
        {
            ReceiveCard(allCards[i]);
        }
    }

    public void ReceiveCard(CreatureCard newCard)
    {
        if (cardsInHand.Count == handCardsLimit)
        {
            return;
        }

        cardsInHand.Add(newCard);

        var newCardObject = GameObject.Instantiate(cardPrefab, this.transform);
        newCardObject.GetComponent<CardController>().Init(newCard);
    }
}
