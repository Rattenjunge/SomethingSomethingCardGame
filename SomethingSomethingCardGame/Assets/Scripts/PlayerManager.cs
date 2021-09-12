using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class PlayerManager : NetworkBehaviour {
    private GameObject playerArea;
    private TurnIndicationController turnIndicator;

    private List<CreatureCard> cards = new List<CreatureCard>();
    private List<CreatureCard> cardDeck = new List<CreatureCard>();
    public List<GameObject> HandCards = new List<GameObject>();
    public InstantiatedCard BattlefieldCardPrefab;
    public InstantiatedCard HandCardPrefab;
    public BattleCalculation BattleCalculationPrefab;
    private BattleCalculation battleCalculation;

    private Button readyButton;

    private int numberOfPlayers = 0;
    private bool localPlayerHasActiveTurn = false;
    private bool cardsDealtThisTurn = false;
    private List<uint> connectedPlayerIds = new List<uint>();
    //private int localId;

    private int currentActivePlayerIndex = 0;

    public bool IsServerVersion = false;

    public override void OnStartClient()
    {
        Debug.Log("OnStartClient");
        base.OnStartClient();
        playerArea = GameObject.FindGameObjectWithTag("PlayerHand");
        foreach (var item in Resources.LoadAll<CreatureCard>("Creatures"))
        {
            cards.Add(item);
        }
        for (int i = 0; i < 20; i++)
        {

            cardDeck.Add(cards[Random.Range(0, cards.Count)]);
        }

        turnIndicator = GetTurnIndicator();
    }

    private TurnIndicationController GetTurnIndicator() {
        var turnIndicatorObject = GameObject.FindGameObjectWithTag("TurnIndicator");
        if (turnIndicatorObject == null) return null;

        return turnIndicatorObject.GetComponent<TurnIndicationController>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("Test " + NetworkClient.connection.identity.connectionToClient.connectionId);
    }
    
    private void Start()
    {
        if (FindObjectsOfType<PlayerManager>().Length <= 1) //Check if this is the hosts version of the player Manager! 
        {
            IsServerVersion = true;
        }
        else
        {
            IsServerVersion = false;
        }
    }
    [Command]
    public void CmdSpawnBattleCalculator()
    {
        battleCalculation = Instantiate(BattleCalculationPrefab, transform);
        NetworkServer.Spawn(battleCalculation.gameObject, connectionToClient);
    }

    [Command]
    public void CmdPlayerReady(uint nNetId, bool b) //Player has clicked ready, server now checks if all players are ready! 
    {
        if (hasAuthority && IsServerVersion) //Check if playermanager is owned by the server.
        {
            Debug.Log("Cmd Player Ready " + nNetId + " "+ b);
            if (b)
            {
                numberOfPlayers++;
                connectedPlayerIds.Add(nNetId);
                Debug.Log("Server detects client connection, number of players: " + numberOfPlayers);

                if (numberOfPlayers == 2)
                {
                    //Start the game, figure out which player starts, unlock gameplay for that player.
                    RpcRemoveReadyButton();
                    CmdSpawnBattleCalculator();
                    currentActivePlayerIndex = Random.Range(0, 2);
                    foreach (PlayerManager manager in FindObjectsOfType<PlayerManager>())
                    {
                        manager.RpcStartTurn(connectedPlayerIds[currentActivePlayerIndex]);
                    }
                }
            } else
            {
                numberOfPlayers--;
                connectedPlayerIds.Remove(nNetId);
            }
        } else //Find the playermanager that IS owned by the server and call CmdPlayerReady there.
        {
            foreach(PlayerManager manager in FindObjectsOfType<PlayerManager>())
            {
                if (manager.IsServerVersion)
                {
                    manager.CmdPlayerReady(nNetId, b);
                }
            }
        }
    }

    [ClientRpc] 
    public void RpcRemoveReadyButton()
    {
        FindObjectOfType<ReadyButtonController>().gameObject.SetActive(false);
    }


    [ClientRpc] 
    public void RpcStartTurn(uint activePlayerNetId)
    {
        if (!hasAuthority) return;
        Debug.Log("Start Turn " + activePlayerNetId + " ClientConnectionId " + NetworkClient.connection.connectionId, gameObject);
        if (activePlayerNetId == netId)
        {
            //Local Player is active in this turn
            Debug.Log("Start Turn");
            cardsDealtThisTurn = false;
            localPlayerHasActiveTurn = true;
            DealCards();
            foreach(var handCard in HandCards)
            {
                //TODO: Enable cardController click sounds and stuff
                handCard.GetComponent<CardMover>().enabled = true;
            }
            if (turnIndicator != null) {
                turnIndicator.SetTurn(true);
            }

        } else
        {
            //Enemy Player is active in this turn.
            localPlayerHasActiveTurn = false;
            foreach (var handCard in HandCards)
            {
                //TODO: Disable cardController click sounds and stuff
                handCard.GetComponent<CardMover>().enabled = false;
            }
            if (turnIndicator != null) {
                turnIndicator.SetTurn(false);
            }
        }
    }
    
    


    public void DealCards()
    {
        if (!localPlayerHasActiveTurn) return;
        if (cardsDealtThisTurn) return;

        cardsDealtThisTurn = true;

        int cardsInHand = HandCards.Count;
        if (HandCards.Count >= 5)
        {
            return;
        }
        for (int i = 0; i < 5 - cardsInHand; i++)
        {
            InstantiatedCard card = Instantiate(HandCardPrefab, new Vector2(+0, 0), Quaternion.identity);
            int index = Random.Range(0, cardDeck.Count);
            card.playableCard = cardDeck[index];
            card.GetComponent<CardController>().Init(card.playableCard as CreatureCard);
            HandCards.Add(card.gameObject);
            cardDeck.RemoveAt(index);
            card.transform.SetParent(playerArea.transform);
        }
    }

    [Command]
    public void CmdCreateCardOnServer(GameObject dropZoneCell, int creatureID, uint playerNetid)
    {
        //Attention! there is no way to send scriptableobjects with sprites to the server with mirror
        //this is a workaround

        //this action should only be done by the server owned version! 
        if (hasAuthority && IsServerVersion) //Check if playermanager is owned by the server.
        {
            CreatureCard creature = GetCardFromCardList(creatureID);
            
            dropZoneCell.GetComponent<BoxCollider2D>().enabled = false;
            InstantiatedCard serverCard = Instantiate(BattlefieldCardPrefab);

            serverCard.GetComponent<InstantiatedCard>().playableCard = creature;
            dropZoneCell.GetComponent<DropZone>().card = serverCard;
            NetworkServer.Spawn(serverCard.gameObject, connectionToClient);
            RpcMoveCardToDropZoneCell(dropZoneCell, serverCard.gameObject, playerNetid);
            serverCard.GetComponent<BattlefieldCard>().CreatureCard = creature;
            serverCard.GetComponent<BattlefieldCard>().RpcInit(creatureID, playerNetid);

            currentActivePlayerIndex = (currentActivePlayerIndex == 0) ? 1 : 0; //check if currentActive player is 0 or 1 and flip around.
            Debug.Log("New Active Player: " + currentActivePlayerIndex);

            battleCalculation.CmdCalculateBattle(dropZoneCell, serverCard.gameObject, playerNetid);

            foreach (PlayerManager manager in FindObjectsOfType<PlayerManager>())
            {
                manager.RpcStartTurn(connectedPlayerIds[currentActivePlayerIndex]);
            }


            //find winner
            RpcFindWinder();



        }
        else //Find the playermanager that IS owned by the server and call CmdCreateCardOnServer there.
        {
            foreach (PlayerManager manager in FindObjectsOfType<PlayerManager>())
            {
                if (manager.IsServerVersion)
                {
                    manager.CmdCreateCardOnServer(dropZoneCell, creatureID, playerNetid);
                }
            }
        }
        
    }

    //TODO: originalOwnerID isn't used
    [ClientRpc]
    public void RpcMoveCardToDropZoneCell(GameObject dropZoneCell, GameObject serverCard, uint originalOwnerID)
    {
        dropZoneCell.GetComponent<BoxCollider2D>().enabled = false;
        serverCard.transform.SetParent(dropZoneCell.transform);
        serverCard.transform.localPosition = Vector2.zero;
    }

    public CreatureCard GetCardFromCardList(int creatureID)
    {
        CreatureCard creature = null;
        foreach (var item in cards)
        {
            if(item.Id == creatureID)
            {
                creature = item;
            }
        }
        return creature;
    }

    [ClientRpc]
    private void RpcFindWinder()
    {
        
        DropZone[] dropZones = FindObjectsOfType<DropZone>();
        
        //if there is an empty dropzone, the game isn't over
        for (int i = 0; i < dropZones.Length; i++)
        {
            if (dropZones[i].transform.childCount == 0)
                return;
        }

        int playerScore = 0;
        int enemyScore = 0;

        for(int i = 0; i < dropZones.Length; i++)
        {
            if (dropZones[i].transform.GetChild(0).GetComponent<BattlefieldCard>().currentOwner == NetworkClient.connection.identity.netId)
                playerScore++;
            else
                enemyScore++;
        }

        if(playerScore > enemyScore)
        {
            Debug.Log("YOU WIN");
        }
        else if(playerScore < enemyScore)
        {
            Debug.Log("YOU LOSE");
        }
        else
        {
            Debug.Log("DRAW");
        }

    }
}
