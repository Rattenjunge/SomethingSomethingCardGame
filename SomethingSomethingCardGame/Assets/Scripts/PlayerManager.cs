using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    private GameObject playerArea;
    private GameObject dropZone;

    private List<CreatureCard> cards = new List<CreatureCard>();
    private List<CreatureCard> cardDeck = new List<CreatureCard>();
    public List<GameObject> HandCards = new List<GameObject>();
    public InstantiatedCard CardPrefab;
    public override void OnStartClient()
    {
        base.OnStartClient();
        playerArea = GameObject.FindGameObjectWithTag("PlayerHand");
        dropZone = GameObject.FindGameObjectWithTag("DropZoneBackground");
        foreach (var item in Resources.LoadAll<CreatureCard>("Creatures"))
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
            InstantiatedCard card = Instantiate(CardPrefab, new Vector2(+0, 0),Quaternion.identity);
            int index = Random.Range(0, cardDeck.Count);
            card.playableCard = cardDeck[index];
            HandCards.Add(card.gameObject);
            cardDeck.RemoveAt(index);
            card.transform.SetParent(playerArea.transform);
        }
    }

    [Command]
    public void CmdCreateCardOnServer( GameObject dropZoneCell, PlayableCard creature)
    {
        dropZoneCell.GetComponent<BoxCollider2D>().enabled = false;
        InstantiatedCard serverCard = Instantiate(CardPrefab,dropZoneCell.transform);
        serverCard.playableCard = creature;
        serverCard.transform.localPosition = Vector2.zero;
        dropZoneCell.GetComponent<DropZone>().card = serverCard;
        NetworkServer.Spawn(serverCard.gameObject, connectionToClient);
    }
}
