using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    private GameObject playerArea;
    private GameObject dropZone;

    private List<GameObject> cards = new List<GameObject>();
    private List<GameObject> cardDeck = new List<GameObject>();
    public List<GameObject> HandCards = new List<GameObject>();
    public override void OnStartClient()
    {
        base.OnStartClient();
        playerArea = GameObject.FindGameObjectWithTag("PlayerHand");
        dropZone = GameObject.FindGameObjectWithTag("DropZoneBackground");
        foreach (var item in Resources.LoadAll<GameObject>("ScriptableObjects"))
        {
            cards.Add(item);
        }
        for (int i = 0; i < 20; i++)
        {
            cardDeck.Add(cards[Random.Range(0, cards.Count)]);
        }
        
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
    }

    public void DealCards()
    {
        int cardsInHand = HandCards.Count;
        if(HandCards.Count >= 5)
        {
            return;
        }
        for (int i = 0; i < 5-cardsInHand; i++)
        {
            GameObject card = Instantiate(cardDeck[Random.Range(0, cardDeck.Count)], new Vector2(+0, 0),Quaternion.identity);
            HandCards.Add(card);
            cardDeck.Remove(card);
            card.transform.SetParent(playerArea.transform);
        }
    }


}
