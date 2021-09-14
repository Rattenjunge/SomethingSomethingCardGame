using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCalculation : NetworkBehaviour
{
    BattleCalculation Instance;
    private bool Fought = false;
    DropZone[,] gridDropZones = new DropZone[4, 4];
    PlayerManager playerManager;
    private void Start()
    {
        DropZone[] dropZones = FindObjectsOfType<DropZone>();
        foreach (DropZone zone in dropZones)
        {
            gridDropZones[zone.position.x, zone.position.y] = zone;
        }


        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();
    }

    [Command]
    public void CmdCalculateBattle(GameObject cellObject, GameObject playedCardObject, uint playerNetId)
    {
        Fought = false;

        //Calculation worked for the host, even ownership swapped! did not work for the other client tho! 
        DropZone cell = cellObject.GetComponent<DropZone>();
        BattlefieldCard playedCard = playedCardObject.GetComponent<BattlefieldCard>();
        //Debug.Log("Played Card: " + playedCard);
        Vector2Int placedPosition = cell.position;

        // For every direction/neighbour
        foreach (DirectionPack direction in DirectionPack.allDirections)
        {
            Vector2Int neighbourPosition = placedPosition + direction.Vector;

            // Calculate attack
            Direction4 oppositeDirection = GetOppositeDirection(direction.Direction);
            int? neighbourAttack = GetCellAttack(neighbourPosition, oppositeDirection);

            // Invalid cell selected
            if (neighbourAttack == null)
            {
                continue;
            }

            // Calculate winner of fight
            int cleanNeighbourAttack = neighbourAttack ?? 0;
            int playedAttack = playedCard.CreatureCard.GetAttackPointsByDirection(direction.Direction);
            if (cleanNeighbourAttack < playedAttack)
            {
                RpcSetOwnership(neighbourPosition, playerNetId);
                Fought = true;
            }
        }
        // if(!Fought && hasAuthority)
        // {
        //     RpcTellClientFightisOver();
        // }
    }
    private int? GetCellAttack(Vector2Int gridPosition, Direction4 attackDirection)
    {
        if (gridPosition.x < 0 || gridPosition.x > 3 ||
            gridPosition.y < 0 || gridPosition.y > 3)
        {
            return null;
        }

        var dropzone = gridDropZones[gridPosition.x, gridPosition.y];

        // Is dropzone empty?
        if (dropzone == null)
        {
            return null;
        }

        try
        {
            CreatureCard creature = dropzone.card.GetComponent<BattlefieldCard>().CreatureCard;
            return creature.GetAttackPointsByDirection(attackDirection);
        }
        catch
        {
            return null;
        }
    }


    private Direction4 GetOppositeDirection(Direction4 attackDirection)
    {
        switch (attackDirection)
        {
            case Direction4.North:
                return Direction4.South;
            case Direction4.South:
                return Direction4.North;
            case Direction4.East:
                return Direction4.West;
            case Direction4.West:
                return Direction4.East;
            default:
                throw new System.Exception("Not a valid direction.");
        }
    }


    [ClientRpc]
    void RpcSetOwnership(Vector2Int gridPosition, uint ownerNetId)
    {
        gridDropZones[gridPosition.x, gridPosition.y].card?.GetComponent<BattlefieldCard>().RpcSetControl(ownerNetId);
    }

    class DirectionPack
    {
        public Vector2Int Vector;
        public Direction4 Direction;

        public static DirectionPack[] allDirections = new DirectionPack[] {
            new DirectionPack(Vector2Int.down, Direction4.North),
            new DirectionPack(Vector2Int.up, Direction4.South),
            new DirectionPack(Vector2Int.right, Direction4.East),
            new DirectionPack(Vector2Int.left, Direction4.West)
        };

        public DirectionPack(Vector2Int vector, Direction4 direction)
        {
            this.Vector = vector;
            this.Direction = direction;
        }
    }
 //[ClientRpc]
 //void RpcTellClientFightisOver()
 //{
 //    playerManager.FightOver = true;
 //}
 //


  
}
