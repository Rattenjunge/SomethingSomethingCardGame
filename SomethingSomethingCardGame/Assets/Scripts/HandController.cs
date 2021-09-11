using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private int spaceBetweenCards;
    [SerializeField] private int handCardsLimit = 5;

    private List<PlayableCard> cardsInHand;

    private void Awake()
    {
        cardsInHand = new List<PlayableCard>();
    }

    public void ReceiveCard(PlayableCard newCard)
    {
        if (cardsInHand.Count == handCardsLimit)
        {
            return;
        }

        cardsInHand.Add(newCard);

        //spawn and position card
        var cardNumber = cardsInHand.IndexOf(newCard);
        var newCardObject = GameObject.Instantiate(cardPrefab, this.transform);
        newCardObject.transform.position = new Vector3(spaceBetweenCards * cardNumber, 0, 0);
    }
}
