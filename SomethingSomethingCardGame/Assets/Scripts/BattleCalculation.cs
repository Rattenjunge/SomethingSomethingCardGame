using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCalculation : NetworkBehaviour
{
    BattleCalculation Instance;

    DropZone[,] gridDropZones = new DropZone[4,4];

    private void Start()
    {
        DropZone[] dropZones = FindObjectsOfType<DropZone>();
        foreach(DropZone zone in dropZones)
        {
            gridDropZones[zone.position.x, zone.position.y] = zone;
        }
    }

    [Command]
    public void CmdCalculateBattle(GameObject cellObject, GameObject playedCardObject, uint playerNetId)
    {
        //Calculation worked for the host, even ownership swapped! did not work for the other client tho! 
        DropZone cell = cellObject.GetComponent<DropZone>();
        BattlefieldCard playedCard = playedCardObject.GetComponent<BattlefieldCard>();
        Debug.Log("Played Card: " + playedCard);
        Vector2Int placedPosition = cell.position;
        //calculate NORTH value:
        if(placedPosition.y > 0)
        {
            Vector2Int gridPosition = placedPosition + Vector2Int.down;

            DropZone current = gridDropZones[gridPosition.x, gridPosition.y];
            if (current != null)
            {
                InstantiatedCard currentCard = current.card;
                if(currentCard != null)
                {
                    BattlefieldCard currentBattlefieldCard = currentCard.GetComponent<BattlefieldCard>();
                    if(currentBattlefieldCard!= null)
                    {
                        Debug.Log("Northern Card: " + currentBattlefieldCard.CreatureCard);
                        if (currentBattlefieldCard.CreatureCard.SouthAttack < playedCard.CreatureCard.NorthAttack)
                        {
                            RpcSetOwnership(gridPosition, playerNetId);
                        }
                    }
                }
            }
        }

        //Calculate West value:
        if(placedPosition.x > 0)
        {
            Vector2Int gridPosition = placedPosition + Vector2Int.left;

            DropZone current = gridDropZones[gridPosition.x, gridPosition.y];
            if (current != null)
            {
                InstantiatedCard currentCard = current.card;
                if (currentCard != null)
                {
                    BattlefieldCard currentBattlefieldCard = currentCard.GetComponent<BattlefieldCard>();
                    if (currentBattlefieldCard != null)
                    {
                        Debug.Log("Western Card: " + currentBattlefieldCard.CreatureCard);
                        if (currentBattlefieldCard.CreatureCard.SouthAttack < playedCard.CreatureCard.NorthAttack)
                        {
                            RpcSetOwnership(gridPosition, playerNetId);
                        }
                    }
                }
            }
        }

        //Calculate South value:
        if(placedPosition.y < 3)
        {
            Vector2Int gridPosition = placedPosition + Vector2Int.up;


            DropZone current = gridDropZones[gridPosition.x, gridPosition.y];
            if (current != null)
            {
                InstantiatedCard currentCard = current.card;
                if (currentCard != null)
                {
                    BattlefieldCard currentBattlefieldCard = currentCard.GetComponent<BattlefieldCard>();
                    if (currentBattlefieldCard != null)
                    {
                        Debug.Log("Southern Card: " + currentBattlefieldCard.CreatureCard);
                        if (currentBattlefieldCard.CreatureCard.SouthAttack < playedCard.CreatureCard.NorthAttack)
                        {
                            RpcSetOwnership(gridPosition, playerNetId);
                        }
                    }
                }
            }
        }

        //Calculate East value:
        if (placedPosition.x < 3)
        {
            Vector2Int gridPosition = placedPosition + Vector2Int.right;

            DropZone current = gridDropZones[gridPosition.x, gridPosition.y];
            if (current != null)
            {
                InstantiatedCard currentCard = current.card;
                if (currentCard != null)
                {
                    BattlefieldCard currentBattlefieldCard = currentCard.GetComponent<BattlefieldCard>();
                    if (currentBattlefieldCard != null)
                    {
                        Debug.Log("Eastern Card: " + currentBattlefieldCard.CreatureCard);
                        if (currentBattlefieldCard.CreatureCard.SouthAttack < playedCard.CreatureCard.NorthAttack)
                        {
                            RpcSetOwnership(gridPosition, playerNetId);
                        }
                    }
                }
            }
        }
    }

    [ClientRpc]
    void RpcSetOwnership(Vector2Int gridPosition, uint ownerNetId)
    {
        gridDropZones[gridPosition.x, gridPosition.y].card?.GetComponent<BattlefieldCard>().SetControl(ownerNetId);
    }

}
