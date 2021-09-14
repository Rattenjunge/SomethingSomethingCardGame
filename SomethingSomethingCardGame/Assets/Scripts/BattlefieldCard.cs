using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattlefieldCard : NetworkBehaviour
{
    [SerializeField] Image borderImage;
    public Sprite OwnedSprite;
    public Sprite LostSprite;
    public uint currentOwner;

    [SerializeField] Image creatureImage;
    [SerializeField] private List<AttackScoreController> attackScoreControllers;
    [SerializeField] private List<AttackScoreColor> attackScoreColors;
    uint originalOwnerId;
    public PlayerManager playerManager;
    CreatureCard creatureCard;

    public CreatureCard CreatureCard { get => creatureCard; set => creatureCard = value; }

    private void Awake()
    {
        attackScoreColors.Sort(AttackScoreColor.CompareByScore);
        attackScoreControllers.ForEach(controller => { controller.SetAttackScoreColors(attackScoreColors); });
    }

    private void Start()
    {
        NetworkIdentity networkIdentity = NetworkClient.connection.identity;
        playerManager = networkIdentity.GetComponent<PlayerManager>();
    }
    [ClientRpc]
    public void RpcSetControl(uint netId)
    {
        //Debug.Log("Set Control " + netId, gameObject);
        currentOwner = netId;
        //Check if netId is client or opponent.
        if (netId == NetworkClient.connection.identity.netId)
        {
            borderImage.sprite = OwnedSprite;
        }
        else
        {
            borderImage.sprite = LostSprite;
        }
       //if (afterFight)
       //{
       //    playerManager.FightOver = true;
       //}
    }
    [ClientRpc]
    public void RpcInit(int creatureID, uint originalOwnerNetId)
    {
        if (hasAuthority)
        {
            RpcSetControl(originalOwnerNetId);
        }
        CreatureCard newCard = FindObjectOfType<PlayerManager>().GetCardFromCardList(creatureID);
        creatureCard = newCard;
        originalOwnerId = originalOwnerNetId;
        GetComponent<InstantiatedCard>().playableCard = newCard;
        creatureImage.sprite = newCard.FaceImage;
       // Debug.Log("New Card = " + newCard);
        foreach (Direction4 direction4 in Enum.GetValues(typeof(Direction4)))
        {
            attackScoreControllers.Where(controller => controller.direction4 == direction4).First()
                .SetAttackScore(newCard.GetAttackPointsByDirection(direction4));
        }
    }

    
}
